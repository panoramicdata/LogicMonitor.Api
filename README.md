# LogicMonitor.Api

The LogicMonitor REST API nuget package, authored by Panoramic Data Limited.

[![Nuget](https://img.shields.io/nuget/v/LogicMonitor.Api)](https://www.nuget.org/packages/LogicMonitor.Api/)
[![Nuget](https://img.shields.io/nuget/dt/LogicMonitor.Api)](https://www.nuget.org/packages/LogicMonitor.Api/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/c35eed8f289a4e11bcf1fd63dd5271ce)](https://www.codacy.com/gh/panoramicdata/LogicMonitor.Api/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=panoramicdata/LogicMonitor.Api&amp;utm_campaign=Badge_Grade)

If you want some LogicMonitor software developed, come find us at: https://www.panoramicdata.com/ !

To get started, watch the videos here:

http://www.panoramicdata.com/products/logicmonitor-api-nuget-package/

A simple example:

```c#
using LogicMonitor.Api;

[...]

public static async Task GetAllResources(ILogger logger, CancellationToken cancellationToken)
{
	using var logicMonitorClient = new LogicMonitorClient(
		new LogicMonitorClientOptions
		{
			Account = "acme",
			AccessId = "accessId",
			AccessKey = "accessKey",
			Logger = logger
		}
	);

	var resources = await logicMonitorClient
		.GetAllAsync<Resource>(cancellationToken)
		.ConfigureAwait(false);

	Console.WriteLine($"Resource Count: {resources.Count}");
}
```

## LogicModule Export/Import (JSON Format)

The modern LogicMonitor UI exports LogicModules to JSON format. This library supports both JSON and XML export/import:

```c#
// Export a DataSource as JSON (modern UI format)
var json = await logicMonitorClient
	.GetDataSourceJsonAsync(dataSourceId, cancellationToken);

// Export a DataSource as XML (legacy format)
var xml = await logicMonitorClient
	.GetDataSourceXmlAsync(dataSourceId, cancellationToken);

// Generic export by LogicModuleType
var json = await logicMonitorClient
	.GetLogicModuleJsonAsync(LogicModuleType.DataSource, dataSourceId, cancellationToken);

// Export to a file
await logicMonitorClient
	.ExportLogicModuleToJsonFileAsync(
		LogicModuleType.DataSource,
		dataSourceId,
		"datasource.json",
		cancellationToken);

// Import from JSON string
var imported = await logicMonitorClient
	.ImportDataSourceJsonAsync(json, cancellationToken);

// Import from file
var imported = await logicMonitorClient
	.ImportDataSourceFromJsonFileAsync("datasource.json", cancellationToken);
```

Supported LogicModule types for export/import:
- DataSource
- EventSource
- ConfigSource
- PropertySource
- TopologySource
- JobMonitor
- AppliesToFunction

## API Documentation

For more information on the LogicMonitor REST API, see the [official documentation](https://www.logicmonitor.com/support/rest-api-developers-guide/overview/using-logicmonitors-rest-api/).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
