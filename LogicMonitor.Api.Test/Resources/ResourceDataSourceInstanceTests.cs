namespace LogicMonitor.Api.Test.Resources;

public class ResourceDataSourceInstanceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllDeviceDataSourceInstancesAsync()
	{
		var result = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, cancellationToken: CancellationToken)
			;
		result.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllDeviceDataSourceInstancesForOneDeviceDataSourceAsync()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", cancellationToken: CancellationToken)
			;
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);

		_ = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), cancellationToken: CancellationToken)
			;
	}

	[Fact]
	public async Task OnlyMonitoredInstances()
	{
		var device = await GetSnmpResourceAsync(CancellationToken);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("snmp64_If-", cancellationToken: CancellationToken)
			;
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, cancellationToken: CancellationToken)
			;
		var resourceDataSourceInstances = await LogicMonitorClient.GetAllResourceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<ResourceDataSourceInstance>
		{
			Order = new Order<ResourceDataSourceInstance> { Direction = OrderDirection.Asc, Property = nameof(ResourceDataSourceInstance.DisplayName) },
			FilterItems =
			[
				new Eq<ResourceDataSourceInstance>(nameof(ResourceDataSourceInstance.StopMonitoring), false)
			]
		}, CancellationToken);

		resourceDataSourceInstances.Should().AllSatisfy(dsi => dsi.StopMonitoring.Should().BeFalse());
	}

	[Fact]
	public async Task AddDeviceDataSourceInstance()
	{
		var resource = await GetWindowsResourceAsync(CancellationToken);
		var resourceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(resource.Id, new Filter<ResourceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties =
				[
					nameof(ResourceDataSource.Id),
				]
		}, CancellationToken);

		var newInstance = new ResourceDataSourceInstanceCreationDto()
		{
			DisplayName = "lornaTest",
			Description = "test",
			WildValue = "26",
		};

		await LogicMonitorClient
			.AddResourceDataSourceInstanceAsync(resource.Id, resourceDataSources[9].Id, newInstance, CancellationToken);

		var datasourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(resource.Id, resourceDataSources[9].Id, new Filter<ResourceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(ResourceDataSourceInstance.Id), nameof(ResourceDataSourceInstance.DisplayName)]
			}, CancellationToken);

		var foundTest = false;
		var testInstance = new ResourceDataSourceInstance();
		foreach (var instance in datasourceInstances)
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
					CancellationToken);
		}

		foundTest.Should().BeTrue();
	}

	[Fact]
	public async Task GetDataPointConfiguration()
	{
		var resource = await GetWindowsResourceAsync(CancellationToken);
		var resourceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(resource.Id, new Filter<ResourceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties =
				[
					nameof(ResourceDataSource.Id),
				]
		}, CancellationToken);

		var resourceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(resource.Id, resourceDataSources[0].Id, new Filter<ResourceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(ResourceDataSourceInstance.Id), nameof(ResourceDataSourceInstance.DisplayName)]
			}, CancellationToken);

		var config = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(resource.Id, resourceDataSources[0].Id, resourceDataSourceInstances[0].Id, CancellationToken);

		config.Should().NotBeEmpty();
	}

	[Fact]
	public async Task UpdateDataPointConfig()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var deviceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(device.Id, new Filter<ResourceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties =
				[
					nameof(ResourceDataSource.Id),
				]
		}, CancellationToken);

		var datasourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(device.Id, deviceDataSources[0].Id, new Filter<ResourceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(ResourceDataSourceInstance.Id), nameof(ResourceDataSourceInstance.DisplayName)]
			}, CancellationToken);

		var config = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, CancellationToken);

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
			.UpdateDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, configId, updateConfig, CancellationToken);

		var refetchedConfig = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, CancellationToken);

		updateConfig.DisableAlerting = prevSetting;

		await LogicMonitorClient
			.UpdateDataPointConfigurationAsync(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, configId, updateConfig, CancellationToken);

		foreach (var dataPointConfig in refetchedConfig)
		{
			if (dataPointConfig.Id == configId)
			{
				dataPointConfig.DisableAlerting.Should().Be(!prevSetting);
			}
		}
	}

	[Fact]
	public async Task SetResourceDataSourceInstanceCustomProperty()
	{
		var resource = await GetWindowsResourceAsync(CancellationToken);
		var resourceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(resource.Id, new Filter<ResourceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties =
				[
					nameof(ResourceDataSource.Id),
				]
		}, CancellationToken);

		var datasourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(
				resource.Id,
				resourceDataSources[0].Id,
				new Filter<ResourceDataSourceInstance>()
				{
					Skip = 0,
				}, CancellationToken);

		var instance = datasourceInstances[0];

		var customPropertyName = "test";

		// Is it already set?
		var customProperty = instance.CustomProperties.FirstOrDefault(cp => cp.Name == customPropertyName);
		if (customProperty != null)
		{
			// Set it to "value1"
			customProperty.Value = "value1";
		}
		else
		{
			instance.CustomProperties.Add(new EntityProperty
			{
				Name = customPropertyName,
				Value = "value1"
			});
		}

		// Update the instance
		await LogicMonitorClient
			.UpdateResourceDataSourceInstanceAsync(
				resource.Id,
				resourceDataSources[0].Id,
				instance.Id,
				instance,
				CancellationToken)
			;

		// Re-fetch the instance
		var refetchedInstance = await LogicMonitorClient
			.GetResourceDataSourceInstanceAsync(resource.Id, resourceDataSources[0].Id, instance.Id, CancellationToken);

		// Check that the custom property was set
		customProperty = refetchedInstance.CustomProperties.FirstOrDefault(cp => cp.Name == "test");
		customProperty.Should().NotBeNull();
		customProperty.Value.Should().Be("value1");

		// Set it to something else
		customProperty.Value = "value2";

		// Update the instance
		await LogicMonitorClient
			.UpdateResourceDataSourceInstanceAsync(
				resource.Id,
				resourceDataSources[0].Id,
				instance.Id,
				refetchedInstance,
				CancellationToken)
			;

		// Re-fetch the instance
		refetchedInstance = await LogicMonitorClient
			.GetResourceDataSourceInstanceAsync(resource.Id, resourceDataSources[0].Id, instance.Id, CancellationToken);

		// Check that the custom property was set
		customProperty = refetchedInstance.CustomProperties.FirstOrDefault(cp => cp.Name == "test");
		customProperty.Should().NotBeNull();
		customProperty.Value.Should().Be("value2");
	}
}
