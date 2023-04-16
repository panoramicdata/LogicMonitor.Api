using LogicMonitor.Api.LogicModules;
using LogicMonitor.Api.Test.Extensions;
using System.Security.Cryptography;

namespace LogicMonitor.Api.Test.LogicModules;

public class DataSourceTests : TestWithOutput
{
	public DataSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetDataSourceByName()
	{
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", default)
			.ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		dataSource!.Id.Should().NotBe(0);
	}

	[Fact]
	public async Task GetDeviceGroupDataSources()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default).ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();

		var deviceGroupDataSources = await LogicMonitorClient.GetAllDeviceGroupDataSourcesAsync(deviceGroup.Id, default).ConfigureAwait(false);
		deviceGroupDataSources.Should().NotBeNullOrEmpty();

		var deviceGroupDataSource = await LogicMonitorClient
			.GetDeviceGroupDataSourceByIdAsync(deviceGroup.Id, deviceGroupDataSources[0].DataSourceId, default)
			.ConfigureAwait(false);

		deviceGroupDataSources[0].DataSourceName.Should().Be(deviceGroupDataSource.DataSourceName);
	}

	[Fact]
	public async Task GetDeviceGroupDeviceDataSourceInstances()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default).ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();
		deviceGroup.Id.Should().NotBe(0);
		// We have the device group

		// Determine the DataSources
		var pingDataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", default)
			.ConfigureAwait(false);
		pingDataSource.Should().NotBeNull();

		var dnsDataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("dns", default)
			.ConfigureAwait(false);
		dnsDataSource.Should().NotBeNull();

		var dataSourcesIds = new List<int>
		{
			pingDataSource!.Id,
			dnsDataSource!.Id,
		};

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetInstancesAsync(
				LogicModuleType.DataSource,
				deviceGroup.Id,
				dataSourcesIds,
				null,
				null,
				new Filter<InstanceProperty>(),
				cancellationToken: default)
			.ConfigureAwait(false);

		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var sum = 0;
		foreach (var deviceDataSourceInstance in deviceDataSourceInstances)
		{
			if (deviceDataSourceInstance.DeviceId is not null && deviceDataSourceInstance.DataSourceId is not null)
			{
				var refetchedDeviceDataSourceInstanceCount = (await LogicMonitorClient
					 .GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(deviceDataSourceInstance.DeviceId.Value, deviceDataSourceInstance.DataSourceId.Value, default)
					 .ConfigureAwait(false)).InstanceCount;
				refetchedDeviceDataSourceInstanceCount.Should().NotBe(0);
				sum += refetchedDeviceDataSourceInstanceCount;
			}
		}

		sum.Should().Be(deviceDataSourceInstances.Count);
	}

	[Fact]
	public async Task GetWinService()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(false);
		var windowsServices = await LogicMonitorClient
			.GetDeviceProcesses(device.Id, DeviceProcessServiceTaskType.WindowsService, default)
			.ConfigureAwait(false);
		windowsServices.Should().NotBeNull();
		windowsServices.Items.Should().NotBeNull();
		windowsServices.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetMonitoredWinService()
	{
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
		device.Should().NotBeNull();
		var windowsServices = await LogicMonitorClient.GetMonitoredDeviceProcesses(device.Id, DeviceProcessServiceTaskType.WindowsService, default).ConfigureAwait(false);
		windowsServices.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetXml()
	{
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU", default).ConfigureAwait(false);
		dataSource ??= new();
		var xml = await LogicMonitorClient.GetDataSourceXmlAsync(dataSource.Id, default).ConfigureAwait(false);

		xml.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourcesPage()
	{
		var dataSourcePage = await LogicMonitorClient.GetPageAsync(new Filter<DataSource> { Skip = 0, Take = 10 }, default).ConfigureAwait(false);

		// Make sure that some are returned
		dataSourcePage.Items.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		dataSourcePage.Items.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Check each one
		var dataSourcesString = string.Empty;
		foreach (var dataSource in dataSourcePage.Items)
		{
			// TODO
			var overviewGraphs = await LogicMonitorClient
			.GetDataSourceOverviewGraphsPageAsync(dataSource.Id, new Filter<DataSourceOverviewGraph>(), default)
			.ConfigureAwait(false);

			overviewGraphs.Should().NotBeNull();

			var testGraphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(dataSource.Id, default)
				.ConfigureAwait(false);

			testGraphs.Should().NotBeNull();
		}

		Logger.LogInformation("{DataSourcesString}", dataSourcesString);
	}

	[Fact]
	public async Task GetAllDataSources()
	{
		var dataSources = await LogicMonitorClient.GetAllAsync<DataSource>(default).ConfigureAwait(false);
		dataSources.Should().NotBeNull();
		dataSources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDataPointThresholdDetailsForDeviceDataSourceInstance()
	{
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU", default)
			.ConfigureAwait(false);
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, default)
			.ConfigureAwait(false);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance> { Skip = 0, Take = 10 }, default)
			.ConfigureAwait(false);
		var deviceDataSourceInstance = deviceDataSourceInstances[0];
		var dataPointDetails = await LogicMonitorClient
			.GetDeviceDataSourceInstanceDataPointConfigurationAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id, default)
			.ConfigureAwait(false);
		var dataPointConfiguration = dataPointDetails.Items[0];
		dataPointConfiguration.Should().NotBeNull();
		dataPointConfiguration.GlobalAlertExpr.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_ValidName_Ok()
	{
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU", default).ConfigureAwait(false);
		dataSource.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_ValidNameWithSpaces_Ok()
	{
		const string DataSourceName = "IP Addresses";
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync(DataSourceName, default).ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		dataSource ??= new();
		dataSource.Name.Should().Be(DataSourceName);
	}

	[Fact]
	public async Task GetDataSourceByUniqueName_BadName_Null()
	{
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU-", default).ConfigureAwait(false);
		dataSource.Should().BeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceInstances()
	{
		var portalClient = LogicMonitorClient;
		var device = await GetSnmpDeviceAsync(default).ConfigureAwait(false);
		device.Should().NotBeNull();

		var dataSource = await portalClient.GetByNameAsync<DataSource>("snmp64_If-", default).ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var deviceDataSource = await portalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, default).ConfigureAwait(false);
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
				}, default)
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
					default)
				.ConfigureAwait(false);
			deviceDataSourceInstanceRefetch.Should().NotBeNull();
		}

		var deviceDataSourceInstanceGroups = await portalClient
			.GetDeviceDataSourceInstanceGroupsAsync(
				device.Id,
				deviceDataSource.Id,
				default)
			.ConfigureAwait(false);
		deviceDataSourceInstanceGroups.Should().NotBeNull();

		var fetchedGraph = await LogicMonitorClient
			.GetDeviceDataSourceInstanceGroupAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstanceGroups[0].Id, false, default)
			.ConfigureAwait(false);
		if (fetchedGraph != null)
		{
			deviceDataSourceInstanceGroups[0].Name.Should().Be(fetchedGraph.Name);
		}

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
					}, default)
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
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default).ConfigureAwait(false);
		var items = await LogicMonitorClient.GetDeviceGroupDataPointConfigurationAsync(deviceGroup.Id, 3, default).ConfigureAwait(false);
		items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceByName_IsFast()
	{
		var stopwatch = Stopwatch.StartNew();
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
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
			}, default).ConfigureAwait(false);
		var durationMs = stopwatch.ElapsedMilliseconds;

		deviceDataSources.Should().NotBeNull();
		var deviceDataSource = deviceDataSources.SingleOrDefault();
		deviceDataSource.Should().NotBeNull();
		durationMs.Should().BeLessThan(2000);
	}

	[Fact]
	public async Task GetDeviceDataSources()
	{
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
		var deviceDataSources = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(device.Id, new Filter<DeviceDataSource>
		{
			Skip = 0,
			Take = 10,
			Properties = new List<string>
				{
					nameof(DeviceDataSource.Id),
					nameof(DeviceDataSource.CreatedOnSeconds),
				}
		}, default).ConfigureAwait(false);

		// Make sure that we have groups and they are not null
		deviceDataSources.Should().NotBeNull();

		foreach (var deviceDataSource in deviceDataSources)
		{
			// Refetch
			var deviceDataSourceRefetch = await LogicMonitorClient
				.GetDeviceDataSourceAsync(
					device.Id,
					deviceDataSource.Id,
					default)
				.ConfigureAwait(false);

			// Make sure they are the same
			deviceDataSourceRefetch.DeviceId.Should().Be(device.Id);
			deviceDataSourceRefetch.CreatedOnSeconds.Should().Be(deviceDataSource.CreatedOnSeconds);

			// Get the instances
			_ = await LogicMonitorClient
				.GetAllDeviceDataSourceInstancesAsync(
					device.Id,
					deviceDataSource.Id,
					new Filter<DeviceDataSourceInstance>
					{
						Skip = 0,
						Take = 300,
						Properties = new List<string> { nameof(DeviceDataSourceInstance.Id) }
					}, default)
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
				}, default).ConfigureAwait(false);

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
		}, default).ConfigureAwait(false);

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
		var dataSources = await LogicMonitorClient.GetAllAsync(new Filter<DataSource> { Properties = new List<string> { nameof(DataSource.Group) } }, default).ConfigureAwait(false);

		var distinctGroups = dataSources.Select(ds => ds.Group).Distinct().ToList();

		distinctGroups.Should().HaveCountGreaterThan(1);
	}

	[Fact]
	public async Task GetDataSourceCollectionMethodsQuickly()
	{
		// Get all DataSource Collection methods
		var dataSources = await LogicMonitorClient.GetAllAsync(new Filter<DataSource> { Properties = new List<string> { nameof(DataSource.CollectionMethod) } }, default).ConfigureAwait(false);
		dataSources.Should().NotBeNull();
	}

	[Fact]
	public async Task WindowsServerDisks()
	{
		var device = await GetWindowsDeviceAsync(default).ConfigureAwait(false);
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default).ConfigureAwait(false);
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, default).ConfigureAwait(false);
		deviceDataSource.DeviceId.Should().Be(device.Id);
		deviceDataSource.DataSourceId.Should().Be(dataSource.Id);
	}

	[Fact]
	public async Task GetDataSourceOGraphByName()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("CiscoTemp-", default)
			.ConfigureAwait(false);

		if (dataSource != null)
		{

			var ograph = await LogicMonitorClient
				.GetDataSourceOverviewGraphByNameAsync(dataSource.Id, "Temperature", default)
				.ConfigureAwait(false);

			ograph.Should().NotBeNull();

			var ographById = await LogicMonitorClient
				.GetDataSourceOverviewGraphAsync(dataSource.Id, ograph.Id, default)
				.ConfigureAwait(false);

			ographById.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetDataSourceGraph()
	{
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", default)
			.ConfigureAwait(false);

		if (dataSource != null )
		{
			var testGraphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(dataSource.Id, default)
				.ConfigureAwait(false);

			var graph = await LogicMonitorClient
				.GetDataSourceGraphAsync(dataSource.Id, testGraphs[0].Id, default)
				.ConfigureAwait(false);

			testGraphs[0].Name.Should().Be(graph.Name);
		}
	}
}