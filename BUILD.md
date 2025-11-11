# Build Scripts

This directory contains automated build and publish scripts for the LogicMonitor.Api solution.

## Scripts Overview

### `build.ps1`
Main build script for building, testing, and optionally publishing the solution.

### `publish.ps1`
Dedicated publishing script that handles Git validation, building, testing, and NuGet publishing.

## Prerequisites

- **.NET SDK** (latest version recommended)
- **PowerShell 7.0+**
- **Git**
- **nbgv** (Nerdbank.GitVersioning CLI) - automatically installed if missing
- **nuget-key.txt** file in solution root (for publishing)

## build.ps1

### Usage

```powershell
# Standard build with tests
.\build.ps1

# Debug build without tests
.\build.ps1 -Configuration Debug -SkipTests

# Quick rebuild (no clean, no tests)
.\build.ps1 -SkipClean -SkipTests

# Build and publish to NuGet
.\build.ps1 -Publish

# Build and publish without Git checks (not recommended)
.\build.ps1 -Publish -SkipGitCheck
```

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Configuration` | String | `Release` | Build configuration (Debug or Release) |
| `SkipTests` | Switch | `false` | Skip running unit tests |
| `SkipClean` | Switch | `false` | Skip cleaning the solution before building |
| `Publish` | Switch | `false` | Publish packages to NuGet after successful build |
| `SkipGitCheck` | Switch | `false` | Skip Git repository validation (not recommended) |

### What It Does

1. **Validates Prerequisites** - Checks for dotnet, git, and nbgv
2. **Gets Version** - Retrieves version from Nerdbank.GitVersioning
3. **Checks Git Status** (if publishing) - Ensures repository is clean
4. **Cleans Solution** (optional) - Removes previous build artifacts
5. **Restores Dependencies** - Downloads NuGet packages
6. **Builds Solution** - Compiles in specified configuration
7. **Runs Tests** (optional) - Executes unit tests
8. **Publishes** (optional) - Calls publish.ps1 if -Publish is specified

## publish.ps1

### Usage

```powershell
# Standard publish with tests and Git checks
.\publish.ps1

# Skip tests (not recommended)
.\publish.ps1 -SkipTests

# Dry run (everything except actual publish)
.\publish.ps1 -DryRun

# Skip Git validation (not recommended)
.\publish.ps1 -SkipGitCheck

# Custom NuGet source
.\publish.ps1 -NuGetSource "https://my-nuget-server.com/v3/index.json"
```

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `SkipTests` | Switch | `false` | Skip running unit tests |
| `NuGetSource` | String | `https://api.nuget.org/v3/index.json` | NuGet source URL |
| `DryRun` | Switch | `false` | Perform all steps except actual publish |
| `SkipGitCheck` | Switch | `false` | Skip Git porcelain check |

### What It Does

1. **Validates Prerequisites** - Checks for dotnet, git, and nuget-key.txt
2. **Checks Git Status** - Ensures working directory is clean (porcelain)
   - Detects uncommitted changes
   - Warns about unpushed commits
   - Prompts for confirmation
3. **Cleans Solution** - Removes previous build artifacts
4. **Restores Dependencies** - Downloads NuGet packages
5. **Builds Solution** - Compiles in Release mode with symbols (.snupkg)
6. **Runs Tests** - Executes all unit tests
7. **Creates Packages** - Generates NuGet packages with symbols
8. **Lists Packages** - Shows all created packages
9. **Publishes to NuGet** - Uploads packages using API key

### Git Checks

The publish script performs comprehensive Git validation:

#### Uncommitted Changes
```
? Git working directory is not clean. Uncommitted changes detected:

 M LogicMonitor.Api/LogicMonitor.Api.csproj
 
Please commit or stash your changes before publishing.
```
**Action**: Blocks publishing until changes are committed

#### Unpushed Commits
```
? You have unpushed commits:

abc1234 Add new feature
def5678 Fix bug

Consider pushing your changes before publishing.
Continue anyway? (y/N)
```
**Action**: Warns and prompts for confirmation

## NuGet API Key Setup

Create a `nuget-key.txt` file in the solution root:

```powershell
# Create the file
New-Item -Path nuget-key.txt -ItemType File

# Add your API key (get from https://www.nuget.org/account/apikeys)
Set-Content -Path nuget-key.txt -Value "your-api-key-here"
```

**Important**: This file is in `.gitignore` and should never be committed to version control.

## Versioning

Both scripts use **Nerdbank.GitVersioning** (nbgv) for automatic version management:

- **Version Source**: `version.json` in solution root
- **Format**: `{Major}.{Minor}.{Height}+{CommitHash}`
- **Example**: `3.229.9+4162cbb62b`

The version is automatically calculated based on:
- Base version in `version.json`
- Git commit height since last version tag
- Current Git commit hash

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Build and Publish

on:
  push:
    branches: [ main ]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0  # Required for nbgv
          
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
          
      - name: Build
        run: pwsh -File build.ps1
        
      - name: Publish to NuGet
        if: github.ref == 'refs/heads/main'
        env:
          NUGET_API_KEY: ${{ secrets.NUGET_API_KEY }}
        run: |
          echo $env:NUGET_API_KEY | Out-File -FilePath nuget-key.txt -Encoding utf8
          pwsh -File publish.ps1 -SkipGitCheck
```

### Azure Pipelines Example

```yaml
trigger:
  branches:
    include:
      - main

pool:
  vmImage: 'windows-latest'

steps:
- task: UseDotNet@2
  inputs:
    version: '10.0.x'
    
- pwsh: |
    .\build.ps1 -Publish -SkipGitCheck
  displayName: 'Build and Publish'
  env:
    NUGET_API_KEY: $(NuGetApiKey)
```

## Common Workflows

### Local Development Build
```powershell
# Quick build for development
.\build.ps1 -SkipTests -SkipClean
```

### Pre-Commit Validation
```powershell
# Full build with tests
.\build.ps1
```

### Release Process
```powershell
# 1. Ensure you're on main branch
git checkout main
git pull

# 2. Update version.json if needed
# Edit version.json to bump major/minor version

# 3. Commit version change
git add version.json
git commit -m "Bump version to 3.230"
git push

# 4. Run publish script
.\publish.ps1

# The script will:
# - Check Git is clean
# - Build in Release
# - Run all tests
# - Create packages
# - Publish to NuGet
```

## Troubleshooting

### "nuget-key.txt not found"
**Solution**: Create the file in solution root with your NuGet API key

### "Git working directory is not clean"
**Solution**: Commit or stash your changes before publishing

### "Tests failed"
**Solution**: Fix failing tests or use `-SkipTests` (not recommended for publishing)

### "nbgv not found"
**Solution**: The script will attempt to install it automatically, or install manually:
```powershell
dotnet tool install -g nbgv
```

### Build artifacts in wrong location
**Solution**: Run with `-SkipClean` removed to ensure clean state

## Output Locations

```
LogicMonitor.Api/
??? nupkgs/                          # Published packages
?   ??? LogicMonitor.Api.3.229.9.nupkg
?   ??? LogicMonitor.Api.3.229.9.snupkg
?   ??? LogicMonitor.PowerShell.3.229.9.nupkg
?   ??? LogicMonitor.PowerShell.3.229.9.snupkg
??? LogicMonitor.Api/bin/Release/
?   ??? netstandard2.0/
?       ??? LogicMonitor.Api.dll
??? LogicMonitor.PowerShell/bin/Release/
?   ??? net10.0/
?       ??? LogicMonitor.PowerShell.dll
?       ??? LogicMonitor/            # PowerShell module output
??? LogicMonitor.Api.Test/bin/Release/
    ??? net10.0/
        ??? LogicMonitor.Api.Test.dll
```

## Best Practices

1. **Always run tests** before publishing (never use `-SkipTests` for releases)
2. **Keep Git clean** - commit all changes before publishing
3. **Use DryRun** first to verify everything works
4. **Version bumps** should be explicit commits to `version.json`
5. **API keys** should never be committed to source control
6. **Symbols** (.snupkg) are automatically included for debugging support

## Related Files

- `version.json` - Version configuration for Nerdbank.GitVersioning
- `global.json` - .NET SDK version pinning
- `.gitignore` - Excludes nuget-key.txt and build artifacts
- `nuget.config` - NuGet package sources configuration (if exists)
