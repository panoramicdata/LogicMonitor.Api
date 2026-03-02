param(
	[string]$RootPath = "./LogicMonitor.Api.Test/EventLogs/UnhandledLogs",
	[int]$First = 0
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $RootPath))
{
	throw "Path not found: $RootPath"
}

$files = Get-ChildItem -LiteralPath $RootPath -File | Sort-Object Name
$allMessages = New-Object 'System.Collections.Generic.List[object]'

foreach ($file in $files)
{
	$lines = Get-Content -LiteralPath $file.FullName
	$currentBlock = New-Object 'System.Collections.Generic.List[string]'

	function Add-BlockMessage {
		param(
			[System.Collections.Generic.List[string]]$Block,
			[string]$SourceFile,
			[System.Collections.Generic.List[object]]$OutputList
		)

		if ($Block.Count -lt 2)
		{
			return
		}

		$message = ($Block | Select-Object -Skip 1) -join [Environment]::NewLine
		$message = $message.Trim()

		if ([string]::IsNullOrWhiteSpace($message))
		{
			return
		}

		$OutputList.Add([pscustomobject]@{
			File = $SourceFile
			Message = $message
		})
	}

	foreach ($line in $lines)
	{
		if ($line -like '-----*')
		{
			Add-BlockMessage -Block $currentBlock -SourceFile $file.Name -OutputList $allMessages
			$currentBlock.Clear()
			continue
		}

		$currentBlock.Add($line)
	}

	Add-BlockMessage -Block $currentBlock -SourceFile $file.Name -OutputList $allMessages
}

$selected = if ($First -gt 0) { $allMessages | Select-Object -First $First } else { $allMessages }

$index = 1
foreach ($item in $selected)
{
	Write-Output "[$index] $($item.File)"
	Write-Output $item.Message
	Write-Output ('-' * 80)
	$index++
}

Write-Output "Total messages: $($allMessages.Count)"
