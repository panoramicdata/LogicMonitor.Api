namespace LogicMonitor.Api.Test.Devices;

public class DeviceDataSourceInstanceTests : TestWithOutput
{
	public DeviceDataSourceInstanceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllDeviceDataSourceInstancesAsync()
	{
		_ = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, cancellationToken: default).ConfigureAwait(false);
	}

	[Fact]
	public async Task GetAllDeviceDataSourceInstancesForOneDeviceDataSourceAsync()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinIf-", cancellationToken: default)
			.ConfigureAwait(false);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource!.Id, default)
			.ConfigureAwait(false);

		_ = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task OnlyMonitoredInstances()
	{
		var device = await GetSnmpDeviceAsync(default)
			.ConfigureAwait(false);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("snmp64_If-", cancellationToken: default)
			.ConfigureAwait(false);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource!.Id, cancellationToken: default)
			.ConfigureAwait(false);
		var deviceDataSourceInstances = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>
		{
			Order = new Order<DeviceDataSourceInstance> { Direction = OrderDirection.Asc, Property = nameof(DeviceDataSourceInstance.DisplayName) },
			FilterItems = new List<FilterItem<DeviceDataSourceInstance>>
				{
					new Eq<DeviceDataSourceInstance>(nameof(DeviceDataSourceInstance.StopMonitoring), false)
				}
		}, default).ConfigureAwait(false);

		deviceDataSourceInstances.Should().AllSatisfy(dsi => dsi.StopMonitoring.Should().BeFalse());
	}

	[Fact]
	public async Task AddDeviceDataSourceInstance()
	{
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
		var deviceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(device.Id, new Filter<DeviceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties = new List<string>
				{
					nameof(DeviceDataSource.Id),
				}
		}, default).ConfigureAwait(false);

		var newInstance = new DeviceDataSourceInstanceCreationDto()
		{
			DisplayName = "lornaTest",
			Description = "test",
			WildValue = "26",
		};

		await LogicMonitorClient
			.AddDeviceDataSourceInstanceAsync(device.Id, deviceDataSources[9].Id, newInstance, default)
			.ConfigureAwait(false);

		var datasourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSources[9].Id, new Filter<DeviceDataSourceInstance>()
			{
				Skip = 0,
				Properties = new List<string> { nameof(DeviceDataSourceInstance.Id), nameof(DeviceDataSourceInstance.DisplayName) }
			}, default)
			.ConfigureAwait(false);

		var foundTest = false;
		var testInstance = new DeviceDataSourceInstance();
		foreach (DeviceDataSourceInstance instance in datasourceInstances)
		{
			if (instance.DisplayName.Equals("lornaTest", StringComparison.Ordinal))
			{
				foundTest = true;
				testInstance = instance;
			}
		}
		foundTest.Should().BeTrue();
		if (foundTest)
		{
			await LogicMonitorClient
				.DeleteAsync(new DeviceDataSourceInstance() { DeviceId = device.Id, DeviceDataSourceId = deviceDataSources[9].Id, Id = testInstance.Id }, default)
				.ConfigureAwait(false);
		}

	}

	[Fact]
	public async Task GetDataPointConfiguration()
	{
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
		var deviceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(device.Id, new Filter<DeviceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties = new List<string>
				{
					nameof(DeviceDataSource.Id),
				}
		}, default).ConfigureAwait(false);

		var datasourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSources[0].Id, new Filter<DeviceDataSourceInstance>()
			{
				Skip = 0,
				Properties = new List<string> { nameof(DeviceDataSourceInstance.Id), nameof(DeviceDataSourceInstance.DisplayName) }
			}, default)
			.ConfigureAwait(false);

		var config = await LogicMonitorClient
			.GetDeviceDataSourceInstanceDataPointConfigurations(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, default)
			.ConfigureAwait(false);

		config.Should().NotBeEmpty();
	}
}
