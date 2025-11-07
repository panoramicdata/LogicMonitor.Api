using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.LogicModules;

public class DataSourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetDataSourceByName()
	{
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource.Id.Should().NotBe(0);
	}

	[Fact]
	public async Task GetDeviceGroupDataSources()
	{
		var deviceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);
		deviceGroup.Should().NotBeNull();

		var resourceGroupDataSources = await LogicMonitorClient
			.GetAllResourceGroupDataSourcesAsync(deviceGroup.Id, CancellationToken);
		resourceGroupDataSources.Should().NotBeNullOrEmpty();

		var deviceGroupDataSource = await LogicMonitorClient
			.GetResourceGroupDataSourceByIdAsync(deviceGroup.Id, resourceGroupDataSources[0].DataSourceId, CancellationToken);

		resourceGroupDataSources[0].DataSourceName.Should().Be(deviceGroupDataSource.DataSourceName);
	}

	[Fact]
	public async Task GetDeviceGroupDeviceDataSourceInstances()
	{
		var deviceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);
		deviceGroup.Should().NotBeNull();
		deviceGroup.Id.Should().NotBe(0);
		// We have the ResourceGroup

		// Determine the DataSources
		var pingDataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", CancellationToken);
		pingDataSource.Should().NotBeNull();

		var dnsDataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("dns", CancellationToken);
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
				CancellationToken);

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
						CancellationToken)
					 ).InstanceCount;
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
		var device = await GetWindowsResourceAsync(CancellationToken);
		var windowsServices = await LogicMonitorClient
			.GetResourceProcessesAsync(device.Id, ResourceProcessServiceTaskType.WindowsService, CancellationToken);
		windowsServices.Should().NotBeNull();
		windowsServices.Items.Should().NotBeNull();
		windowsServices.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetMonitoredWinService()
	{
		var windowsServices = await LogicMonitorClient
			.GetMonitoredResourceProcessesAsync(29, ResourceProcessServiceTaskType.WindowsService, CancellationToken);
		windowsServices.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetXml()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken);
		dataSource ??= new();
		var xml = await LogicMonitorClient
			.GetDataSourceXmlAsync(dataSource.Id, CancellationToken);

		xml.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourcesPage()
	{
		var dataSourcePage = await LogicMonitorClient
			.GetPageAsync(new Filter<DataSource> { Skip = 0, Take = 10 }, CancellationToken);

		// Make sure that some are returned
		dataSourcePage.Items.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		dataSourcePage.Items.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Check each one
		var dataSourcesString = string.Empty;
		foreach (var dataSource in dataSourcePage.Items)
		{
			var overviewGraphs = await LogicMonitorClient
			.GetDataSourceOverviewGraphsPageAsync(dataSource.Id, new Filter<DataSourceGraph>(), CancellationToken);

			overviewGraphs.Should().NotBeNull();

			var testGraphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(dataSource.Id, CancellationToken);

			testGraphs.Should().NotBeNull();
		}

		Logger.LogInformation("{DataSourcesString}", dataSourcesString);
	}

	[Fact]
	public async Task GetAllDataSources()
	{
		var dataSources = await LogicMonitorClient
			.GetAllAsync<DataSource>(CancellationToken);
		dataSources.Should().NotBeNull();
		dataSources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDataPointThresholdDetailsForDeviceDataSourceInstance()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new Filter<ResourceDataSourceInstance> { Skip = 0, Take = 10 }, CancellationToken);
		var deviceDataSourceInstance = deviceDataSourceInstances[0];
		var dataPointDetails = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance.Id, CancellationToken);
		var dataPointConfiguration = dataPointDetails[0];
		dataPointConfiguration.Should().NotBeNull();
		dataPointConfiguration.GlobalAlertExpr.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_ValidName_Ok()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken);
		dataSource.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_ValidNameWithSpaces_Ok()
	{
		const string DataSourceName = "IP Addresses";
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync(DataSourceName, CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource ??= new();
		dataSource.Name.Should().Be(DataSourceName);
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_BadName_Null()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU-", CancellationToken);
		dataSource.Should().BeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceInstances()
	{
		var portalClient = LogicMonitorClient;
		var device = await GetSnmpResourceAsync(CancellationToken);
		device.Should().NotBeNull();

		var dataSource = await portalClient
			.GetByNameAsync<DataSource>("snmp64_If-", CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var deviceDataSource = await portalClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken);
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
				}, CancellationToken);
		deviceDataSourceInstances.Should().NotBeNull();
		foreach (var deviceDataSourceInstance in deviceDataSourceInstances)
		{
			deviceDataSourceInstance.Should().NotBeNull();
			var deviceDataSourceInstanceRefetch = await portalClient
				.GetResourceDataSourceInstanceAsync(
					device.Id,
					deviceDataSource.Id,
					deviceDataSourceInstance.Id,
					CancellationToken);
			deviceDataSourceInstanceRefetch.Should().NotBeNull();
		}

		var deviceDataSourceInstanceGroups = await portalClient
			.GetResourceDataSourceInstanceGroupsAsync(
				device.Id,
				deviceDataSource.Id,
				CancellationToken);
		deviceDataSourceInstanceGroups.Should().NotBeNull();

		var fetchedGraph = await LogicMonitorClient
			.GetResourceDataSourceInstanceGroupAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstanceGroups[0].Id, false, CancellationToken);
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
					}, CancellationToken);
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
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);
		var items = await LogicMonitorClient
			.GetResourceGroupDataPointConfigurationAsync(deviceGroup.Id, 3, CancellationToken);
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
			}, CancellationToken);
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
		}, CancellationToken);

		// Make sure that we have groups and they are not null
		deviceDataSources.Should().NotBeNull();

		foreach (var deviceDataSource in deviceDataSources)
		{
			// Re-fetch
			var deviceDataSourceRefetch = await LogicMonitorClient
				.GetResourceDataSourceAsync(
					WindowsDeviceId,
					deviceDataSource.Id,
					CancellationToken);

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
					}, CancellationToken);

			// Get the groups
			var deviceDataSourceGroups = await LogicMonitorClient.GetResourceDataSourceGroupsPageAsync(
				WindowsDeviceId,
				deviceDataSource.Id,
				new Filter<ResourceDataSourceGroup>
				{
					Skip = 0,
					Take = 300,
					Properties = [nameof(ResourceDataSourceInstance.Id), nameof(ResourceDataSourceInstance.ResourceId)]
				}, CancellationToken);

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
		var device = await GetWindowsResourceAsync(CancellationToken);

		var deviceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(device.Id, new Filter<ResourceDataSource>
		{
			Skip = 0,
			Take = 1,
			Properties =
				[
					nameof(ResourceDataSource.Id),
				]
		}, CancellationToken);

		var datasourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(device.Id, deviceDataSources[0].Id, new Filter<ResourceDataSourceInstance>()
			{
				Skip = 0,
				Properties = [nameof(ResourceDataSourceInstance.Id)]
			}, CancellationToken);

		await LogicMonitorClient
			.CollectResourceConfigSourceConfig(device.Id, deviceDataSources[0].Id, datasourceInstances[0].Id, CancellationToken);
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
		}, CancellationToken);

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
			.GetAllAsync(new Filter<DataSource> { Properties = [nameof(DataSource.Group)] }, CancellationToken);

		var distinctGroups = dataSources.Select(ds => ds.Group).Distinct().ToList();

		distinctGroups.Should().HaveCountGreaterThan(1);
	}

	[Fact]
	public async Task GetDataSourceCollectionMethodsQuickly()
	{
		// Get all DataSource Collection methods
		var dataSources = await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource> { Properties = [nameof(DataSource.CollectionMethod)] }, CancellationToken);
		dataSources.Should().NotBeNull();
	}

	[Fact]
	public async Task WindowsServerDisks()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(1765, dataSource.Id, CancellationToken);
		deviceDataSource.ResourceId.Should().Be(1765);
		deviceDataSource.DataSourceId.Should().Be(dataSource.Id);
	}

	[Fact]
	public async Task GetDataSourceOGraphByName()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("CiscoTemp-", CancellationToken);

		if (dataSource != null)
		{

			var ograph = await LogicMonitorClient
				.GetDataSourceOverviewGraphByNameAsync(dataSource.Id, "Temperature", CancellationToken);

			ograph.Should().NotBeNull();

			var ographById = await LogicMonitorClient
				.GetDataSourceOverviewGraphAsync(dataSource.Id, ograph.Id, CancellationToken);

			ographById.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetDataSourceGraph()
	{
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", CancellationToken);

		if (dataSource != null)
		{
			var testGraphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(dataSource.Id, CancellationToken);

			var graph = await LogicMonitorClient
				.GetDataSourceGraphAsync(dataSource.Id, testGraphs[0].Id, CancellationToken);

			testGraphs[0].Name.Should().Be(graph.Name);
		}
	}
}