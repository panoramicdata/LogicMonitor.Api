namespace LogicMonitor.Api.Test.Resources;

public class DeviceDataSourceInstanceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAllDeviceDataSourceInstancesAsync()
	{
		var result = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, cancellationToken: default)
			.ConfigureAwait(true);
		result.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllDeviceDataSourceInstancesForOneDeviceDataSourceAsync()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", cancellationToken: default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, default)
			.ConfigureAwait(true);

		_ = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), cancellationToken: default)
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, cancellationToken: default)
			.ConfigureAwait(true);
		var resourceDataSourceInstances = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<ResourceDataSourceInstance>
		{
			Order = new Order<ResourceDataSourceInstance> { Direction = OrderDirection.Asc, Property = nameof(ResourceDataSourceInstance.DisplayName) },
			FilterItems =
			[
				new Eq<ResourceDataSourceInstance>(nameof(ResourceDataSourceInstance.StopMonitoring), false)
			]
		}, default).ConfigureAwait(true);

		resourceDataSourceInstances.Should().AllSatisfy(dsi => dsi.StopMonitoring.Should().BeFalse());
	}

	[Fact]
	public async Task AddDeviceDataSourceInstance()
	{
		var resource = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var resourceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(resource.Id, new Filter<DeviceDataSource>
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
			.AddDeviceDataSourceInstanceAsync(resource.Id, resourceDataSources[9].Id, newInstance, default)
			.ConfigureAwait(true);

		var datasourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(resource.Id, resourceDataSources[9].Id, new Filter<ResourceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(ResourceDataSourceInstance.Id), nameof(ResourceDataSourceInstance.DisplayName)]
			}, default)
			.ConfigureAwait(true);

		var foundTest = false;
		var testInstance = new ResourceDataSourceInstance();
		foreach (ResourceDataSourceInstance instance in datasourceInstances)
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
				.DeleteAsync(new ResourceDataSourceInstance()
				{
					ResourceId = resource.Id,
					ResourceDataSourceId = resourceDataSources[9].Id,
					Id = testInstance.Id
				},
					default)
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
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSources[0].Id, new Filter<ResourceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(ResourceDataSourceInstance.Id), nameof(ResourceDataSourceInstance.DisplayName)]
			}, default)
			.ConfigureAwait(true);

		var config = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, default)
			.ConfigureAwait(true);

		config.Should().NotBeEmpty();
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
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSources[0].Id, new Filter<ResourceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(ResourceDataSourceInstance.Id), nameof(ResourceDataSourceInstance.DisplayName)]
			}, default)
			.ConfigureAwait(true);

		var config = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, default)
			.ConfigureAwait(true);

		var configId = config[0].Id;
		var prevSetting = config[0].DisableAlerting;
		var updateConfig = new DataPointConfigurationCreationDTO()
		{
			AlertExpression = config[0].AlertExpression,
			AlertExpressionNote = config[0].AlertExpressionNote,
			DisableAlerting = !prevSetting,
			IsActiveDiscoveryAdvancedSettingEnabled = config[0].IsActiveDiscoveryAdvancedSettingEnabled,
			WarnActiveDiscoveryAdvancedSetting = config[0].WarnActiveDiscoveryAdvancedSetting,
			ErrorActiveDiscoveryAdvancedSetting = config[0].ErrorActiveDiscoveryAdvancedSetting,
			CriticalActiveDiscoveryAdvancedSetting = config[0].CriticalActiveDiscoveryAdvancedSetting,
			ParentInstanceGroupAlertExpression = config[0].ParentInstanceGroupAlertExpression
		};

		await LogicMonitorClient
			.UpdateDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, configId, updateConfig, default)
			.ConfigureAwait(true);

		var refetchedConfig = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, default)
			.ConfigureAwait(true);

		updateConfig.DisableAlerting = prevSetting;

		await LogicMonitorClient
			.UpdateDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, configId, updateConfig, default)
			.ConfigureAwait(true);

		foreach (var dataPointConfig in refetchedConfig)
		{
			if (dataPointConfig.Id == configId)
			{
				dataPointConfig.DisableAlerting.Should().Be(!prevSetting);
			}
		}
	}
}
