# LogicMonitor PowerShell Module

This PowerShell module provides a wrapper around the LogicMonitor.Api .NET library, enabling you to interact with LogicMonitor's REST API using PowerShell cmdlets.

## Installation

### From Local Build
1. Clone the repository
2. Build the solution
3. Import the module:
```powershell
Import-Module .\LogicMonitor.PowerShell\bin\Debug\net9.0\LogicMonitor
```

### From PowerShell Gallery (when published)
```powershell
Install-Module -Name LogicMonitor
```

## Configuration

Before using the module, you need to configure your LogicMonitor credentials:

```powershell
# Connect to LogicMonitor
Connect-LogicMonitor -Account "yourcompany" -AccessId "your-access-id" -AccessKey "your-access-key"
```

## Basic Usage Examples

### Get All Resources
```powershell
# Get all monitored resources
$resources = Get-LMResource

# Get resources with specific filter
$webServers = Get-LMResource -Filter @{ Name = "*web*" }
```

### Get Alerts
```powershell
# Get all active alerts
$alerts = Get-LMAlert

# Get critical alerts only
$criticalAlerts = Get-LMAlert -Level "critical"
```

### Get Dashboards
```powershell
# Get all dashboards
$dashboards = Get-LMDashboard

# Get specific dashboard by name
$dashboard = Get-LMDashboard -Name "Infrastructure Overview"
```

### Resource Management
```powershell
# Create a new resource
$newResource = New-LMResource -Name "MyServer" -DisplayName "My Server" -ResourceGroupId 1

# Update resource properties
Set-LMResourceProperty -ResourceId 123 -Name "custom.property" -Value "some value"

# Remove a resource
Remove-LMResource -ResourceId 123
```

### Data Collection
```powershell
# Get performance data
$data = Get-LMResourceData -ResourceId 123 -DataSourceName "CPU" -StartTime (Get-Date).AddHours(-1)

# Get raw data for specific instance
$rawData = Get-LMRawData -ResourceId 123 -DataSourceId 456 -InstanceId 789 -StartTime (Get-Date).AddDays(-1)
```

## Available Cmdlets

### Connection Management
- `Connect-LogicMonitor` - Establish connection to LogicMonitor API
- `Disconnect-LogicMonitor` - Close connection to LogicMonitor API
- `Test-LMConnection` - Test current connection status

### Resource Management
- `Get-LMResource` - Retrieve resources
- `New-LMResource` - Create new resource
- `Set-LMResource` - Update resource
- `Remove-LMResource` - Delete resource
- `Get-LMResourceGroup` - Retrieve resource groups
- `New-LMResourceGroup` - Create new resource group

### Alert Management
- `Get-LMAlert` - Retrieve alerts
- `Set-LMAlert` - Update alert (acknowledge, etc.)
- `Get-LMAlertRule` - Retrieve alert rules

### Dashboard Management
- `Get-LMDashboard` - Retrieve dashboards
- `Get-LMDashboardGroup` - Retrieve dashboard groups

### Data Collection
- `Get-LMResourceData` - Get performance data
- `Get-LMRawData` - Get raw metric data
- `Get-LMGraphData` - Get graph data

### User Management
- `Get-LMUser` - Retrieve users
- `New-LMUser` - Create new user
- `Set-LMUser` - Update user
- `Remove-LMUser` - Delete user

### Collector Management
- `Get-LMCollector` - Retrieve collectors
- `Get-LMCollectorGroup` - Retrieve collector groups

## Error Handling

The module provides consistent error handling with meaningful error messages:

```powershell
try {
    $resource = Get-LMResource -Id 999999
}
catch {
    Write-Error "Failed to retrieve resource: $($_.Exception.Message)"
}
```

## Advanced Usage

### Custom Filters
```powershell
# Complex filtering
$filter = @{
    Name = "*prod*"
    Type = "Server"
    Status = "Normal"
}
$resources = Get-LMResource -Filter $filter
```

### Bulk Operations
```powershell
# Process multiple resources
$resources = Get-LMResource -Filter @{ Group = "Production" }
foreach ($resource in $resources) {
    Set-LMResourceProperty -ResourceId $resource.Id -Name "environment" -Value "prod"
}
```

## Contributing

Contributions are welcome! Please see the main repository for contribution guidelines.

## License

This project is licensed under the MIT License - see the LICENSE file for details.