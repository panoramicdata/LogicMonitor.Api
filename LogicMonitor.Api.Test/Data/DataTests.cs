using System.Globalization;

namespace LogicMonitor.Api.Test.Data;

public class DataTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetForecastGraphData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		var dataSourceGraphs = await LogicMonitorClient
			.GetDataSourceGraphsAsync(dataSource!.Id, default)
			.ConfigureAwait(true);
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource.Id, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(
				425,
				deviceDataSource.Id,
				new(),
				cancellationToken: default)
			.ConfigureAwait(true);
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
			.ConfigureAwait(true);

		forecastGraphData.TrainingGraphData.Lines.Should().HaveCount(1);
		forecastGraphData.ForecastedGraphData.Lines.Should().HaveCount(3);
	}

	[Fact]
	public async Task GetOverviewGraphData()
	{
		var device = await GetSnmpDeviceAsync(default)
			.ConfigureAwait(true);
		device.Should().NotBeNull();
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("snmp64_If-", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, default)
			.ConfigureAwait(true);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstanceGroups = await LogicMonitorClient
			.GetDeviceDataSourceInstanceGroupsAsync(device.Id, deviceDataSource.Id, default)
			.ConfigureAwait(true);
		deviceDataSourceInstanceGroups.Should().NotBeNull();
		deviceDataSourceInstanceGroups.Should().NotBeNullOrEmpty();
		var deviceDataSourceInstanceGroup = deviceDataSourceInstanceGroups.Skip(2).First();
		var deviceDataSourceInstanceGroupRefetch = await LogicMonitorClient
			.GetDeviceDataSourceInstanceGroupByNameAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstanceGroup.Name, default)
			.ConfigureAwait(true);
		deviceDataSourceInstanceGroupRefetch.Should().NotBeNull();
		deviceDataSourceInstanceGroupRefetch.Name.Should().Be(deviceDataSourceInstanceGroup.Name);

		var overviewGraph = await LogicMonitorClient
			.GetDeviceOverviewGraphByNameAsync(device.Id, deviceDataSource.Id, "Top 10 Interfaces by Total Packets", default)
			.ConfigureAwait(true);
		overviewGraph.Should().NotBeNull();

		var graphDataRequest = new DeviceDataSourceGraphDataRequest
		{
			DataSourceInstanceGroupId = deviceDataSourceInstanceGroup.Id,
			OverviewGraphId = overviewGraph.Id,
			DeviceId = device.Id,
			DeviceDataSourceId = deviceDataSource.Id,
			StartDateTime = DateTime.UtcNow.FirstDayOfLastMonth(),
			EndDateTime = DateTime.UtcNow.LastDayOfLastMonth(),
			TimePeriod = TimePeriod.Zoom,
			Width = 500
		};
		graphDataRequest.Validate();
		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(graphDataRequest, default)
			.ConfigureAwait(true);
		graphData.Should().NotBeNull();
	}

	/// <summary>
	/// Netflow Data
	/// </summary>
	[Fact]
	public async Task GetNetflowGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var netflowDevice = await GetNetflowDeviceAsync(default)
			.ConfigureAwait(true);
		_ = await LogicMonitorClient.GetGraphDataAsync(new NetflowGraphDataRequest
		{
			DeviceId = netflowDevice.Id,
			StartDateTime = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(-1),
			EndDateTime = new DateTime(utcNow.Year, utcNow.Month, 1),
			NetflowFilter = new NetflowFilters(),
			TimePeriod = TimePeriod.Zoom
		}, default).ConfigureAwait(true);
	}

	/// <summary>
	/// Netflow Data
	/// </summary>
	[Fact]
	public async Task GetNetflowGraphDataForDeviceGroup()
	{
		var utcNow = DateTime.UtcNow;

		// Get the configured Netflow Device
		var netflowDevice = await GetNetflowDeviceAsync(default).ConfigureAwait(true);

		// Create the request
		var request = new NetflowDeviceGroupGraphDataRequest
		{
			DeviceGroupId = int.Parse(netflowDevice.DeviceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
			StartDateTime = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(-1),
			EndDateTime = new DateTime(utcNow.Year, utcNow.Month, 1),
			TimePeriod = TimePeriod.Zoom
		};

		// Send the request
		var data = await LogicMonitorClient
			.GetGraphDataAsync(request, default)
			.ConfigureAwait(true);

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
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();

		var dataSourceGraph = await LogicMonitorClient
			.GetDataSourceGraphByNameAsync(dataSource!.Id, "Certificate Validity", default)
			.ConfigureAwait(true);
		dataSourceGraph.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource.Id, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(425, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>(), default)
			.ConfigureAwait(true);
		var deviceGraphDataRequest = new DeviceDataSourceInstanceGraphDataRequest
		{
			DeviceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
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
				.GetGraphDataAsync(deviceGraphDataRequest, default)
				.ConfigureAwait(true);
		}
	}

	[Fact]
	public async Task GetGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var startDateTime = utcNow.FirstDayOfLastMonth();

		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var dataSourceGraph = await LogicMonitorClient
			.GetDataSourceGraphByNameAsync(dataSource.Id, "Certificate Validity", default)
			.ConfigureAwait(true);
		dataSourceGraph.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource.Id, default)
			.ConfigureAwait(true);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(425, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>(), default)
			.ConfigureAwait(true);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var deviceGraphDataRequest = new DeviceDataSourceInstanceGraphDataRequest
		{
			DeviceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = utcNow.LastDayOfLastMonth()
		};

		//  Ensure Caching is enabled
		LogicMonitorClient.UseCache = true;

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, default)
			.ConfigureAwait(true);

		graphData.Lines.Should().NotBeNullOrEmpty();
		graphData.StartTimeUtc.Should().Be(startDateTime);
		graphData.Lines[0].ColorString.Should().NotBeNull();

		// Ensure that subsequent fetches are fast
		var stopwatch = Stopwatch.StartNew();
		graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, default)
			.ConfigureAwait(true);
		graphData.Should().NotBeNull();
		stopwatch.Stop();
		stopwatch.ElapsedMilliseconds.Should().BeLessThan(50);
	}

	[Fact]
	public async Task GetGraphDataWithoutFakeData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("Ping", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var dataSourceGraph = await LogicMonitorClient
			.GetDataSourceGraphByNameAsync(dataSource.Id, "Ping Round-Trip Time", default)
			.ConfigureAwait(true);
		dataSourceGraph.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(427, dataSource.Id, default)
			.ConfigureAwait(true);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(427, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>(), default)
			.ConfigureAwait(true);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var startDateTime = new DateTime(2023, 7, 12, 10, 0, 0, DateTimeKind.Utc);
		var endDateTime = startDateTime.AddHours(1);

		var deviceGraphDataRequest = new DeviceDataSourceInstanceGraphDataRequest
		{
			DeviceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		//  Ensure Caching is enabled
		LogicMonitorClient.UseCache = true;

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, default)
			.ConfigureAwait(true);
		graphData.TimeStamps.Should().HaveCount(62);
	}

	[Fact]
	public async Task GetWidgetGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var startDateTime = utcNow.FirstDayOfLastMonth();
		var dashboard = await GetAllWidgetsDashboardAsync(default)
			.ConfigureAwait(true);
		dashboard.Should().NotBeNull();

		var widgets = await LogicMonitorClient
			.GetWidgetsByDashboardIdAsync(dashboard.Id, default)
			.ConfigureAwait(true);
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
			.GetGraphDataAsync(widgetGraphDataRequest, default)
			.ConfigureAwait(true);
		graphData.Lines.Should().NotBeNullOrEmpty();
		graphData.StartTimeUtc.Should().Be(startDateTime);
		graphData.Lines[0].ColorString.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceDataSourceInstancesFromDev()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SSL_Certificates", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(425, dataSource.Id, default)
			.ConfigureAwait(true);
		var deviceDataSourceInstances = await LogicMonitorClient
				.GetAllDeviceDataSourceInstancesAsync(
					425,
					deviceDataSource.Id,
					new Filter<DeviceDataSourceInstance>
					{
						Take = 300,
						ExtraFilters =
						[
								new Eq<DeviceDataSourceInstance>(nameof(DeviceDataSourceInstance.StopMonitoring), false)
						],
						Order = new Order<DeviceDataSourceInstance> { Property = nameof(DeviceDataSourceInstance.Name), Direction = OrderDirection.Asc }
					}, default).ConfigureAwait(true);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDataSourceUpdateReasons()
	{
		var dataSource = await LogicMonitorClient
		.GetDataSourceByUniqueNameAsync("WinCPU", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();

		var updateReasons = await LogicMonitorClient
			.GetDataSourceUpdateReasonAsync(dataSource!.Id, new Filter<DataSourceUpdateReason>(), default)
			.ConfigureAwait(true);

		updateReasons.Should().NotBeNull();
		updateReasons.Items.Should().NotBeEmpty();
	}
}
