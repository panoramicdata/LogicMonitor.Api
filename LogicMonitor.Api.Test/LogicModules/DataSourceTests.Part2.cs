using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.LogicModules;

public partial class DataSourceTests
{

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
	public async Task TestResourceGroupAlertSettings()
	{
		var deviceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();
		var items = await LogicMonitorClient
			.GetResourceGroupDataPointConfigurationAsync(deviceGroup.Id, 3, CancellationToken);
		items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceByName_IsFast()
	{
		var stopwatch = Stopwatch.StartNew();
		var deviceDataSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(
			WindowsDeviceId,
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
	[Trait("Category", "Long")]
	public async Task CollectDeviceConfig()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);

		// Config collection only exists for ConfigSources, and only on one that has an instance.
		var configSources = await LogicMonitorClient.GetAllResourceDataSourcesAsync(device.Id, new Filter<ResourceDataSource>
		{
			FilterItems = [new Eq<ResourceDataSource>(nameof(ResourceDataSource.DataSourceType), "CS")],
			Properties = [nameof(ResourceDataSource.Id), nameof(ResourceDataSource.InstanceCount)]
		}, CancellationToken);
		var configSource = configSources.FirstOrDefault(candidate => candidate.InstanceCount > 0);
		configSource.Should().NotBeNull($"resource {device.Id} needs an applied ConfigSource with at least one instance");

		var instances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(device.Id, configSource!.Id, new Filter<ResourceDataSourceInstance>
			{
				Properties = [nameof(ResourceDataSourceInstance.Id)]
			}, CancellationToken);
		instances.Should().NotBeEmpty();

		await LogicMonitorClient
			.CollectResourceConfigSourceConfig(device.Id, configSource.Id, instances[0].Id, CancellationToken);
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(1765, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
