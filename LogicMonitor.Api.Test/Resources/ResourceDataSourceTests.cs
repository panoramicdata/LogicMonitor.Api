namespace LogicMonitor.Api.Test.Resources;

public class ResourceDataSourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllResourceDataSourcesAsync()
	{
		var deviceDataSources = await LogicMonitorClient
			.GetAllResourceDataSourcesAsync(1765, null, CancellationToken);
		deviceDataSources.Should().NotBeNull();
	}

	[Fact]
	public async Task ResourceDataSourceTests2()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", CancellationToken);

		// Get all windows devices in the datacenter
		var resources = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken);
		resources.Should().NotBeNullOrEmpty();
		// We have devices

		// Find datacenter devices with WinCPU datasource
		var resourceDataSources = new List<ResourceDataSource>();
		foreach (var device in resources)
		{
			if (dataSource != null)
			{
				var deviceDataSourceByDeviceIdAndDataSourceId = await LogicMonitorClient
					.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken);
				if (deviceDataSourceByDeviceIdAndDataSourceId is null)
				{
					continue;
				}

				resourceDataSources.Add(deviceDataSourceByDeviceIdAndDataSourceId);
			}
		}

		resourceDataSources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetResourceDatasourceData()
	{
		var deviceDataSources = await LogicMonitorClient
			.GetAllResourceDataSourcesAsync(WindowsDeviceId, null, CancellationToken);

		var data = await LogicMonitorClient
			.GetResourceDataSourceDataAsync(WindowsDeviceId, deviceDataSources[0].Id, CancellationToken);

		data.Should().NotBeNull();
	}
}
