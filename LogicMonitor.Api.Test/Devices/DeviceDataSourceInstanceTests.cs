namespace LogicMonitor.Api.Test.Devices;

public class DeviceDataSourceInstanceTests : TestWithOutput
{
	public DeviceDataSourceInstanceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllDeviceDataSourceInstancesAsync()
	{
		var _ = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, cancellationToken: CancellationToken.None).ConfigureAwait(false);
	}

	[Fact]
	public async Task GetAllDeviceDataSourceInstancesForOneDeviceDataSourceAsync()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinIf-", cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken.None)
			.ConfigureAwait(false);

		var _ = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task OnlyMonitoredInstances()
	{
		var device = await GetSnmpDeviceAsync()
			.ConfigureAwait(false);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("snmp64_If-", cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSourceInstances = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>
		{
			Order = new Order<DeviceDataSourceInstance> { Direction = OrderDirection.Asc, Property = nameof(DeviceDataSourceInstance.DisplayName) },
			FilterItems = new List<FilterItem<DeviceDataSourceInstance>>
				{
					new Eq<DeviceDataSourceInstance>(nameof(DeviceDataSourceInstance.StopMonitoring), false)
				}
		}, CancellationToken.None).ConfigureAwait(false);

		deviceDataSourceInstances.Should().AllSatisfy(dsi => dsi.StopMonitoring.Should().BeFalse());
	}
}
