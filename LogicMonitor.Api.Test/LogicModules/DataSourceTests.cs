using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.LogicModules;

public partial class DataSourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{

	[Fact]
	[Trait("Category", "Long")]
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();
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
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();
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
					 )!.InstanceCount;
				refetchedDeviceDataSourceInstanceCount.Should().NotBe(0);
				sum += refetchedDeviceDataSourceInstanceCount;
			}
		}

		sum.Should().Be(resourceDataSourceInstances.Count);
	}

	[Fact]
	[Trait("Category", "Long")]
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
			.GetMonitoredResourceProcessesAsync(WindowsDeviceId, ResourceProcessServiceTaskType.WindowsService, CancellationToken);
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new Filter<ResourceDataSourceInstance> { Skip = 0, Take = 10 }, CancellationToken);
		var deviceDataSourceInstance = deviceDataSourceInstances[0];
		var dataPointDetails = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(WindowsDeviceId, deviceDataSource.Id, deviceDataSourceInstance.Id, CancellationToken);
		var dataPointConfiguration = dataPointDetails[0];
		dataPointConfiguration.Should().NotBeNull();
		dataPointConfiguration.GlobalAlertExpr.Should().NotBeNull();
	}
}
