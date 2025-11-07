# Import required assemblies
Add-Type -Path "$PSScriptRoot\LogicMonitor.Api.dll"
Add-Type -Path "$PSScriptRoot\LogicMonitor.PowerShell.dll"

# Module variables
$script:LogicMonitorClient = $null
$script:ConnectionInfo = $null

# Helper function to ensure client is connected
function Test-ClientConnection {
    if ($null -eq $script:LogicMonitorClient) {
        throw "Not connected to LogicMonitor. Please run Connect-LogicMonitor first."
    }
}

# Helper function to convert hashtable filter to LogicMonitor filter
function ConvertTo-LMFilter {
    param(
     [hashtable]$Filter,
        [string]$TypeName
   )
    
   if ($null -eq $Filter -or $Filter.Count -eq 0) {
      return $null
   }
    
   # This is a simplified filter conversion
   # In a real implementation, you'd want to properly convert
   # PowerShell hashtables to LogicMonitor Filter objects
    return $null
}

# Export module members
Export-ModuleMember -Function @(
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