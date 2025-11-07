@{
 RootModule = 'LogicMonitor.psm1'
    ModuleVersion = '1.0.0'
    GUID = '12345678-1234-1234-1234-123456789abc'
    Author = 'Panoramic Data Limited'
    CompanyName = 'Panoramic Data Limited'
    Copyright = 'Copyright (c) Panoramic Data Limited 2025'
    Description = 'PowerShell module for LogicMonitor API interactions'
    PowerShellVersion = '5.1'
    RequiredAssemblies = @(
        'LogicMonitor.Api.dll',
        'LogicMonitor.PowerShell.dll',
        'Microsoft.Extensions.Logging.dll',
        'Microsoft.Extensions.Logging.Console.dll',
        'Microsoft.Extensions.Logging.Abstractions.dll',
        'Newtonsoft.Json.dll'
    )
    CmdletsToExport = @(
        'Connect-LogicMonitor',
 'Disconnect-LogicMonitor',
  'Test-LMConnection',
 'Get-LMResource',
        'New-LMResource',
        'Set-LMResource',
      'Remove-LMResource',
        'Get-LMResourceGroup',
        'New-LMResourceGroup',
        'Set-LMResourceGroup',
        'Remove-LMResourceGroup',
   'Get-LMAlert',
        'Set-LMAlert',
        'Get-LMAlertRule',
        'Get-LMDashboard',
        'Get-LMDashboardGroup',
      'Get-LMResourceData',
  'Get-LMRawData',
      'Get-LMGraphData',
'Get-LMUser',
    'New-LMUser',
        'Set-LMUser',
        'Remove-LMUser',
    'Get-LMCollector',
        'Get-LMCollectorGroup',
        'Set-LMResourceProperty',
        'Get-LMResourceProperty',
   'Remove-LMResourceProperty',
     'Get-LMDataSource',
      'Get-LMWebsite',
    'Get-LMWebsiteGroup'
    )
    FunctionsToExport = @()
    VariablesToExport = @()
    AliasesToExport = @()
    PrivateData = @{
        PSData = @{
        Tags = @('LogicMonitor', 'API', 'Monitoring', 'Infrastructure', 'REST')
            LicenseUri = 'https://github.com/panoramicdata/LogicMonitor.Api/blob/main/LICENSE'
         ProjectUri = 'https://github.com/panoramicdata/LogicMonitor.Api'
   RequiredModules = @()
            ExternalModuleDependencies = @()
  }
    }
}