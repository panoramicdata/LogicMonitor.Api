namespace LogicMonitor.Api.Test.Devices;

public class DeviceDataSourceTests : TestWithOutput
{
	public DeviceDataSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

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
		var devices = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true, default)
			.ConfigureAwait(true);
		devices.Should().NotBeNullOrEmpty();
		// We have devices

		// Find datacenter devices with WinCPU datasource
		var deviceDataSources = new List<DeviceDataSource>();
		foreach (var device in devices)
		{
			if (dataSource != null)
			{
				var deviceDataSourceByDeviceIdAndDataSourceId = await LogicMonitorClient
					.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, default)
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
