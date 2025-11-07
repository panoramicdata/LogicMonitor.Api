using System.Globalization;

namespace LogicMonitor.Api.Test.Data;

public class DataTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetForecastGraphData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();

		var dataSourceGraphs = await LogicMonitorClient
			.GetDataSourceGraphsAsync(dataSource.Id, CancellationToken);

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(
				WindowsDeviceId,
				deviceDataSource.Id,
				new(),
				CancellationToken)
			;

		var deviceDataSourceInstance = deviceDataSourceInstances[0];
		var dataSourceGraph = dataSourceGraphs[0];
		var virtualDataPoint = dataSourceGraph.DataPoints[0];
		var forecastGraphData = await LogicMonitorClient
			.GetForecastGraphDataAsync(
				new ForecastDataRequest
				{
					TrainingTimePeriod = TrainingTimePeriod.SixMonths,
					ForecastTimePeriod = ForecastTimePeriod.OneMonth,
					DataSourceInstanceId = deviceDataSourceInstance.Id,
					GraphId = dataSourceGraph.Id,
					DataPointLabel = virtualDataPoint.Name
				},
				default)
			;

		forecastGraphData.TrainingGraphData.Lines.Should().ContainSingle();
		forecastGraphData.ForecastedGraphData.Lines.Should().HaveCount(3);
	}

	[Fact]
	public async Task GetOverviewGraphData()
	{
		var device = await GetSnmpResourceAsync(CancellationToken);
		device.Should().NotBeNull();
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("snmp64_If-", CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstanceGroups = await LogicMonitorClient
			.GetResourceDataSourceInstanceGroupsAsync(device.Id, deviceDataSource.Id, CancellationToken);
		deviceDataSourceInstanceGroups.Should().NotBeNull();
		deviceDataSourceInstanceGroups.Should().NotBeNullOrEmpty();
		var deviceDataSourceInstanceGroup = deviceDataSourceInstanceGroups.Skip(2).First();
		var deviceDataSourceInstanceGroupRefetch = await LogicMonitorClient
			.GetResourceDataSourceInstanceGroupByNameAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstanceGroup.Name, CancellationToken);
		deviceDataSourceInstanceGroupRefetch.Should().NotBeNull();
		deviceDataSourceInstanceGroupRefetch.Name.Should().Be(deviceDataSourceInstanceGroup.Name);

		var overviewGraph = await LogicMonitorClient
			.GetResourceOverviewGraphByNameAsync(device.Id, deviceDataSource.Id, "Top 10 Interfaces by Total Packets", CancellationToken);
		overviewGraph.Should().NotBeNull();

		var graphDataRequest = new ResourceDataSourceGraphDataRequest
		{
			DataSourceInstanceGroupId = deviceDataSourceInstanceGroup.Id,
			OverviewGraphId = overviewGraph.Id,
			ResourceId = device.Id,
			ResourceDataSourceId = deviceDataSource.Id,
			StartDateTime = DateTime.UtcNow.FirstDayOfLastMonth(),
			EndDateTime = DateTime.UtcNow.LastDayOfLastMonth(),
			TimePeriod = TimePeriod.Zoom,
			Width = 500
		};
		graphDataRequest.Validate();
		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(graphDataRequest, CancellationToken);
		graphData.Should().NotBeNull();
	}

	/// <summary>
	/// Netflow Data
	/// </summary>
	[Fact]
	public async Task GetNetflowGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var netflowDevice = await GetNetflowDeviceAsync(CancellationToken);
		_ = await LogicMonitorClient.GetGraphDataAsync(new NetflowGraphDataRequest
		{
			ResourceId = netflowDevice.Id,
			StartDateTime = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(-1),
			EndDateTime = new DateTime(utcNow.Year, utcNow.Month, 1),
			NetflowFilter = new NetflowFilters(),
			TimePeriod = TimePeriod.Zoom
		}, CancellationToken);
	}

	/// <summary>
	/// Netflow Data
	/// </summary>
	[Fact]
	public async Task GetNetflowGraphDataForDeviceGroup()
	{
		var utcNow = DateTime.UtcNow;

		// Get the configured Netflow Device
		var netflowDevice = await GetNetflowDeviceAsync(CancellationToken);

		// Create the request
		var request = new NetflowResourceGroupGraphDataRequest
		{
			ResourceGroupId = int.Parse(netflowDevice.ResourceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
			StartDateTime = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(-1),
			EndDateTime = new DateTime(utcNow.Year, utcNow.Month, 1),
			TimePeriod = TimePeriod.Zoom
		};

		// Send the request
		var data = await LogicMonitorClient
			.GetGraphDataAsync(request, CancellationToken);

		// Check there is at least one line of data
		data.Lines.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetGraphData_X250()
	{
		LogicMonitorClient.UseCache = true;
		var utcNow = DateTime.UtcNow;
		var startDateTime = utcNow.FirstDayOfLastMonth();
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();

		var dataSourceGraph = await LogicMonitorClient
			.GetDataSourceGraphByNameAsync(dataSource.Id, "Capacity Detail", CancellationToken);
		dataSourceGraph.Should().NotBeNull();

		var resourceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, resourceDataSource.Id, new Filter<ResourceDataSourceInstance>(), CancellationToken);
		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = utcNow.LastDayOfLastMonth()
		};
		var stopwatch = Stopwatch.StartNew();
		for (var n = 0; n < 250; n++)
		{
			Logger.LogInformation("{N:000}: {ElapsedMS:00000}ms", n, stopwatch.ElapsedMilliseconds);
			await LogicMonitorClient
				.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);
		}
	}

	[Fact]
	public async Task GetGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var startDateTime = utcNow.FirstDayOfLastMonth();

		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var dataSourceGraph = await LogicMonitorClient
			.GetDataSourceGraphByNameAsync(dataSource.Id, "Capacity Detail", CancellationToken);
		dataSourceGraph.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new Filter<ResourceDataSourceInstance>(), CancellationToken);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = utcNow.LastDayOfLastMonth()
		};

		//  Ensure Caching is enabled
		LogicMonitorClient.UseCache = true;

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);

		graphData.Lines.Should().NotBeNullOrEmpty();
		graphData.StartTimeUtc.Should().Be(startDateTime);
		graphData.Lines[0].ColorString.Should().NotBeNull();

		// Ensure that subsequent fetches are fast
		var stopwatch = Stopwatch.StartNew();
		graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);
		graphData.Should().NotBeNull();
		stopwatch.Stop();
		stopwatch.ElapsedMilliseconds.Should().BeLessThan(50);
	}

	[Fact]
	public async Task GetGraphDataWithoutFakeData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var dataSourceGraph = await LogicMonitorClient
			.GetDataSourceGraphByNameAsync(dataSource.Id, "Ping Round-Trip Time", CancellationToken);
		dataSourceGraph.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new Filter<ResourceDataSourceInstance>(), CancellationToken);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var startDateTime = new DateTime(2023, 7, 12, 10, 0, 0, DateTimeKind.Utc);
		var endDateTime = startDateTime.AddHours(1);

		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		//  Ensure Caching is enabled
		LogicMonitorClient.UseCache = true;

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);
		graphData.TimeStamps.Should().HaveCount(62);
	}

	[Fact]
	public async Task GetWidgetGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var startDateTime = utcNow.FirstDayOfLastMonth();
		var dashboard = await GetAllWidgetsDashboardAsync(CancellationToken);
		dashboard.Should().NotBeNull();

		var widgets = await LogicMonitorClient
			.GetWidgetsByDashboardIdAsync(dashboard.Id, CancellationToken);
		widgets.Should().NotBeNull();
		widgets.Should().NotBeNullOrEmpty();

		var firstCustomGraphWidget = widgets.Find(w => w.Type == "cgraph");
		firstCustomGraphWidget.Should().NotBeNull();
		firstCustomGraphWidget ??= new();
		var widgetGraphDataRequest = new WidgetGraphDataRequest
		{
			WidgetId = firstCustomGraphWidget.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = utcNow.LastDayOfLastMonth()
		};
		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(widgetGraphDataRequest, CancellationToken);
		graphData.Lines.Should().NotBeNullOrEmpty();
		graphData.StartTimeUtc.Should().Be(startDateTime);
		graphData.Lines[0].ColorString.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceInstances()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(
				WindowsDeviceId,
				dataSource.Id,
				default)
			;
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
				.GetAllResourceDataSourceInstancesAsync(
					WindowsDeviceId,
					deviceDataSource.Id,
					new Filter<ResourceDataSourceInstance>
					{
						Take = 300,
						ExtraFilters =
						[
								new Eq<ResourceDataSourceInstance>(nameof(ResourceDataSourceInstance.StopMonitoring), false)
						],
						Order = new Order<ResourceDataSourceInstance> { Property = nameof(ResourceDataSourceInstance.Name), Direction = OrderDirection.Asc }
					}, CancellationToken);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDataSourceUpdateReasons()
	{
		var dataSource = await LogicMonitorClient
		.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken);
		dataSource.Should().NotBeNull();

		var updateReasons = await LogicMonitorClient
			.GetDataSourceUpdateReasonAsync(dataSource.Id, new Filter<DataSourceUpdateReason>(), CancellationToken);

		updateReasons.Should().NotBeNull();
		updateReasons.Items.Should().NotBeEmpty();
	}
}
