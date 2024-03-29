# This script will publish to nuget using the api key in nuget-api-key.txt in the same folder.
# The api key issued by nuget.org should ideally only have permissions to update a single package
# with new versions.

$apiKeyFilename = "nuget-api-key.txt";
if(-not (Test-Path($apiKeyFilename))){
	Write-Output "$apiKeyFilename does not exist"
	exit 1;
}
$apiKey = Get-Content $apiKeyFilename;

# Build and test
dotnet build -c Release
dotnet build ..\LogicMonitor.Api.Test -c Debug
#dotnet test ..\LogicMonitor.Api.Test -c Debug
if ($lastexitcode -ne 0) {
	Write-Error "One or more tests failed. Aborting..."
	exit 1;
}

dotnet pack -c Release

$mostRecentPackage = Get-ChildItem bin\Release\*.nupkg | Sort-Object LastWriteTime | Select-Object -last 1
Write-Output "Publishing $mostRecentPackage..."
.\nuget.exe push -Source https://api.nuget.org/v3/index.json -ApiKey $apiKey "$mostRecentPackage"