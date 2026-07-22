<#
.SYNOPSIS
    Convert an existing LogicMonitor resource into an INTERNAL ping check
    (ResourceType.Ping / deviceType 19), reusing the resource's current
    preferred collector and preserving its configuration properties.

.DESCRIPTION
    A LogicMonitor device's deviceType is immutable once created, so a resource
    cannot be "edited" into a ping check in place. This script therefore performs
    the conversion the same way the LM UI / New-LMUptimeDevice cmdlet do:

        1. SNAPSHOT the existing resource (name, displayName, description,
           preferred collector, static group membership, alerting state and all
           user-set custom properties). A JSON backup is written to disk first.
        2. HARD-DELETE the existing resource (LM enforces unique display names,
           so the old device must go before the new one can take its name).
        3. RECREATE it via  POST /device/devices?type=uptimepingcheck  with the
           v3 structured body (X-Version: 3, model=websiteDevice, isInternal=true).
           The server then registers it as an Uptime resource
           (system.device.provider=Uptime, system.uptime.type=internal) and
           automatically applies the Ping_Check_Overall / Ping_Check_Individual
           DataSources so data starts collecting.

    The new internal ping check runs on the SAME preferred collector as the
    original resource (testLocation.collectorIds = [<preferredCollectorId>]).

    NO CONFIGURATION DATA IS LOST: name, host, description, static group
    membership, alerting state and every user-set custom property are carried
    across. What CANNOT survive is historical time-series (graph) data, because
    a deviceType change requires a delete + recreate — that is an inherent
    LogicMonitor limitation, not a limitation of this script.

    NOTE ON SECRETS: the LogicMonitor API always returns custom-property values
    that are marked as secret/password as "********" (masked). This script
    detects masked values and DROPS them from the recreate rather than writing
    the literal "********" back (which would corrupt the property). Any dropped
    properties are listed in the output so they can be re-entered by hand.

.PARAMETER Account
    LogicMonitor portal subdomain (the "xxxx" in xxxx.logicmonitor.com),
    e.g. "ralphlauren".

.PARAMETER AccessId
    LMv1 API token Access ID.

.PARAMETER AccessKey
    LMv1 API token Access Key. Accepts a plain string or a SecureString.
    If omitted, the script falls back to the LM_ACCESS_KEY environment variable.

.PARAMETER ResourceId
    Id of the resource to convert. Provide this OR -DisplayName.

.PARAMETER DisplayName
    Display name of the resource to convert. Provide this OR -ResourceId.
    Must match exactly one resource.

.PARAMETER PollingIntervalMinutes
    Ping check polling interval, in minutes. Default 10 (matches the existing
    RAL ping-check estate).

.PARAMETER PacketCount
    Number of ICMP packets per poll. Default 5.

.PARAMETER TimeoutMs
    Per-packet timeout, in milliseconds. Default 500.

.PARAMETER PercentPacketsNotReceivedThreshold
    Percentage of packets that may be lost before the check alerts. Default 80.

.PARAMETER OverallAlertLevel
    Overall alert level: warn | error | critical. Default warn.

.PARAMETER IndividualAlertLevel
    Per-checkpoint alert level: warn | error | critical. Default warn.

.PARAMETER BackupDirectory
    Directory for the pre-delete JSON snapshot. Default: script folder.

.PARAMETER DryRun
    Show exactly what would be done (target, collector, groups, carried
    properties) without deleting or creating anything.

.EXAMPLE
    .\Convert-ResourceToInternalPingCheck.ps1 -Account ralphlauren `
        -AccessId <id> -AccessKey <key> -DisplayName "8.8.8.8" -DryRun

.EXAMPLE
    .\Convert-ResourceToInternalPingCheck.ps1 -Account ralphlauren `
        -AccessId <id> -AccessKey <key> -ResourceId 123456

.NOTES
    Panoramic Data - CDW / Ralph Lauren (Epic PS-2766, ticket PS-3101).
    Pure PowerShell + .NET; no external modules required. Works in Windows
    PowerShell 5.1 and PowerShell 7+.
#>
[CmdletBinding(SupportsShouldProcess, ConfirmImpact = 'High')]
param(
    [Parameter(Mandatory)] [string] $Account,
    [Parameter(Mandatory)] [string] $AccessId,
    [Parameter()]          [object] $AccessKey,

    [Parameter()] [int]    $ResourceId,
    [Parameter()] [string] $DisplayName,

    [Parameter()] [int]    $PollingIntervalMinutes = 10,
    [Parameter()] [int]    $PacketCount = 5,
    [Parameter()] [int]    $TimeoutMs = 500,
    [Parameter()] [int]    $PercentPacketsNotReceivedThreshold = 80,

    [Parameter()] [ValidateSet('warn', 'error', 'critical')] [string] $OverallAlertLevel = 'warn',
    [Parameter()] [ValidateSet('warn', 'error', 'critical')] [string] $IndividualAlertLevel = 'warn',

    [Parameter()] [string] $BackupDirectory = $PSScriptRoot,
    [Parameter()] [switch] $DryRun
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# TLS 1.2 for Windows PowerShell 5.1 (PowerShell 7 negotiates automatically).
try { [Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12 } catch {}

# --- Validate resource selector -------------------------------------------------
if (-not $ResourceId -and [string]::IsNullOrWhiteSpace($DisplayName)) {
    throw "Specify the resource to convert with -ResourceId <id> or -DisplayName <name>."
}
if ($ResourceId -and -not [string]::IsNullOrWhiteSpace($DisplayName)) {
    throw "Specify only one of -ResourceId or -DisplayName, not both."
}

# --- Resolve the access key -----------------------------------------------------
function ConvertTo-PlainText([object] $Value) {
    if ($null -eq $Value) { return $null }
    if ($Value -is [System.Security.SecureString]) {
        $bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($Value)
        try { return [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr) }
        finally { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr) }
    }
    return [string] $Value
}

$accessKeyPlain = ConvertTo-PlainText $AccessKey
if ([string]::IsNullOrWhiteSpace($accessKeyPlain)) { $accessKeyPlain = $env:LM_ACCESS_KEY }
if ([string]::IsNullOrWhiteSpace($accessKeyPlain)) {
    throw "No access key supplied. Pass -AccessKey or set the LM_ACCESS_KEY environment variable."
}

$baseUri = "https://$Account.logicmonitor.com/santaba/rest"

# --- LMv1-signed request helper -------------------------------------------------
function Invoke-LMApi {
    param(
        [Parameter(Mandatory)] [ValidateSet('GET', 'POST', 'PUT', 'DELETE')] [string] $Method,
        [Parameter(Mandatory)] [string] $ResourcePath,   # e.g. /device/devices  (NO query string)
        [string] $Query = '',                            # e.g. ?type=uptimepingcheck (NOT signed)
        [string] $Body = ''
    )

    $epoch = [DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds().ToString()
    # LMv1 signs Method + epoch + body + resourcePath (query string excluded).
    $requestVars = "$Method$epoch$Body$ResourcePath"

    $hmac = [System.Security.Cryptography.HMACSHA256]::new([Text.Encoding]::UTF8.GetBytes($accessKeyPlain))
    try {
        $hashBytes = $hmac.ComputeHash([Text.Encoding]::UTF8.GetBytes($requestVars))
    } finally { $hmac.Dispose() }
    $hashHex   = ([BitConverter]::ToString($hashBytes) -replace '-', '').ToLowerInvariant()
    $signature = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($hashHex))

    $headers = @{
        'Authorization' = "LMv1 $AccessId`:$signature`:$epoch"
        'X-Version'     = '3'
        'Accept'        = 'application/json'
    }

    $uri = "$baseUri$ResourcePath$Query"
    $iwrArgs = @{
        Uri         = $uri
        Method      = $Method
        Headers     = $headers
        ContentType = 'application/json'
        ErrorAction = 'Stop'
    }
    if ($Body) { $iwrArgs['Body'] = $Body }

    try {
        $resp = Invoke-WebRequest @iwrArgs -UseBasicParsing
        if ($resp.Content) { return $resp.Content | ConvertFrom-Json }
        return $null
    }
    catch {
        $detail = $null
        if ($_.ErrorDetails -and $_.ErrorDetails.Message) { $detail = $_.ErrorDetails.Message }
        elseif ($_.Exception.Response) {
            try {
                $stream = $_.Exception.Response.GetResponseStream()
                $reader = [IO.StreamReader]::new($stream)
                $detail = $reader.ReadToEnd()
            } catch {}
        }
        throw "LogicMonitor API $Method $ResourcePath failed: $($_.Exception.Message)`n$detail"
    }
}

function ConvertTo-JsonCompact([object] $Value) {
    # -Compress keeps the signed body identical to what we send on the wire.
    return ($Value | ConvertTo-Json -Depth 25 -Compress)
}

Write-Host ("=" * 78)
Write-Host "Convert resource -> INTERNAL ping check   (portal: $Account)"
Write-Host ("=" * 78)
if ($DryRun) { Write-Host "[DRY RUN] No changes will be made." -ForegroundColor Yellow }

# --- 1. Locate the resource -----------------------------------------------------
if ($ResourceId) {
    Write-Host "Fetching resource id $ResourceId ..."
    $resource = Invoke-LMApi -Method GET -ResourcePath "/device/devices/$ResourceId" -Query "?_=$([DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds())"
}
else {
    Write-Host "Locating resource by display name '$DisplayName' ..."
    $escaped = $DisplayName.Replace('"', '\"')
    $result  = Invoke-LMApi -Method GET -ResourcePath "/device/devices" -Query "?filter=displayName:`"$escaped`""
    $items   = @($result.items)
    if ($items.Count -eq 0) { throw "No resource found with display name '$DisplayName'." }
    if ($items.Count -gt 1) { throw "Ambiguous: $($items.Count) resources match display name '$DisplayName'. Use -ResourceId." }
    $resource = $items[0]
}

$resId          = [int]   $resource.id
$resName        = [string]$resource.name
$resDisplayName = [string]$resource.displayName
$resDescription = if ($resource.PSObject.Properties['description']) { [string]$resource.description } else { '' }
$preferredColl  = [int]   $resource.preferredCollectorId
$deviceType     = [int]   $resource.deviceType
$disableAlerting = [bool]($resource.PSObject.Properties['disableAlerting'] -and $resource.disableAlerting)

Write-Host ""
Write-Host "  Id                 : $resId"
Write-Host "  Name (host)        : $resName"
Write-Host "  DisplayName        : $resDisplayName"
Write-Host "  Current deviceType : $deviceType $([string]::Format('({0})', $(if($deviceType -eq 19){'already an internal ping check!'}else{'regular device'})))"
Write-Host "  PreferredCollector : $preferredColl"
Write-Host "  DisableAlerting    : $disableAlerting"

if ($deviceType -eq 19) {
    Write-Warning "Resource $resId already has deviceType 19 (ping check). Nothing to convert."
    return
}
if ($preferredColl -le 0) {
    throw "Resource $resId has no preferred collector (preferredCollectorId=$preferredColl). An internal ping check requires a collector."
}

# --- 2. Determine static group membership to preserve ---------------------------
# Only Normal (static) groups without an AppliesTo can be assigned explicitly;
# dynamic / AppliesTo groups re-derive themselves from properties after recreate.
$hostGroupIds = @()
if ($resource.PSObject.Properties['hostGroupIds'] -and $resource.hostGroupIds) {
    $hostGroupIds = @($resource.hostGroupIds.ToString().Split(',') | Where-Object { $_ } | ForEach-Object { [int]$_ })
}

$staticGroupIds = New-Object System.Collections.Generic.List[int]
foreach ($gid in $hostGroupIds) {
    try {
        $g = Invoke-LMApi -Method GET -ResourcePath "/device/groups/$gid" -Query "?fields=id,name,groupType,appliesTo&_=$([DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds())"
        $isDynamic = ($g.PSObject.Properties['appliesTo'] -and -not [string]::IsNullOrWhiteSpace([string]$g.appliesTo))
        $isNormal  = (-not $g.PSObject.Properties['groupType']) -or ($g.groupType -eq 'Normal')
        if ($isNormal -and -not $isDynamic) { $staticGroupIds.Add([int]$g.id) }
    }
    catch { Write-Warning "  Could not inspect group $gid ($($_.Exception.Message)); skipping it." }
}
if ($staticGroupIds.Count -eq 0) { $staticGroupIds.Add(1) | Out-Null }  # fall back to root

# --- 3. Carry over user-set custom properties -----------------------------------
# Exclude auto/system/uptime/ping-check properties (server regenerates those) and
# drop masked secret values (the API returns "********" and re-sending it corrupts).
$reservedPrefixes = @('system.', 'auto.', 'predef.', 'website.private.', 'uptime.', 'Ping_Check_Individual.', 'Ping_Check_Overall.')
$carried = New-Object System.Collections.Generic.List[object]
$maskedDropped = New-Object System.Collections.Generic.List[string]

$sourceProps = @()
if ($resource.PSObject.Properties['customProperties'] -and $resource.customProperties) { $sourceProps = @($resource.customProperties) }

foreach ($p in $sourceProps) {
    $pname = [string]$p.name
    if ([string]::IsNullOrWhiteSpace($pname)) { continue }
    if ($reservedPrefixes | Where-Object { $pname.StartsWith($_, [StringComparison]::OrdinalIgnoreCase) }) { continue }
    $pval = if ($p.PSObject.Properties['value']) { [string]$p.value } else { '' }
    if ($pval -eq '********') { $maskedDropped.Add($pname) | Out-Null; continue }
    $carried.Add([ordered]@{ name = $pname; value = $pval }) | Out-Null
}

Write-Host ""
Write-Host "  Static groups to reassign : [$([string]::Join(', ', $staticGroupIds))]"
Write-Host "  Collector (testLocation)  : [$preferredColl]  (same as current preferred collector)"
Write-Host "  User properties carried   : $($carried.Count)"
if ($carried.Count -gt 0) { Write-Host "    $([string]::Join(', ', ($carried | ForEach-Object { $_.name })))" }
if ($maskedDropped.Count -gt 0) {
    Write-Warning "  $($maskedDropped.Count) masked (secret) propert$([string]$(if($maskedDropped.Count -eq 1){'y'}else{'ies'})) could not be read back and will be DROPPED (re-enter manually):"
    Write-Warning "    $([string]::Join(', ', $maskedDropped))"
}

# --- 4. Snapshot backup ---------------------------------------------------------
if (-not (Test-Path -LiteralPath $BackupDirectory)) { New-Item -ItemType Directory -Path $BackupDirectory -Force | Out-Null }
$stamp      = Get-Date -Format 'yyyyMMdd-HHmmss'
$backupFile = Join-Path $BackupDirectory ("pingcheck-conversion-backup-{0}-{1}.json" -f $resId, $stamp)
$resource | ConvertTo-Json -Depth 25 | Out-File -LiteralPath $backupFile -Encoding utf8
Write-Host ""
Write-Host "  Backup snapshot written   : $backupFile"

# --- Build the recreate body (v3 structured internal ping check) ----------------
# Build the sub-collections explicitly (avoid generic-List pipeline binding quirks).
[string[]] $groupIdStrings = @()
foreach ($gid in $staticGroupIds) { $groupIdStrings += ([string][int]$gid) }

[int[]] $collectorIdArray = @([int]$preferredColl)

$propArray = @()
foreach ($p in $carried) { $propArray += @{ name = [string]$p.name; value = [string]$p.value } }

$createBody = [ordered]@{
    id                          = 0
    type                        = 'uptimepingcheck'
    model                       = 'websiteDevice'
    deviceType                  = 19
    name                        = $resName
    displayName                 = $resDisplayName
    description                 = $resDescription
    groupIds                    = $groupIdStrings
    isInternal                  = $true
    disableAlerting             = $disableAlerting
    pollingInterval             = $PollingIntervalMinutes
    host                        = $resName
    count                       = $PacketCount
    percentPktsNotReceiveInTime = $PercentPacketsNotReceivedThreshold
    timeoutInMSPktsNotReceive   = $TimeoutMs
    transition                  = 1
    overallAlertLevel           = $OverallAlertLevel
    individualAlertLevel        = $IndividualAlertLevel
    individualSmAlertEnable     = $false
    useDefaultAlertSetting      = $true
    useDefaultLocationSetting   = $false
    globalSmAlertCond           = 0
    testLocation                = [ordered]@{ collectorIds = $collectorIdArray; smgIds = @() }
    properties                  = $propArray
}
$createJson = ConvertTo-JsonCompact $createBody

if ($DryRun) {
    Write-Host ""
    Write-Host "[DRY RUN] Would hard-delete resource $resId then POST /device/devices?type=uptimepingcheck with body:" -ForegroundColor Yellow
    Write-Host ($createBody | ConvertTo-Json -Depth 25)
    Write-Host ""
    Write-Host "[DRY RUN] Complete. No changes made." -ForegroundColor Yellow
    return
}

# --- 5. Hard-delete then recreate -----------------------------------------------
$target = "resource $resId ('$resDisplayName') -> internal ping check on collector $preferredColl"
if (-not $PSCmdlet.ShouldProcess($target, "Hard-delete and recreate as internal ping check")) {
    Write-Host "Aborted by user / -WhatIf." -ForegroundColor Yellow
    return
}

Write-Host ""
Write-Host "  Hard-deleting resource $resId ..." -ForegroundColor Cyan
Invoke-LMApi -Method DELETE -ResourcePath "/device/devices/$resId" -Query "?deleteHard=true" | Out-Null

Write-Host "  Creating internal ping check ..." -ForegroundColor Cyan
$created = Invoke-LMApi -Method POST -ResourcePath "/device/devices" -Query "?type=uptimepingcheck" -Body $createJson
$newId = [int]$created.id
Write-Host "  Created new resource id $newId (deviceType $([int]$created.deviceType))." -ForegroundColor Green

# --- 6. Verify ------------------------------------------------------------------
Write-Host ""
Write-Host "  Verifying ..."
$verify = Invoke-LMApi -Method GET -ResourcePath "/device/devices/$newId" -Query "?_=$([DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds())"
$uptimeType = ''
if ($verify.PSObject.Properties['systemProperties'] -and $verify.systemProperties) {
    $ut = @($verify.systemProperties | Where-Object { $_.name -eq 'system.uptime.type' })
    if ($ut.Count -gt 0) { $uptimeType = [string]$ut[0].value }
}
Write-Host "    deviceType         : $([int]$verify.deviceType)  (expected 19)"
Write-Host "    system.uptime.type : '$uptimeType'  (expected 'internal'; may take a moment to populate)"

$ok = ([int]$verify.deviceType -eq 19)
Write-Host ""
if ($ok) {
    Write-Host "SUCCESS: resource converted to internal ping check (new id $newId)." -ForegroundColor Green
    Write-Host "Backup of the original resource: $backupFile"
} else {
    Write-Warning "Conversion completed but verification did not confirm deviceType 19. Check the portal (new id $newId)."
}

[PSCustomObject]@{
    OriginalId        = $resId
    NewId             = $newId
    DisplayName       = $resDisplayName
    Host              = $resName
    CollectorId       = $preferredColl
    DeviceType        = [int]$verify.deviceType
    UptimeType        = $uptimeType
    PropertiesCarried = $carried.Count
    PropertiesDropped = $maskedDropped.Count
    BackupFile        = $backupFile
}
