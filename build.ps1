#Requires -Version 7.0

<#
.SYNOPSIS
    Build script for LogicMonitor.Api

.DESCRIPTION
    This script builds the LogicMonitor.Api solution, runs tests, and optionally publishes to NuGet.
    It ensures the Git repository is clean before publishing.

.PARAMETER Configuration
    The build configuration (Debug or Release). Default is Release.

.PARAMETER SkipTests
    Skip running unit tests.

.PARAMETER SkipClean
    Skip cleaning the solution before building.

.PARAMETER Publish
    Publish the packages to NuGet after successful build and tests.

.PARAMETER SkipGitCheck
    Skip the Git porcelain check (not recommended for publishing).

.EXAMPLE
    .\build.ps1
    Builds the solution in Release mode and runs tests.

.EXAMPLE
    .\build.ps1 -Configuration Debug -SkipTests
    Builds in Debug mode without running tests.

.EXAMPLE
    .\build.ps1 -Publish
    Builds, tests, and publishes to NuGet (requires nuget-key.txt).

.EXAMPLE
    .\build.ps1 -SkipClean -SkipTests
    Quick build without cleaning or testing.
#>

[CmdletBinding()]
param(
    [Parameter()]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",
    
    [Parameter()]
    [switch]$SkipTests,
    
    [Parameter()]
    [switch]$SkipClean,
    
    [Parameter()]
    [switch]$Publish,
    
    [Parameter()]
    [switch]$SkipGitCheck
)

$ErrorActionPreference = "Stop"
$ScriptDir = $PSScriptRoot

# ANSI color codes
$ColorReset = "`e[0m"
$ColorGreen = "`e[32m"
$ColorYellow = "`e[33m"
$ColorRed = "`e[31m"
$ColorCyan = "`e[36m"
$ColorGray = "`e[90m"

function Write-Step {
    param([string]$Message)
    Write-Host "${ColorCyan}==>${ColorReset} ${Message}" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "${ColorGreen}?${ColorReset} ${Message}" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "${ColorGray}  ${Message}${ColorReset}" -ForegroundColor Gray
}

function Write-Warning {
    param([string]$Message)
    Write-Host "${ColorYellow}?${ColorReset} ${Message}" -ForegroundColor Yellow
}

function Write-ErrorMessage {
    param([string]$Message)
    Write-Host "${ColorRed}?${ColorReset} ${Message}" -ForegroundColor Red
}

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

# Check for nbgv
if (-not (Test-CommandExists "nbgv")) {
    Write-Warning "nbgv (Nerdbank.GitVersioning CLI) not found. Installing..."
    try {
        dotnet tool install -g nbgv
        Write-Success "Installed nbgv"
    }
    catch {
        Write-ErrorMessage "Failed to install nbgv: $_"
        exit 1
    }
}

# Get version from nbgv
try {
    $nbgvVersion = nbgv get-version -v Version
    $versionParts = $nbgvVersion.Split(".")
    $version = "$($versionParts[0]).$($versionParts[1]).$($versionParts[2])"
    Write-Success "Building version: $version"
}
catch {
    Write-Warning "Could not get version from nbgv: $_"
    $version = "unknown"
}

# ============================================================================
# Step 2: Check Git Status (if publishing)
# ============================================================================

if ($Publish -and -not $SkipGitCheck) {
    Write-Step "Checking Git repository status..."
    
    try {
        # Pull latest changes
        Write-Info "Pulling latest changes..."
        git pull
        if ($LASTEXITCODE -ne 0) {
            Write-ErrorMessage "Git pull failed"
            exit 1
        }
        
        # Check if repository is clean
        $gitStatus = git status --porcelain
        if ($gitStatus) {
            Write-ErrorMessage "Git repository is not clean. Uncommitted changes detected:"
            Write-Host ""
            git status --short
            Write-Host ""
            Write-Host "Please commit or stash your changes before publishing." -ForegroundColor Yellow
            Write-Host "To skip this check, use the -SkipGitCheck parameter (not recommended)." -ForegroundColor Gray
            exit 1
        }
        
        Write-Success "Git repository is clean"
    }
    catch {
        Write-ErrorMessage "Failed to check Git status: $_"
        exit 1
    }
}
elseif ($Publish) {
    Write-Warning "Skipping Git repository check as requested"
}

# ============================================================================
# Step 3: Clean Solution (optional)
# ============================================================================

if (-not $SkipClean) {
    Write-Step "Cleaning solution..."
    
    try {
        dotnet clean --configuration $Configuration --verbosity quiet
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Clean completed successfully"
        }
        else {
            Write-Warning "Clean completed with warnings"
        }
    }
    catch {
        Write-ErrorMessage "Failed to clean: $_"
        exit 1
    }
}
else {
    Write-Warning "Skipping clean step"
}

# ============================================================================
# Step 4: Restore Dependencies
# ============================================================================

Write-Step "Restoring NuGet packages..."

try {
    dotnet restore --verbosity quiet
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Restore completed successfully"
    }
    else {
        Write-ErrorMessage "Restore failed"
        exit 1
    }
}
catch {
    Write-ErrorMessage "Failed to restore: $_"
    exit 1
}

# ============================================================================
# Step 5: Build Solution
# ============================================================================

Write-Step "Building solution in $Configuration configuration..."

try {
    $buildArgs = @(
        "build"
        "--configuration", $Configuration
        "--no-restore"
        "/p:TreatWarningsAsErrors=false"
    )
    
    & dotnet @buildArgs
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Build completed successfully"
    }
    else {
        Write-ErrorMessage "Build failed"
        exit 1
    }
}
catch {
    Write-ErrorMessage "Failed to build: $_"
    exit 1
}

# ============================================================================
# Step 6: Run Tests (optional)
# ============================================================================

if (-not $SkipTests) {
    Write-Step "Running unit tests..."
    
    try {
        dotnet test --configuration $Configuration --no-build --verbosity normal --logger "console;verbosity=normal"
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "All tests passed"
        }
        else {
            Write-ErrorMessage "Tests failed"
            Write-Host ""
            Write-Host "To skip tests, run with -SkipTests parameter" -ForegroundColor Yellow
            exit 1
        }
    }
    catch {
        Write-ErrorMessage "Failed to run tests: $_"
        exit 1
    }
}
else {
    Write-Warning "Skipping unit tests as requested"
}

# ============================================================================
# Step 7: Display Build Summary
# ============================================================================

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Success "Build completed successfully!"
Write-Host "  Version:       $version" -ForegroundColor Gray
Write-Host "  Configuration: $Configuration" -ForegroundColor Gray
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# ============================================================================
# Step 8: Publish to NuGet (if requested)
# ============================================================================

if ($Publish) {
    Write-Host ""
    Write-Step "Publishing to NuGet..."
    
    # Check for publish script
    $publishScript = Join-Path $ScriptDir "publish.ps1"
    if (-not (Test-Path $publishScript)) {
        Write-ErrorMessage "publish.ps1 not found at: $publishScript"
        exit 1
    }
    
    # Run publish script
    $publishArgs = @()
    if ($SkipTests) {
        $publishArgs += "-SkipTests"
    }
    if ($SkipGitCheck) {
        $publishArgs += "-SkipGitCheck"
    }
    
    Write-Info "Running publish script..."
    & pwsh -File $publishScript @publishArgs
    
    if ($LASTEXITCODE -ne 0) {
        Write-ErrorMessage "Publish failed"
        exit 1
    }
}
else {
    Write-Info "To publish to NuGet, run with -Publish parameter"
}

Write-Host ""
Write-Success "Build script completed successfully!"
Write-Host ""
