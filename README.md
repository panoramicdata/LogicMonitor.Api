# LogicMonitor.Api

The LogicMonitor REST API nuget package, authored by Panoramic Data Limited.

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
