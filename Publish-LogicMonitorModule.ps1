# Publish-LogicMonitorModule.ps1
# Script to prepare and publish the LogicMonitor PowerShell module

param(
    [Parameter(Mandatory = $true)]
    [string]$Version,
    
    [string]$Repository = "PSGallery",
    
    [string]$ApiKey,
    
    [switch]$WhatIf,
    
    [switch]$SkipBuild,
    
    [switch]$LocalOnly
)

$ErrorActionPreference = "Stop"

Write-Information "Preparing LogicMonitor PowerShell Module v$Version for publishing..." -ForegroundColor Green

# Paths
$rootPath = $PSScriptRoot
$projectPath = Join-Path $rootPath "LogicMonitor.PowerShell"
$manifestPath = Join-Path $projectPath "LogicMonitor.psd1"
$publishPath = Join-Path $rootPath "publish"
$modulePublishPath = Join-Path $publishPath "LogicMonitor"

try {
    # Step 1: Build the project
    if (-not $SkipBuild) {
        Write-Information "Building LogicMonitor.PowerShell project..." -ForegroundColor Yellow
 
        Push-Location $rootPath
     $buildResult = dotnet build "LogicMonitor.PowerShell/LogicMonitor.PowerShell.csproj" -c Release
        Pop-Location
        
      if ($LASTEXITCODE -ne 0) {
      throw "Build failed with exit code $LASTEXITCODE"
  }
        Write-Information "Build completed successfully." -ForegroundColor Green
    }
    
    # Step 2: Update module version
    Write-Information "Updating module manifest version to $Version..." -ForegroundColor Yellow
    
    $manifestContent = Get-Content $manifestPath -Raw
    $manifestContent = $manifestContent -replace "ModuleVersion = '[^']*'", "ModuleVersion = '$Version'"
    Set-Content $manifestPath -Value $manifestContent -Encoding UTF8
    
    # Step 3: Test module manifest
    Write-Information "Testing module manifest..." -ForegroundColor Yellow
    $testResult = Test-ModuleManifest -Path $manifestPath
    if (-not $testResult) {
      throw "Module manifest test failed"
    }
    Write-Information "Module manifest is valid." -ForegroundColor Green
    
  # Step 4: Prepare publishing directory
    Write-Information "Preparing module for publishing..." -ForegroundColor Yellow
    
    if (Test-Path $publishPath) {
     Remove-Item $publishPath -Recurse -Force
    }
    New-Item -ItemType Directory -Path $modulePublishPath -Force | Out-Null
    
    # Copy built assemblies
    $binPath = Join-Path $projectPath "bin\Release\net9.0"
    if (-not (Test-Path $binPath)) {
  throw "Build output not found at $binPath. Please build the project first."
    }
    
    Copy-Item (Join-Path $binPath "*") $modulePublishPath -Recurse -Force
    
    # Copy module files
    Copy-Item $manifestPath $modulePublishPath -Force
    Copy-Item (Join-Path $projectPath "LogicMonitor.psm1") $modulePublishPath -Force
    
    # Copy documentation if exists
    $readmePath = Join-Path $projectPath "README.md"
    if (Test-Path $readmePath) {
        Copy-Item $readmePath $modulePublishPath -Force
    }
    
    # Step 5: Test the prepared module
    Write-Information "Testing prepared module..." -ForegroundColor Yellow
    
    $preparedManifestPath = Join-Path $modulePublishPath "LogicMonitor.psd1"
    Test-ModuleManifest -Path $preparedManifestPath | Out-Null
    
    # Import and test commands
    Import-Module $preparedManifestPath -Force
    $commands = Get-Command -Module LogicMonitor
    Write-Information "Module loaded successfully with $($commands.Count) commands." -ForegroundColor Green
    
    # Step 6: Create local package
    $packagePath = Join-Path $publishPath "LogicMonitor-PowerShell-v$Version.zip"
    Compress-Archive -Path (Join-Path $modulePublishPath "*") -DestinationPath $packagePath -Force
  Write-Information "Created local package: $packagePath" -ForegroundColor Green
    
    if ($LocalOnly) {
        Write-Information "Local package created. Skipping repository publish as requested." -ForegroundColor Yellow
     return
    }
  
    # Step 7: Publish to repository
    if ($WhatIf) {
      Write-Information "WhatIf: Would publish to repository '$Repository'" -ForegroundColor Cyan
        Write-Information "Module Path: $modulePublishPath" -ForegroundColor Cyan
 Write-Information "Version: $Version" -ForegroundColor Cyan
    } else {
        if ([string]::IsNullOrEmpty($ApiKey)) {
   if ($Repository -eq "PSGallery") {
      throw "API key is required for PowerShell Gallery. Get one from https://www.powershellgallery.com/account/apikeys"
      } else {
         Write-Warning "No API key provided. Attempting to publish without explicit key..."
     }
        }
  
        Write-Information "Publishing module to repository '$Repository'..." -ForegroundColor Yellow
      
        $publishParams = @{
   Path = $modulePublishPath
     Repository = $Repository
     Verbose = $true
   }
        
    if (-not [string]::IsNullOrEmpty($ApiKey)) {
        $publishParams.NuGetApiKey = $ApiKey
        }
        
   Publish-Module @publishParams
    Write-Information "Module published successfully!" -ForegroundColor Green
    }
    
} catch {
    Write-Error "Publishing failed: $($_.Exception.Message)"
  Write-Error $_.ScriptStackTrace
  exit 1
} finally {
    # Cleanup: Remove module from session
    Remove-Module LogicMonitor -ErrorAction SilentlyContinue
}

Write-Information "Publishing process completed!" -ForegroundColor Green