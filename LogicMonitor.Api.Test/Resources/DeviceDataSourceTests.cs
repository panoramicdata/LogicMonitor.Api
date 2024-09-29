namespace LogicMonitor.Api.Test.Resources;

public class DeviceDataSourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAllDeviceDataSourcesAsync()
	{
		var deviceDataSources = await LogicMonitorClient
			.GetAllDeviceDataSourcesAsync(1765, null, default)
			.ConfigureAwait(true);
		deviceDataSources.Should().NotBeNull();
	}

	[Fact]
	public async Task DeviceDataSourceTests2()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);

		// Get all windows devices in the datacenter
		var resources = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(DeviceGroupFullPath, true, default)
			.ConfigureAwait(true);
		resources.Should().NotBeNullOrEmpty();
		// We have devices

		// Find datacenter devices with WinCPU datasource
		var deviceDataSources = new List<DeviceDataSource>();
		foreach (var device in resources)
		{
			if (dataSource != null)
			{
				var deviceDataSourceByDeviceIdAndDataSourceId = await LogicMonitorClient
					.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, default)
					.ConfigureAwait(true);
				if (deviceDataSourceByDeviceIdAndDataSourceId is null)
				{
					continue;
				}

				deviceDataSources.Add(deviceDataSourceByDeviceIdAndDataSourceId);
			}
		}

		deviceDataSources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceDatasourceData()
	{
		var deviceDataSources = await LogicMonitorClient
			.GetAllDeviceDataSourcesAsync(WindowsDeviceId, null, default)
			.ConfigureAwait(true);

		var data = await LogicMonitorClient
			.GetDeviceDataSourceDataAsync(WindowsDeviceId, deviceDataSources[0].Id, default)
			.ConfigureAwait(true);

		data.Should().NotBeNull();
	}
}
