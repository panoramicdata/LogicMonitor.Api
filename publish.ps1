#Requires -Version 7.0

<#
.SYNOPSIS
 Builds and publishes LogicMonitor.Api packages to NuGet.

.DESCRIPTION
    This script performs the following steps:
    1. Validates that nuget-key.txt exists
 2. Ensures Git working directory is clean (porcelain)
    3. Cleans previous build artifacts
    4. Builds the solution in Release mode with symbols
    5. Runs unit tests (can be skipped with -SkipTests)
    6. Publishes packages to NuGet

.PARAMETER SkipTests
    Skip running unit tests before publishing.

.PARAMETER NuGetSource
    The NuGet source to publish to. Defaults to https://api.nuget.org/v3/index.json

.PARAMETER DryRun
    Performs all steps except the actual publish to NuGet.

.PARAMETER SkipGitCheck
    Skip the Git porcelain check (not recommended for production releases).

.EXAMPLE
    .\publish.ps1
    Builds, tests, and publishes the packages.

.EXAMPLE
    .\publish.ps1 -SkipTests
    Builds and publishes without running tests.

.EXAMPLE
    .\publish.ps1 -DryRun
    Performs all steps except publishing to NuGet.

.EXAMPLE
    .\publish.ps1 -SkipGitCheck
    Skips the Git working directory check.
#>

[CmdletBinding()]
param(
    [Parameter()]
    [switch]$SkipTests,

    [Parameter()]
    [string]$NuGetSource = "https://api.nuget.org/v3/index.json",

    [Parameter()]
    [switch]$DryRun,

    [Parameter()]
    [switch]$SkipGitCheck
)

$ErrorActionPreference = "Stop"
$ScriptDir = $PSScriptRoot

# ANSI color codes for better output
$ColorReset = "`e[0m"
$ColorGreen = "`e[32m"
$ColorYellow = "`e[33m"
$ColorRed = "`e[31m"
$ColorCyan = "`e[36m"

function Write-Step {
  param([string]$Message)
  Write-Host "${ColorCyan}==>${ColorReset} ${Message}" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "${ColorGreen}?${ColorReset} ${Message}" -ForegroundColor Green
}

function Write-Warning {
    param([string]$Message)
    Write-Host "${ColorYellow}?${ColorReset} ${Message}" -ForegroundColor Yellow
}

function Write-ErrorMessage {
    param([string]$Message)
    Write-Host "${ColorRed}?${ColorReset} ${Message}" -ForegroundColor Red
}

# Function to check if a command exists
function Test-CommandExists {
    param([string]$Command)
    $null -ne (Get-Command $Command -ErrorAction SilentlyContinue)
}

# ============================================================================
# Step 1: Validate Prerequisites
# ============================================================================

Write-Step "Validating prerequisites..."

# Check for dotnet CLI
if (-not (Test-CommandExists "dotnet")) {
    Write-ErrorMessage "dotnet CLI not found. Please install .NET SDK."
    exit 1
}

$dotnetVersion = dotnet --version
Write-Success "Found dotnet CLI version: $dotnetVersion"

# Check for Git
if (-not (Test-CommandExists "git")) {
    Write-ErrorMessage "git CLI not found. Please install Git."
    exit 1
}

$gitVersion = git --version
Write-Success "Found $gitVersion"

# Check for nuget-key.txt
$NuGetKeyFile = Join-Path $ScriptDir "nuget-key.txt"
if (-not (Test-Path $NuGetKeyFile)) {
    Write-ErrorMessage "nuget-key.txt not found in solution root: $ScriptDir"
    Write-Host "Please create nuget-key.txt with your NuGet API key."
    exit 1
}

$NuGetApiKey = (Get-Content $NuGetKeyFile -Raw).Trim()
if ([string]::IsNullOrWhiteSpace($NuGetApiKey)) {
    Write-ErrorMessage "nuget-key.txt is empty. Please add your NuGet API key."
    exit 1
}

Write-Success "Found nuget-key.txt with API key"

# ============================================================================
# Step 2: Check Git Working Directory is Clean
# ============================================================================

if (-not $SkipGitCheck) {
    Write-Step "Checking Git working directory status..."

    try {
        # Check if we're in a Git repository
   $isGitRepo = git rev-parse --is-inside-work-tree 2>$null
 
        if ($isGitRepo -eq "true") {
# Get current branch
    $currentBranch = git rev-parse --abbrev-ref HEAD
          Write-Host "  Current branch: $currentBranch" -ForegroundColor Gray
    
            # Check for uncommitted changes
            $gitStatus = git status --porcelain
     
  if ($gitStatus) {
       Write-ErrorMessage "Git working directory is not clean. Uncommitted changes detected:"
                Write-Host ""
      git status --short
         Write-Host ""
    Write-Host "Please commit or stash your changes before publishing." -ForegroundColor Yellow
 Write-Host "To skip this check, use the -SkipGitCheck parameter (not recommended)." -ForegroundColor Gray
                exit 1
            }
     
            # Check for unpushed commits
            $unpushedCommits = git log origin/$currentBranch..$currentBranch --oneline 2>$null
   if ($unpushedCommits) {
  Write-Warning "You have unpushed commits:"
            Write-Host ""
     Write-Host $unpushedCommits -ForegroundColor Yellow
    Write-Host ""
       Write-Host "Consider pushing your changes before publishing." -ForegroundColor Yellow
      
     # Prompt user to continue
  $response = Read-Host "Continue anyway? (y/N)"
    if ($response -ne 'y' -and $response -ne 'Y') {
  Write-Host "Publishing cancelled." -ForegroundColor Yellow
     exit 0
        }
            }
       
      Write-Success "Git working directory is clean"
        } else {
            Write-Warning "Not in a Git repository - skipping Git checks"
}
    } catch {
        Write-Warning "Could not check Git status: $_"
   Write-Host "Continuing anyway..." -ForegroundColor Gray
    }
} else {
    Write-Warning "Skipping Git working directory check as requested"
}

# ============================================================================
# Step 3: Clean Previous Build Artifacts
# ============================================================================

Write-Step "Cleaning previous build artifacts..."

try {
    dotnet clean --configuration Release --verbosity quiet
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Clean completed successfully"
    } else {
   Write-Warning "Clean completed with warnings"
    }
} catch {
    Write-ErrorMessage "Failed to clean: $_"
    exit 1
}

# ============================================================================
# Step 4: Restore Dependencies
# ============================================================================

Write-Step "Restoring NuGet packages..."

try {
    dotnet restore --verbosity quiet
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Restore completed successfully"
    } else {
      Write-ErrorMessage "Restore failed"
        exit 1
    }
} catch {
    Write-ErrorMessage "Failed to restore: $_"
    exit 1
}

# ============================================================================
# Step 5: Build Solution in Release Mode with Symbols
# ============================================================================

Write-Step "Building solution in Release mode with symbols..."

try {
    dotnet build --configuration Release --no-restore /p:IncludeSymbols=true /p:SymbolPackageFormat=snupkg
    if ($LASTEXITCODE -eq 0) {
    Write-Success "Build completed successfully"
    } else {
        Write-ErrorMessage "Build failed"
     exit 1
    }
} catch {
    Write-ErrorMessage "Failed to build: $_"
    exit 1
}

# ============================================================================
# Step 6: Run Unit Tests
# ============================================================================

if (-not $SkipTests) {
Write-Step "Running unit tests..."
    
    try {
    dotnet test --configuration Release --no-build --verbosity normal --logger "console;verbosity=normal"
  if ($LASTEXITCODE -eq 0) {
            Write-Success "All tests passed"
        } else {
     Write-ErrorMessage "Tests failed"
        Write-Host ""
            Write-Host "To skip tests and publish anyway, run with -SkipTests parameter" -ForegroundColor Yellow
            exit 1
        }
} catch {
  Write-ErrorMessage "Failed to run tests: $_"
        exit 1
    }
} else {
    Write-Warning "Skipping unit tests as requested"
}

# ============================================================================
# Step 7: Pack NuGet Packages
# ============================================================================

Write-Step "Creating NuGet packages..."

try {
    dotnet pack --configuration Release --no-build --include-symbols -p:SymbolPackageFormat=snupkg --output "$ScriptDir/nupkgs"
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Packages created successfully"
    } else {
 Write-ErrorMessage "Pack failed"
        exit 1
    }
} catch {
    Write-ErrorMessage "Failed to pack: $_"
    exit 1
}

# ============================================================================
# Step 8: List Created Packages
# ============================================================================

$PackageDir = Join-Path $ScriptDir "nupkgs"
$NuGetPackages = Get-ChildItem -Path $PackageDir -Filter "*.nupkg" -Exclude "*.symbols.nupkg"
$SymbolPackages = Get-ChildItem -Path $PackageDir -Filter "*.snupkg"

Write-Host ""
Write-Step "Created packages:"
foreach ($package in $NuGetPackages) {
  Write-Host "  ?? $($package.Name)" -ForegroundColor White
}
foreach ($package in $SymbolPackages) {
    Write-Host "  ?? $($package.Name)" -ForegroundColor Gray
}
Write-Host ""

# ============================================================================
# Step 9: Publish to NuGet
# ============================================================================

if ($DryRun) {
    Write-Warning "DRY RUN MODE: Skipping publish to NuGet"
    Write-Host ""
    Write-Host "Packages are ready in: $PackageDir"
    Write-Host "To publish, run without -DryRun parameter"
    exit 0
}

Write-Step "Publishing packages to NuGet ($NuGetSource)..."
Write-Host ""

$publishSuccess = $true

foreach ($package in $NuGetPackages) {
    Write-Host "Publishing: $($package.Name)" -ForegroundColor Cyan
    
    try {
        dotnet nuget push $package.FullName `
  --api-key $NuGetApiKey `
   --source $NuGetSource `
    --skip-duplicate
        
        if ($LASTEXITCODE -eq 0) {
     Write-Success "Published: $($package.Name)"
        } else {
            Write-ErrorMessage "Failed to publish: $($package.Name)"
            $publishSuccess = $false
        }
    } catch {
        Write-ErrorMessage "Error publishing $($package.Name): $_"
        $publishSuccess = $false
    }
    
    Write-Host ""
}

# ============================================================================
# Summary
# ============================================================================

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
if ($publishSuccess) {
    Write-Success "Publish completed successfully!"
    Write-Host ""
    Write-Host "Your packages have been published to NuGet." -ForegroundColor Green
    Write-Host "It may take a few minutes for them to appear in search results." -ForegroundColor Gray
} else {
    Write-ErrorMessage "Publish completed with errors"
    Write-Host ""
    Write-Host "Some packages failed to publish. Please review the errors above." -ForegroundColor Yellow
    exit 1
}
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
