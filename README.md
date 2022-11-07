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

public static async Task GetAllDevices(ILogger logger, CancellationToken cancellationToken)
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

	var devices = await logicMonitorClient
		.GetAllAsync<Device>(cancellationToken)
		.ConfigureAwait(false);

	Console.WriteLine($"Device Count: {devices.Count}");
}
