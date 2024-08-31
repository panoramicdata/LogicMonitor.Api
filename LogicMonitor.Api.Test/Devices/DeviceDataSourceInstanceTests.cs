namespace LogicMonitor.Api.Test.Devices;

public class DeviceDataSourceInstanceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAllDeviceDataSourceInstancesAsync()
	{
		_ = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, cancellationToken: default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task GetAllDeviceDataSourceInstancesForOneDeviceDataSourceAsync()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", cancellationToken: default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource!.Id, default)
			.ConfigureAwait(true);

		_ = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(425, deviceDataSource.Id, new(), cancellationToken: default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task OnlyMonitoredInstances()
	{
		var device = await GetSnmpDeviceAsync(default)
			.ConfigureAwait(true);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("snmp64_If-", cancellationToken: default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource!.Id, cancellationToken: default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>
		{
			Order = new Order<DeviceDataSourceInstance> { Direction = OrderDirection.Asc, Property = nameof(DeviceDataSourceInstance.DisplayName) },
			FilterItems =
			[
				new Eq<DeviceDataSourceInstance>(nameof(DeviceDataSourceInstance.StopMonitoring), false)
			]
		}, default).ConfigureAwait(true);

		deviceDataSourceInstances.Should().AllSatisfy(dsi => dsi.StopMonitoring.Should().BeFalse());
	}

	[Fact]
	public async Task AddDeviceDataSourceInstance()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var deviceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(device.Id, new Filter<DeviceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties =
				[
					nameof(DeviceDataSource.Id),
				]
		}, default)
			.ConfigureAwait(true);

		var newInstance = new DeviceDataSourceInstanceCreationDto()
		{
			DisplayName = "lornaTest",
			Description = "test",
			WildValue = "26",
		};

		await LogicMonitorClient
			.AddDeviceDataSourceInstanceAsync(device.Id, deviceDataSources[9].Id, newInstance, default)
			.ConfigureAwait(true);

		var datasourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSources[9].Id, new Filter<DeviceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(DeviceDataSourceInstance.Id), nameof(DeviceDataSourceInstance.DisplayName)]
			}, default)
			.ConfigureAwait(true);

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
		if (foundTest)
		{
			await LogicMonitorClient
				.DeleteAsync(new DeviceDataSourceInstance() { DeviceId = device.Id, DeviceDataSourceId = deviceDataSources[9].Id, Id = testInstance.Id }, default)
				.ConfigureAwait(true);
		}
		foundTest.Should().BeTrue();
	}

	[Fact]
	public async Task GetDataPointConfiguration()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var deviceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(device.Id, new Filter<DeviceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties =
				[
					nameof(DeviceDataSource.Id),
				]
		}, default).ConfigureAwait(true);

		var datasourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSources[0].Id, new Filter<DeviceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(DeviceDataSourceInstance.Id), nameof(DeviceDataSourceInstance.DisplayName)]
			}, default)
			.ConfigureAwait(true);

		var config = await LogicMonitorClient
			.GetDeviceDataSourceInstanceDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, default)
			.ConfigureAwait(true);

		config.Items.Should().NotBeEmpty();
	}

	[Fact]
	public async Task UpdateDataPointConfig()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var deviceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(device.Id, new Filter<DeviceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties =
				[
					nameof(DeviceDataSource.Id),
				]
		}, default).ConfigureAwait(true);

		var datasourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSources[0].Id, new Filter<DeviceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(DeviceDataSourceInstance.Id), nameof(DeviceDataSourceInstance.DisplayName)]
			}, default)
			.ConfigureAwait(true);

		var config = await LogicMonitorClient
			.GetDeviceDataSourceInstanceDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, default)
			.ConfigureAwait(true);

		var configId = config.Items[0].Id;
		var prevSetting = config.Items[0].DisableAlerting;
		var updateConfig = new DataPointConfigurationCreationDTO()
		{
			AlertExpression = config.Items[0].AlertExpression,
			AlertExpressionNote = config.Items[0].AlertExpressionNote,
			DisableAlerting = !(prevSetting),
			IsActiveDiscoveryAdvancedSettingEnabled = config.Items[0].IsActiveDiscoveryAdvancedSettingEnabled,
			WarnActiveDiscoveryAdvancedSetting = config.Items[0].WarnActiveDiscoveryAdvancedSetting,
			ErrorActiveDiscoveryAdvancedSetting = config.Items[0].ErrorActiveDiscoveryAdvancedSetting,
			CriticalActiveDiscoveryAdvancedSetting = config.Items[0].CriticalActiveDiscoveryAdvancedSetting,
			ParentInstanceGroupAlertExpression = config.Items[0].ParentInstanceGroupAlertExpression
		};

		await LogicMonitorClient
			.UpdateDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, configId, updateConfig, default)
			.ConfigureAwait(true);

		var refetchedConfig = await LogicMonitorClient
			.GetDeviceDataSourceInstanceDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, default)
			.ConfigureAwait(true);

		updateConfig.DisableAlerting = prevSetting;

		await LogicMonitorClient
			.UpdateDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, configId, updateConfig, default)
			.ConfigureAwait(true);

		foreach (var dataPointConfig in refetchedConfig.Items)
		{
			if (dataPointConfig.Id == configId)
			{
				dataPointConfig.DisableAlerting.Should().Be(!(prevSetting));
			}
		}
	}
}
