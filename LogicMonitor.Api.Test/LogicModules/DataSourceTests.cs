using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.LogicModules;

public class DataSourceTests : TestWithOutput
{
	public DataSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetDeviceGroupDataSources()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken.None).ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();

		var deviceGroupDataSources = await LogicMonitorClient.GetAllDeviceGroupDataSourcesAsync(deviceGroup.Id, CancellationToken.None).ConfigureAwait(false);
		deviceGroupDataSources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceGroupDeviceDataSourceInstances()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken.None).ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();
		deviceGroup.Id.Should().NotBe(0);
		// We have the device group

		// Determine the DataSources
		var dataSourcesIds = new List<DataSource>
				{
					await LogicMonitorClient.GetDataSourceByUniqueNameAsync("Ping", CancellationToken.None).ConfigureAwait(false),
					await LogicMonitorClient.GetDataSourceByUniqueNameAsync("dns", CancellationToken.None).ConfigureAwait(false)
				}
			.ConvertAll(ds => ds.Id);

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetInstancesAsync(LogicModuleType.DataSource, deviceGroup.Id, dataSourcesIds, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);

		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var sum = 0;
		foreach (var deviceDataSourceInstance in deviceDataSourceInstances)
		{
			var device = await LogicMonitorClient
				.GetAsync<Device>(deviceDataSourceInstance.DeviceId.Value, CancellationToken.None)
				.ConfigureAwait(false);
			var refetchedDeviceDataSourceInstanceCount = (await LogicMonitorClient
			 .GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(deviceDataSourceInstance.DeviceId.Value, deviceDataSourceInstance.DataSourceId.Value, CancellationToken.None)
			 .ConfigureAwait(false)).InstanceCount;
			refetchedDeviceDataSourceInstanceCount.Should().NotBe(0);
			sum += refetchedDeviceDataSourceInstanceCount;
		}

		sum.Should().Be(deviceDataSourceInstances.Count);
	}

	[Fact]
	public async Task GetWinService()
	{
		var device = await GetWindowsDeviceAsync(CancellationToken.None)
			.ConfigureAwait(false);
		var windowsServices = await LogicMonitorClient
			.GetDeviceProcesses(device.Id, DeviceProcessServiceTaskType.WindowsService, CancellationToken.None)
			.ConfigureAwait(false);
		windowsServices.Should().NotBeNull();
		windowsServices.Items.Should().NotBeNull();
		windowsServices.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetMonitoredWinService()
	{
		var device = await GetWindowsDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		device.Should().NotBeNull();
		var windowsServices = await LogicMonitorClient.GetMonitoredDeviceProcesses(device.Id, DeviceProcessServiceTaskType.WindowsService, CancellationToken.None).ConfigureAwait(false);
		windowsServices.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetXml()
	{
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken.None).ConfigureAwait(false);
		var xml = await LogicMonitorClient.GetDataSourceXmlAsync(dataSource.Id, CancellationToken.None).ConfigureAwait(false);

		xml.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourcesPage()
	{
		var dataSourcePage = await LogicMonitorClient.GetPageAsync(new Filter<DataSource> { Skip = 0, Take = 10 }, CancellationToken.None).ConfigureAwait(false);

		// Make sure that some are returned
		dataSourcePage.Items.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		dataSourcePage.Items.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Check each one
		var dataSourcesString = string.Empty;
		foreach (var dataSource in dataSourcePage.Items)
		{
			// TODO
			//dataSourcesString += $"{dataSource.Name} / {dataSource.DisplayedAs}\r\n";
			//TestOverviewGraphs(dataSource);
			//TestGraphs(dataSource);
		}

		Logger.LogInformation("{DataSourcesString}", dataSourcesString);
	}

	[Fact]
	public async Task GetAllDataSources()
	{
		var dataSources = await LogicMonitorClient.GetAllAsync<DataSource>(CancellationToken.None).ConfigureAwait(false);
		dataSources.Should().NotBeNull();
		dataSources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDataPointThresholdDetailsForDeviceDataSourceInstance()
	{
		var device = await GetWindowsDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance> { Skip = 0, Take = 10 }, CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSourceInstance = deviceDataSourceInstances[0];
		var dataPointDetails = await LogicMonitorClient
			.GetDeviceDataSourceInstanceDataPointConfigurationAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id, CancellationToken.None)
			.ConfigureAwait(false);
		var dataPointConfiguration = dataPointDetails.Items[0];
		dataPointConfiguration.Should().NotBeNull();
		dataPointConfiguration.GlobalAlertExpr.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_ValidName_Ok()
	{
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken.None).ConfigureAwait(false);
		dataSource.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_ValidNameWithSpaces_Ok()
	{
		const string DataSourceName = "IP Addresses";
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync(DataSourceName, CancellationToken.None).ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		dataSource.Name.Should().Be(DataSourceName);
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_BadName_Null()
	{
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU-", CancellationToken.None).ConfigureAwait(false);
		dataSource.Should().BeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceInstances()
	{
		var portalClient = LogicMonitorClient;
		var device = await GetSnmpDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		device.Should().NotBeNull();

		var dataSource = await portalClient.GetByNameAsync<DataSource>("snmp64_If-", CancellationToken.None).ConfigureAwait(false);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await portalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken.None).ConfigureAwait(false);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await portalClient
			.GetAllDeviceDataSourceInstancesAsync(
				device.Id,
				deviceDataSource.Id,
				new Filter<DeviceDataSourceInstance>
				{
					Skip = 0,
					Take = 10,
					Properties = new List<string> { nameof(DeviceDataSourceInstance.Id) }
				}, CancellationToken.None)
			.ConfigureAwait(false);
		deviceDataSourceInstances.Should().NotBeNull();
		foreach (var deviceDataSourceInstance in deviceDataSourceInstances)
		{
			deviceDataSourceInstance.Should().NotBeNull();
			var deviceDataSourceInstanceRefetch = await portalClient
				.GetDeviceDataSourceInstanceAsync(
					device.Id,
					deviceDataSource.Id,
					deviceDataSourceInstance.Id,
					CancellationToken.None)
				.ConfigureAwait(false);
			deviceDataSourceInstanceRefetch.Should().NotBeNull();

			// Get all instance properties
			var deviceDataSourceInstanceProperties = await portalClient
				.GetDeviceDataSourceInstancePropertiesAsync(
					device.Id,
					deviceDataSource.Id,
					deviceDataSourceInstance.Id,
					new Filter<DeviceDataSourceInstanceProperty>
					{
						Skip = 0,
						Take = 300,
					},
					CancellationToken.None)
				.ConfigureAwait(false);
			deviceDataSourceInstanceProperties.Should().NotBeNull();

			// Check each
			foreach (var deviceDataSourceInstanceProperty in deviceDataSourceInstanceProperties.Items)
			{
				deviceDataSourceInstanceProperty.Name.Should().NotBeNull();
				deviceDataSourceInstanceProperty.Value.Should().NotBeNull();

				// Refetch it
				var deviceDataSourceInstancePropertyRefetch = await portalClient
					.GetDeviceDataSourceInstancePropertyAsync(
						device.Id,
						deviceDataSource.Id,
						deviceDataSourceInstance.Id,
						deviceDataSourceInstanceProperty.Name,
						CancellationToken.None)
					.ConfigureAwait(false);
				deviceDataSourceInstancePropertyRefetch.Name.Should().Be(deviceDataSourceInstanceProperty.Name);
				deviceDataSourceInstancePropertyRefetch.Value.Should().Be(deviceDataSourceInstanceProperty.Value);
			}
		}

		var deviceDataSourceInstanceGroups = await portalClient
			.GetDeviceDataSourceInstanceGroupsAsync(
				device.Id,
				deviceDataSource.Id,
				CancellationToken.None)
			.ConfigureAwait(false);
		deviceDataSourceInstanceGroups.Should().NotBeNull();

		foreach (var deviceDataSourceInstanceGroup in deviceDataSourceInstanceGroups)
		{
			deviceDataSourceInstanceGroup.Should().NotBeNull();

			var deviceDataSourceInstanceGroupInstances = await portalClient
				.GetDeviceDataSourceInstanceGroupInstancesPageAsync(
					device.Id,
					deviceDataSource.Id,
					deviceDataSourceInstanceGroup.Id,
					new Filter<DeviceDataSourceInstance>
					{
						Skip = 0,
						Take = 300,
						Properties = new List<string>
						{
								nameof(DeviceDataSourceInstance.Id)
						}
					}, CancellationToken.None)
				.ConfigureAwait(false);
			deviceDataSourceInstanceGroupInstances.Should().NotBeNull();
			deviceDataSourceInstanceGroupInstances.Items.Should().NotBeNull();

			foreach (var deviceDataSourceInstanceGroupInstance in deviceDataSourceInstanceGroupInstances.Items)
			{
				deviceDataSourceInstanceGroupInstance.Should().NotBeNull();
			}
		}
	}

	[Fact]
	public async Task TestDeviceGroupAlertSettings()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken.None).ConfigureAwait(false);
		var items = await LogicMonitorClient.GetDeviceGroupDataPointConfigurationAsync(deviceGroup.Id, 3, CancellationToken.None).ConfigureAwait(false);
		items.Should().NotBeNull();
	}

	//		private static void TestGraphs(DataSource dataSource)
	//		{
	//// Get graphs
	//			var graphs = await DefaultPortalClient.GetDataSourceGraphList(dataSource.Id);
	//			foreach (var graph in graphs)
	//			{
	//				// Make sure that all have Unique Ids
	//				((graphs.Select(c => c.Id).HasDuplicates())).Should().BeFalse();

	//				// Get DataPoints
	//				var graphLines = await DefaultPortalClient.GetDataSourceGraphLines(graph.Id);

	//				// Get datapointNames
	//				var graphDataPointNames = await DefaultPortalClient.GetDataSourceGraphDataPointNames(dataSource.Id, graph.Id);

	//				// Make sure that all have Unique Ids
	//				((graphDataPointNames.HasDuplicates())).Should().BeFalse();

	//				// Get DataPoints
	//				var graphDataPoints = await DefaultPortalClient.GetDataSourceGraphDataPoints(graph.Id);

	//				// Make sure that all have Unique Ids
	//				((graphDataPoints.Select(c => c.Id).HasDuplicates())).Should().BeFalse();

	//				// Get VirtualDataPoints
	//				var graphVirtualDataPoints = await DefaultPortalClient.GetDataSourceGraphVirtualDataPoints(graph.Id);

	//				// Make sure that all have Unique Ids
	//				((graphVirtualDataPoints.Select(c => c.Id).HasDuplicates())).Should().BeFalse();
	//			}
	//		}

	//[Fact]
	//private static void TestOverviewGraphs(DataSource dataSource)
	//{
	//	// Get overview graphs
	//	var overviewGraphs = await DefaultPortalClient.GetDataSourceOverviewGraphList(dataSource.Id);
	//	foreach (var overviewGraph in overviewGraphs)
	//	{
	//		// Make sure that all have Unique Ids
	//		((overviewGraphs.Select(c => c.Id).HasDuplicates())).Should().BeFalse();

	//		// Get datapointNames
	//		var graphDataPointNames = await DefaultPortalClient.GetDataSourceGraphDataPointNames(dataSource.Id, overviewGraph.Id);

	//		// Make sure that all have Unique Ids
	//		((graphDataPointNames.HasDuplicates())).Should().BeFalse();

	//		//// Get DataPoints
	//		//var graphDataPoints = await DefaultPortalClient.GetDataSourceGraphDataPoints(overviewGraph.Id);

	//		//// Make sure that all have Unique Ids
	//		//((graphDataPoints.Select(c => c.Id).HasDuplicates())).Should().BeFalse();
	//	}
	//}

	[Fact]
	public async Task GetDeviceDataSourceByName_IsFast()
	{
		var stopwatch = Stopwatch.StartNew();
		var device = await GetWindowsDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		var deviceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(
			device.Id,
			new Filter<DeviceDataSource>
			{
				Take = 1,
				Properties = new List<string>
				{
						nameof(DeviceDataSource.Id),
						nameof(DeviceDataSource.DataSourceName)
				},
				FilterItems = new List<FilterItem<DeviceDataSource>>
				{
						new FilterItem<DeviceDataSource> {
							Property = nameof(DeviceDataSource.DataSourceName),
							Operation = ":",
							Value = "WinCPU"
						}
				}
			}, CancellationToken.None).ConfigureAwait(false);
		var durationMs = stopwatch.ElapsedMilliseconds;

		deviceDataSources.Should().NotBeNull();
		var deviceDataSource = deviceDataSources.SingleOrDefault();
		deviceDataSource.Should().NotBeNull();
		durationMs.Should().BeLessThan(2000);
	}

	[Fact]
	public async Task GetDeviceDataSources()
	{
		var device = await GetWindowsDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		var deviceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(device.Id, new Filter<DeviceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties = new List<string>
				{
					nameof(DeviceDataSource.Id),
					nameof(DeviceDataSource.CreatedOnSeconds),
				}
		}, CancellationToken.None).ConfigureAwait(false);

		// Make sure that we have groups and they are not null
		deviceDataSources.Should().NotBeNull();

		foreach (var deviceDataSource in deviceDataSources)
		{
			// Refetch
			var deviceDataSourceRefetch = await LogicMonitorClient
				.GetDeviceDataSourceAsync(
					device.Id,
					deviceDataSource.Id,
					CancellationToken.None)
				.ConfigureAwait(false);

			// Make sure they are the same
			deviceDataSourceRefetch.DeviceId.Should().Be(device.Id);
			deviceDataSourceRefetch.CreatedOnSeconds.Should().Be(deviceDataSource.CreatedOnSeconds);

			// Get the instances
			var deviceDataSourceInstances = await LogicMonitorClient
				.GetAllDeviceDataSourceInstancesAsync(
					device.Id,
					deviceDataSource.Id,
					new Filter<DeviceDataSourceInstance>
					{
						Skip = 0,
						Take = 300,
						Properties = new List<string> { nameof(DeviceDataSourceInstance.Id) }
					}, CancellationToken.None)
				.ConfigureAwait(false);

			// Get the groups
			var deviceDataSourceGroups = await LogicMonitorClient.GetDeviceDataSourceGroupsPageAsync(
				device.Id,
				deviceDataSource.Id,
				new Filter<DeviceDataSourceGroup>
				{
					Skip = 0,
					Take = 300,
					Properties = new List<string> { nameof(DeviceDataSourceInstance.Id), nameof(DeviceDataSourceInstance.DeviceId) }
				}, CancellationToken.None).ConfigureAwait(false);

			// Check any that come back
			foreach (var deviceDataSourceGroup in deviceDataSourceGroups.Items)
			{
				// Make sure they match
				deviceDataSourceGroup.DeviceId.Should().Be(device.Id);
			}
		}
	}

	[Fact]
	public async Task GetFilteredDataSources()
	{
		const string groupName = ".Net";
		var dataSourcesPage = await LogicMonitorClient.GetAllAsync(new Filter<DataSource>
		{
			FilterItems = new List<FilterItem<DataSource>>
				{
					new Eq<DataSource>(nameof(DataSource.Group), groupName)
				}
		}, CancellationToken.None).ConfigureAwait(false);

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
		var dataSources = await LogicMonitorClient.GetAllAsync(new Filter<DataSource> { Properties = new List<string> { nameof(DataSource.Group) } }, CancellationToken.None).ConfigureAwait(false);

		var distinctGroups = dataSources.Select(ds => ds.Group).Distinct().ToList();

		distinctGroups.Should().HaveCountGreaterThan(1);
	}

	[Fact]
	public async Task GetDataSourceCollectionMethodsQuickly()
	{
		// Get all DataSource Collection methods
		var dataSources = await LogicMonitorClient.GetAllAsync(new Filter<DataSource> { Properties = new List<string> { nameof(DataSource.CollectionMethod) } }, CancellationToken.None).ConfigureAwait(false);
		dataSources.Should().NotBeNull();
	}

	[Fact]
	public async Task WindowsServerDisks()
	{
		var device = await GetWindowsDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken.None).ConfigureAwait(false);
		var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken.None).ConfigureAwait(false);
		deviceDataSource.DeviceId.Should().Be(device.Id);
		deviceDataSource.DataSourceId.Should().Be(dataSource.Id);
	}
}