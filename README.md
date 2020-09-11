# LogicMonitor.Api

The LogicMonitor REST API nuget package, authored by Panoramic Data Limited.

If you want some LogicMonitor software developed, come find us at: https://www.panoramicdata.com/ !

To get started, watch the videos here:

http://www.panoramicdata.com/products/logicmonitor-api-nuget-package/

A simple example:

```c#
using LogicMonitor.Api;

[...]

public async Task GetAllDevices()
{
	var portalClient = new PortalClient("acme", "accessTokenId", "accessTokenKey");
	var devices = await portalClient.GetAllAsync<Device>().ConfigureAwait(false);
	Console.WriteLine($"Device Count: {devices.Count}");
}
