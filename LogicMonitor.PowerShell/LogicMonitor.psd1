@{
 RootModule = 'LogicMonitor.psm1'
    ModuleVersion = 'zzzzzzzzzz'
    GUID = '12345678-1234-1234-1234-123456789abc'
    Author = 'Panoramic Data Limited'
    CompanyName = 'Panoramic Data Limited'
    Copyright = 'Copyright (c) Panoramic Data Limited 2025'
    Description = 'PowerShell module for LogicMonitor API interactions'
    PowerShellVersion = '5.1'
    NestedModules = @('LogicMonitor.PowerShell.dll')
    RequiredAssemblies = @(
        'LogicMonitor.Api.dll',
        'LogicMonitor.PowerShell.dll',
        'Microsoft.Extensions.Logging.dll',
        'Microsoft.Extensions.Logging.Console.dll',
        'Microsoft.Extensions.Logging.Abstractions.dll',
        'Newtonsoft.Json.dll'
    )
    CmdletsToExport = @('*')
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
