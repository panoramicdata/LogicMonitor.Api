using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.LogicModules;

public class DataSourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetDataSourceByName()
	{
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		dataSource.Id.Should().NotBe(0);
	}

	[Fact]
	public async Task GetDeviceGroupDataSources()
	{
		var deviceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		deviceGroup.Should().NotBeNull();

		var resourceGroupDataSources = await LogicMonitorClient
			.GetAllResourceGroupDataSourcesAsync(deviceGroup.Id, default)
			.ConfigureAwait(true);
		resourceGroupDataSources.Should().NotBeNullOrEmpty();

		var deviceGroupDataSource = await LogicMonitorClient
			.GetResourceGroupDataSourceByIdAsync(deviceGroup.Id, resourceGroupDataSources[0].DataSourceId, default)
			.ConfigureAwait(true);

		resourceGroupDataSources[0].DataSourceName.Should().Be(deviceGroupDataSource.DataSourceName);
	}

	[Fact]
	public async Task GetDeviceGroupDeviceDataSourceInstances()
	{
		var deviceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		deviceGroup.Should().NotBeNull();
		deviceGroup.Id.Should().NotBe(0);
		// We have the ResourceGroup

		// Determine the DataSources
		var pingDataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", default)
			.ConfigureAwait(true);
		pingDataSource.Should().NotBeNull();

		var dnsDataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("dns", default)
			.ConfigureAwait(true);
		dnsDataSource.Should().NotBeNull();

		var dataSourcesIds = new List<int>
		{
			pingDataSource.Id,
			dnsDataSource.Id,
		};

		var resourceDataSourceInstances = await LogicMonitorClient
			.GetInstancesAsync(
				LogicModuleType.DataSource,
				deviceGroup.Id,
				dataSourcesIds,
				null,
				null,
				new Filter<InstanceProperty>(),
				cancellationToken: default)
			.ConfigureAwait(true);

		resourceDataSourceInstances.Should().NotBeNull();
		resourceDataSourceInstances.Should().NotBeNullOrEmpty();

		var sum = 0;
		foreach (var resourceDataSourceInstance in resourceDataSourceInstances)
		{
			if (resourceDataSourceInstance.DataSourceId is not null)
			{
				var refetchedDeviceDataSourceInstanceCount = (await LogicMonitorClient
					 .GetResourceDataSourceByResourceIdAndDataSourceIdAsync(
						resourceDataSourceInstance.ResourceId,
						resourceDataSourceInstance.DataSourceId.Value,
						default)
					 .ConfigureAwait(true)).InstanceCount;
				refetchedDeviceDataSourceInstanceCount.Should().NotBe(0);
				sum += refetchedDeviceDataSourceInstanceCount;
			}
		}

		sum.Should().Be(resourceDataSourceInstances.Count);
	}

	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetWinService()
	{
		var device = await GetWindowsResourceAsync(default)
			.ConfigureAwait(true);
		var windowsServices = await LogicMonitorClient
			.GetResourceProcessesAsync(device.Id, ResourceProcessServiceTaskType.WindowsService, default)
			.ConfigureAwait(true);
		windowsServices.Should().NotBeNull();
		windowsServices.Items.Should().NotBeNull();
		windowsServices.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetMonitoredWinService()
	{
		var windowsServices = await LogicMonitorClient
			.GetMonitoredResourceProcessesAsync(29, ResourceProcessServiceTaskType.WindowsService, default)
			.ConfigureAwait(true);
		windowsServices.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetXml()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU", default)
			.ConfigureAwait(true);
		dataSource ??= new();
		var xml = await LogicMonitorClient
			.GetDataSourceXmlAsync(dataSource.Id, default)
			.ConfigureAwait(true);

		xml.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourcesPage()
	{
		var dataSourcePage = await LogicMonitorClient
			.GetPageAsync(new Filter<DataSource> { Skip = 0, Take = 10 }, default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		dataSourcePage.Items.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		dataSourcePage.Items.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Check each one
		var dataSourcesString = string.Empty;
		foreach (var dataSource in dataSourcePage.Items)
		{
			var overviewGraphs = await LogicMonitorClient
			.GetDataSourceOverviewGraphsPageAsync(dataSource.Id, new Filter<DataSourceGraph>(), default)
			.ConfigureAwait(true);

			overviewGraphs.Should().NotBeNull();

			var testGraphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(dataSource.Id, default)
				.ConfigureAwait(true);

			testGraphs.Should().NotBeNull();
		}

		Logger.LogInformation("{DataSourcesString}", dataSourcesString);
	}

	[Fact]
	public async Task GetAllDataSources()
	{
		var dataSources = await LogicMonitorClient
			.GetAllAsync<DataSource>(default)
			.ConfigureAwait(true);
		dataSources.Should().NotBeNull();
		dataSources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDataPointThresholdDetailsForDeviceDataSourceInstance()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default)
			.ConfigureAwait(true);
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new Filter<ResourceDataSourceInstance> { Skip = 0, Take = 10 }, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstance = deviceDataSourceInstances[0];
		var dataPointDetails = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance.Id, default)
			.ConfigureAwait(true);
		var dataPointConfiguration = dataPointDetails[0];
		dataPointConfiguration.Should().NotBeNull();
		dataPointConfiguration.GlobalAlertExpr.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_ValidName_Ok()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_ValidNameWithSpaces_Ok()
	{
		const string DataSourceName = "IP Addresses";
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync(DataSourceName, default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		dataSource ??= new();
		dataSource.Name.Should().Be(DataSourceName);
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_BadName_Null()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU-", default)
			.ConfigureAwait(true);
		dataSource.Should().BeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceInstances()
	{
		var portalClient = LogicMonitorClient;
		var device = await GetSnmpResourceAsync(default)
			.ConfigureAwait(true);
		device.Should().NotBeNull();

		var dataSource = await portalClient
			.GetByNameAsync<DataSource>("snmp64_If-", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var deviceDataSource = await portalClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, default)
			.ConfigureAwait(true);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await portalClient
			.GetAllResourceDataSourceInstancesAsync(
				device.Id,
				deviceDataSource.Id,
				new Filter<ResourceDataSourceInstance>
				{
					Skip = 0,
					Take = 10,
					Properties = [nameof(ResourceDataSourceInstance.Id)]
				}, default)
			.ConfigureAwait(true);
		deviceDataSourceInstances.Should().NotBeNull();
		foreach (var deviceDataSourceInstance in deviceDataSourceInstances)
		{
			deviceDataSourceInstance.Should().NotBeNull();
			var deviceDataSourceInstanceRefetch = await portalClient
				.GetResourceDataSourceInstanceAsync(
					device.Id,
					deviceDataSource.Id,
					deviceDataSourceInstance.Id,
					default)
				.ConfigureAwait(true);
			deviceDataSourceInstanceRefetch.Should().NotBeNull();
		}

		var deviceDataSourceInstanceGroups = await portalClient
			.GetResourceDataSourceInstanceGroupsAsync(
				device.Id,
				deviceDataSource.Id,
				default)
			.ConfigureAwait(true);
		deviceDataSourceInstanceGroups.Should().NotBeNull();

		var fetchedGraph = await LogicMonitorClient
			.GetResourceDataSourceInstanceGroupAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstanceGroups[0].Id, false, default)
			.ConfigureAwait(true);
		if (fetchedGraph != null)
		{
			deviceDataSourceInstanceGroups[0].Name.Should().Be(fetchedGraph.Name);
		}

		foreach (var deviceDataSourceInstanceGroup in deviceDataSourceInstanceGroups)
		{
			deviceDataSourceInstanceGroup.Should().NotBeNull();

			var deviceDataSourceInstanceGroupInstances = await portalClient
				.GetResourceDataSourceInstanceGroupInstancesPageAsync(
					device.Id,
					deviceDataSource.Id,
					deviceDataSourceInstanceGroup.Id,
					new Filter<ResourceDataSourceInstance>
					{
						Skip = 0,
						Take = 300,
						Properties =
						[
								nameof(ResourceDataSourceInstance.Id)
						]
					}, default)
				.ConfigureAwait(true);
			deviceDataSourceInstanceGroupInstances.Should().NotBeNull();
			deviceDataSourceInstanceGroupInstances.Items.Should().NotBeNull();

			foreach (var deviceDataSourceInstanceGroupInstance in deviceDataSourceInstanceGroupInstances.Items)
			{
				deviceDataSourceInstanceGroupInstance.Should().NotBeNull();
			}
		}
	}

	[Fact]
	public async Task TestResourceGroupAlertSettings()
	{
		var deviceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		var items = await LogicMonitorClient
			.GetResourceGroupDataPointConfigurationAsync(deviceGroup.Id, 3, default)
			.ConfigureAwait(true);
		items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceByName_IsFast()
	{
		var stopwatch = Stopwatch.StartNew();
		var deviceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(
			425,
			new Filter<ResourceDataSource>
			{
				Take = 1,
				Properties =
				[
					nameof(ResourceDataSource.Id),
					nameof(ResourceDataSource.DataSourceName)
				],
				FilterItems =
				[
					new FilterItem<ResourceDataSource>
					{
						Property = nameof(ResourceDataSource.DataSourceName),
						Operation = ":",
						Value = "SSL_Certificates"
					}
				]
			}, default).ConfigureAwait(true);
		var durationMs = stopwatch.ElapsedMilliseconds;

		deviceDataSources.Should().NotBeNull();
		var deviceDataSource = deviceDataSources.SingleOrDefault();
		deviceDataSource.Should().NotBeNull();
		durationMs.Should().BeLessThan(2000);
	}

	[Fact]
	public async Task GetDeviceDataSources()
	{
		var deviceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(WindowsDeviceId, new Filter<ResourceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties =
				[
					nameof(ResourceDataSource.Id),
					nameof(ResourceDataSource.CreatedOnSeconds),
				]
		}, default).ConfigureAwait(true);

		// Make sure that we have groups and they are not null
		deviceDataSources.Should().NotBeNull();

		foreach (var deviceDataSource in deviceDataSources)
		{
			// Re-fetch
			var deviceDataSourceRefetch = await LogicMonitorClient
				.GetResourceDataSourceAsync(
					WindowsDeviceId,
					deviceDataSource.Id,
					default)
				.ConfigureAwait(true);

			// Make sure they are the same
			deviceDataSourceRefetch.ResourceId.Should().Be(WindowsDeviceId);
			deviceDataSourceRefetch.CreatedOnSeconds.Should().Be(deviceDataSource.CreatedOnSeconds);

			// Get the instances
			_ = await LogicMonitorClient
				.GetAllResourceDataSourceInstancesAsync(
					WindowsDeviceId,
					deviceDataSource.Id,
					new Filter<ResourceDataSourceInstance>
					{
						Skip = 0,
						Take = 300
					}, default)
				.ConfigureAwait(true);

			// Get the groups
			var deviceDataSourceGroups = await LogicMonitorClient.GetResourceDataSourceGroupsPageAsync(
				WindowsDeviceId,
				deviceDataSource.Id,
				new Filter<ResourceDataSourceGroup>
				{
					Skip = 0,
					Take = 300,
					Properties = [nameof(ResourceDataSourceInstance.Id), nameof(ResourceDataSourceInstance.ResourceId)]
				}, default).ConfigureAwait(true);

			// Check any that come back
			foreach (var deviceDataSourceGroup in deviceDataSourceGroups.Items)
			{
				// Make sure they match
				deviceDataSourceGroup.ResourceId.Should().Be(WindowsDeviceId);
			}
		}
	}

	[Fact]
	public async Task CollectDeviceConfig()
	{
		var device = await GetWindowsResourceAsync(default)
			.ConfigureAwait(true);

		var deviceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(device.Id, new Filter<ResourceDataSource>
		{
			Skip = 0,
			Take = 1,
			Properties =
				[
					nameof(ResourceDataSource.Id),
				]
		}, default).ConfigureAwait(true);

		var datasourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(device.Id, deviceDataSources[0].Id, new Filter<ResourceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(ResourceDataSourceInstance.Id)]
			}, default)
			.ConfigureAwait(true);

		await LogicMonitorClient
			.CollectResourceConfigSourceConfig(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task GetFilteredDataSources()
	{
		const string groupName = ".Net";
		var dataSourcesPage = await LogicMonitorClient.GetAllAsync(new Filter<DataSource>
		{
			FilterItems =
				[
					new Eq<DataSource>(nameof(DataSource.Group), groupName)
				]
		}, default).ConfigureAwait(true);

		// Make sure that some are returned
		dataSourcesPage.Should().NotBeNull();
		dataSourcesPage.Should().NotBeNullOrEmpty();
		dataSourcesPage.Should().HaveCountLessThan(2000);

		// Make sure that they match the expected group
		dataSourcesPage.Should().AllSatisfy(item => item.Group.Should().Be(groupName));

		// The whole thing should take less than 60 seconds
		AssertIsFast(80);
	}

	[Fact]
	public async Task GetDataSourceGroupsQuickly()
	{
		// Get all DataSourceGroups
		var dataSources = await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource> { Properties = [nameof(DataSource.Group)] }, default)
			.ConfigureAwait(true);

		var distinctGroups = dataSources.Select(ds => ds.Group).Distinct().ToList();

		distinctGroups.Should().HaveCountGreaterThan(1);
	}

	[Fact]
	public async Task GetDataSourceCollectionMethodsQuickly()
	{
		// Get all DataSource Collection methods
		var dataSources = await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource> { Properties = [nameof(DataSource.CollectionMethod)] }, default)
			.ConfigureAwait(true);
		dataSources.Should().NotBeNull();
	}

	[Fact]
	public async Task WindowsServerDisks()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default)
			.ConfigureAwait(true);
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(1765, dataSource.Id, default)
			.ConfigureAwait(true);
		deviceDataSource.ResourceId.Should().Be(1765);
		deviceDataSource.DataSourceId.Should().Be(dataSource.Id);
	}

	[Fact]
	public async Task GetDataSourceOGraphByName()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("CiscoTemp-", default)
			.ConfigureAwait(true);

		if (dataSource != null)
		{

			var ograph = await LogicMonitorClient
				.GetDataSourceOverviewGraphByNameAsync(dataSource.Id, "Temperature", default)
				.ConfigureAwait(true);

			ograph.Should().NotBeNull();

			var ographById = await LogicMonitorClient
				.GetDataSourceOverviewGraphAsync(dataSource.Id, ograph.Id, default)
				.ConfigureAwait(true);

			ographById.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetDataSourceGraph()
	{
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", default)
			.ConfigureAwait(true);

		if (dataSource != null)
		{
			var testGraphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(dataSource.Id, default)
				.ConfigureAwait(true);

			var graph = await LogicMonitorClient
				.GetDataSourceGraphAsync(dataSource.Id, testGraphs[0].Id, default)
				.ConfigureAwait(true);

			testGraphs[0].Name.Should().Be(graph.Name);
		}
	}
}