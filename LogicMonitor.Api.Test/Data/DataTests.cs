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
				CancellationToken);

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
				CancellationToken);

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
	public async Task GetGraphData_OneMonth_ShouldHaveHourlyResolution()
	{
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

		var startDateTime = DateTime.UtcNow.AddMonths(-1);
		var endDateTime = DateTime.UtcNow;

		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);

		graphData.Should().NotBeNull();
		graphData.TimeStamps.Should().NotBeEmpty();
		
		// For a 1-month period, we expect hourly resolution (3600 seconds)
		// LogicMonitor should aggregate data to hourly intervals
		graphData.Step.Should().Be(3600, "for a 1-month time range, data should be aggregated to hourly intervals");
		
		// Calculate expected number of data points
		var durationInSeconds = (endDateTime - startDateTime).TotalSeconds;
		var expectedDataPoints = (int)Math.Ceiling(durationInSeconds / 3600);
		
		// Allow some tolerance for edge cases (±5%)
		var tolerance = (int)(expectedDataPoints * 0.05);
		graphData.TimeStamps.Should().HaveCountGreaterThanOrEqualTo(expectedDataPoints - tolerance);
		graphData.TimeStamps.Should().HaveCountLessThanOrEqualTo(expectedDataPoints + tolerance);
		
		// Log actual values for debugging
		Logger.LogInformation("1-Month Period - Start: {Start}, End: {End}", startDateTime, endDateTime);
		Logger.LogInformation("1-Month Period - Step: {Step}s, Expected: 3600s", graphData.Step);
		Logger.LogInformation("1-Month Period - TimeStamp Count: {Count}, Expected: ~{Expected}", graphData.TimeStamps.Count, expectedDataPoints);
	}

	/// <summary>
	/// Tests LogicMonitor's actual aggregation behaviour for various time periods.
	/// NOTE: Short time periods (1 hour to 2 days) return only 2 data points (start/end) when using
	/// the TimePeriod enum, which is LogicMonitor's intended behaviour. For finer granularity,
	/// use TimePeriod.Zoom with specific start/end dates instead.
	/// 
	/// Actual behaviour discovered via diagnostic testing:
	/// - OneHour to TwoDays: Returns 2 data points spanning entire period (not useful for trending)
	/// - SevenDays: Returns ~11 minute intervals (660s step)
	/// - OneMonth: Returns 1 hour intervals (3600s step)
	/// - ThreeMonths: Returns ~2.2 hour intervals (7980s step)
	/// </summary>
	[Theory]
	[InlineData(TimePeriod.SevenDays, 660, "7 days returns ~11 minute resolution")]
	[InlineData(TimePeriod.OneMonth, 3600, "1 month returns hourly resolution")]
	[InlineData(TimePeriod.ThreeMonths, 7980, "3 months returns ~2.2 hour resolution")]
	public async Task GetGraphData_VariousTimePeriods_ShouldHaveExpectedResolution(
		TimePeriod timePeriod,
		int expectedStepSeconds,
		string reason)
	{
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

		// Calculate appropriate date range based on TimePeriod
		var endDateTime = DateTime.UtcNow;
		var startDateTime = timePeriod switch
		{
			TimePeriod.OneHour => endDateTime.AddHours(-1),
			TimePeriod.TwoHours => endDateTime.AddHours(-2),
			TimePeriod.FiveHours => endDateTime.AddHours(-5),
			TimePeriod.OneDay => endDateTime.AddDays(-1),
			TimePeriod.TwoDays => endDateTime.AddDays(-2),
			TimePeriod.SevenDays => endDateTime.AddDays(-7),
			TimePeriod.OneMonth => endDateTime.AddMonths(-1),
			TimePeriod.ThreeMonths => endDateTime.AddMonths(-3),
			_ => endDateTime.AddHours(-1)
		};

		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = timePeriod,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);

		graphData.Should().NotBeNull();
		graphData.TimeStamps.Should().NotBeEmpty();
		
		// Verify the step (data interval) matches expected resolution (allow ±5% tolerance for minor variations)
		var stepTolerance = (int)(expectedStepSeconds * 0.05);
		graphData.Step.Should().BeInRange(expectedStepSeconds - stepTolerance, expectedStepSeconds + stepTolerance, reason);
		
		// Calculate expected number of data points
		var durationInSeconds = (endDateTime - startDateTime).TotalSeconds;
		var expectedDataPoints = (int)Math.Ceiling(durationInSeconds / expectedStepSeconds);
		
		// Allow tolerance for edge cases (±10% to account for LogicMonitor's aggregation logic)
		var tolerance = Math.Max(1, (int)(expectedDataPoints * 0.10));
		graphData.TimeStamps.Should().HaveCountGreaterThanOrEqualTo(expectedDataPoints - tolerance,
			$"should have at least {expectedDataPoints - tolerance} data points");
		graphData.TimeStamps.Should().HaveCountLessThanOrEqualTo(expectedDataPoints + tolerance,
			$"should have at most {expectedDataPoints + tolerance} data points");
		
		// Log actual values for debugging/documentation
		Logger.LogInformation("{TimePeriod} - Start: {Start}, End: {End}", timePeriod, startDateTime, endDateTime);
		Logger.LogInformation("{TimePeriod} - Actual Step: {Step}s, Expected: {Expected}s", timePeriod, graphData.Step, expectedStepSeconds);
		Logger.LogInformation("{TimePeriod} - TimeStamp Count: {Count}, Expected: ~{Expected}", timePeriod, graphData.TimeStamps.Count, expectedDataPoints);
	}

	[Fact]
	public async Task GetGraphData_OneMonth_Zoom_ShouldVerifyActualIntervals()
	{
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

		var startDateTime = DateTime.UtcNow.AddMonths(-1);
		var endDateTime = DateTime.UtcNow;

		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);

		graphData.Should().NotBeNull();
		graphData.TimeStamps.Should().NotBeEmpty();
		
		// Verify we have at least 2 timestamps to calculate intervals
		graphData.TimeStamps.Should().HaveCountGreaterThan(1);
		
		// Calculate actual intervals between consecutive timestamps
		var intervals = new List<long>();
		for (int i = 1; i < Math.Min(graphData.TimeStamps.Count, 100); i++)
		{
			var intervalMs = graphData.TimeStamps[i] - graphData.TimeStamps[i - 1];
			intervals.Add(intervalMs);
		}
		
		// Convert to seconds
		var intervalsInSeconds = intervals.Select(ms => ms / 1000).ToList();
		
		// Most intervals should be equal to the Step value
		var expectedIntervalSeconds = graphData.Step;
		var matchingIntervals = intervalsInSeconds.Count(interval => Math.Abs(interval - expectedIntervalSeconds) <= 1);
		var matchPercentage = (double)matchingIntervals / intervalsInSeconds.Count * 100;
		
		// At least 90% of intervals should match the expected step
		matchPercentage.Should().BeGreaterThanOrEqualTo(90,
			$"most intervals should match the Step value of {expectedIntervalSeconds}s");
		
		// Log detailed information
		Logger.LogInformation("1-Month Zoom - Total TimeStamps: {Count}", graphData.TimeStamps.Count);
		Logger.LogInformation("1-Month Zoom - Step: {Step}s", graphData.Step);
		Logger.LogInformation("1-Month Zoom - Average Interval: {Avg}s", intervalsInSeconds.Average());
		Logger.LogInformation("1-Month Zoom - Min Interval: {Min}s, Max Interval: {Max}s",
			intervalsInSeconds.Min(), intervalsInSeconds.Max());
		Logger.LogInformation("1-Month Zoom - Intervals matching Step: {Match}%", matchPercentage);
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
				CancellationToken);
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

	/// <summary>
	/// Tests TimePeriod.Zoom boundary conditions to find exact transition points where LogicMonitor
	/// changes aggregation levels. This helps diagnose issues where data resolution changed from
	/// hourly (3600s) to coarser intervals.
	/// </summary>
	[Theory]
	[InlineData(33, "33 days - testing 30-45 boundary")]
	[InlineData(35, "35 days - testing 30-45 boundary")]
	[InlineData(38, "38 days - testing 30-45 boundary")]
	[InlineData(40, "40 days - testing 30-45 boundary")]
	[InlineData(42, "42 days - testing 30-45 boundary")]
	[InlineData(43, "43 days - testing 30-45 boundary")]
	[InlineData(44, "44 days - testing 30-45 boundary")]
	[InlineData(46, "46 days - testing 45-60 boundary")]
	[InlineData(50, "50 days - testing 45-60 boundary")]
	[InlineData(55, "55 days - testing 45-60 boundary")]
	[InlineData(75, "75 days - testing 60-90 boundary")]
	[InlineData(80, "80 days - testing 60-90 boundary")]
	[InlineData(85, "85 days - testing 60-90 boundary")]
	[InlineData(100, "100 days - testing 90-120 boundary")]
	[InlineData(110, "110 days - testing 90-120 boundary")]
	[InlineData(150, "150 days (5 months) - beyond 120")]
	[InlineData(180, "180 days (6 months) - beyond 120")]
	public async Task GetGraphData_ZoomBoundaryDiscovery_FindExactTransitionPoints(
		int daysBack,
		string description)
	{
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

		var endDateTime = DateTime.UtcNow;
		var startDateTime = endDateTime.AddDays(-daysBack);

		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);

		// Calculate metrics
		var durationInSeconds = (endDateTime - startDateTime).TotalSeconds;
		var actualDataPoints = graphData.TimeStamps.Count;
		var averageInterval = actualDataPoints > 1 
			? durationInSeconds / (actualDataPoints - 1) 
			: graphData.Step;

		// Log detailed information for boundary analysis
		Logger.LogInformation("=== {Description} ===", description);
		Logger.LogInformation("Days: {Days} | Step: {Step}s ({StepHuman}) | Points: {Count} | Density: {Density:F2}/day", 
			daysBack, 
			graphData.Step, 
			FormatDuration(graphData.Step),
			actualDataPoints,
			actualDataPoints / (double)daysBack);
		Logger.LogInformation("");

		// Basic validation
		graphData.Should().NotBeNull();
		graphData.TimeStamps.Should().NotBeEmpty();
		graphData.Step.Should().BeGreaterThan(0);
	}

	/// <summary>
	/// Tests TimePeriod.Zoom boundary behaviour when using DataSourceGraphId = -1 (all data).
	/// This matches the production code pattern used in AU DataMagic and verifies that 
	/// aggregation behaviour is consistent regardless of whether we query a specific graph or all data.
	/// </summary>
	[Theory]
	[InlineData(30, "30 days with GraphId=-1 (should be hourly)")]
	[InlineData(40, "40 days with GraphId=-1 (should be hourly)")]
	[InlineData(42, "42 days with GraphId=-1 (transition zone)")]
	[InlineData(43, "43 days with GraphId=-1 (should degrade)")]
	[InlineData(45, "45 days with GraphId=-1 (adaptive scaling)")]
	[InlineData(90, "90 days with GraphId=-1 (2.2 hour resolution)")]
	public async Task GetGraphData_ZoomWithGraphIdNegativeOne_ShouldMatchSpecificGraphbehaviour(
		int daysBack,
		string description)
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new Filter<ResourceDataSourceInstance>(), CancellationToken);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var endDateTime = DateTime.UtcNow;
		var startDateTime = endDateTime.AddDays(-daysBack);

		// USE -1 FOR GRAPH ID - MATCHES PRODUCTION CODE PATTERN
		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = -1, // Special value: all data (production pattern)
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);

		// Calculate metrics
		var durationInSeconds = (endDateTime - startDateTime).TotalSeconds;
		var actualDataPoints = graphData.TimeStamps.Count;
		var averageInterval = actualDataPoints > 1 
			? durationInSeconds / (actualDataPoints - 1) 
			: graphData.Step;

		// Log detailed information for comparison with specific graph tests
		Logger.LogInformation("=== {Description} (DataSourceGraphId=-1) ===", description);
		Logger.LogInformation("Days: {Days} | Step: {Step}s ({StepHuman}) | Points: {Count} | Density: {Density:F2}/day", 
			daysBack, 
			graphData.Step, 
			FormatDuration(graphData.Step),
			actualDataPoints,
			actualDataPoints / (double)daysBack);
		Logger.LogInformation("");

		// Validate based on known boundaries
		graphData.Should().NotBeNull();
		graphData.TimeStamps.Should().NotBeEmpty();
		
		// Verify the boundary behaviour matches what we discovered with specific graphs
		if (daysBack <= 40)
		{
			graphData.Step.Should().Be(3600, "40 days or less should maintain hourly resolution even with GraphId=-1");
		}
		else if (daysBack >= 43)
		{
			graphData.Step.Should().BeGreaterThan(3600, "43+ days should trigger adaptive scaling even with GraphId=-1");
		}
	}

	/// <summary>
	/// Tests TimePeriod.Zoom with various date ranges to understand LogicMonitor's adaptive aggregation.
	/// Zoom should automatically select appropriate step sizes based on the actual date range.
	/// </summary>
	[Theory]
	[InlineData(1, "1 day with Zoom")]
	[InlineData(2, "2 days with Zoom")]
	[InlineData(6, "6 days with Zoom")]
	[InlineData(7, "7 days with Zoom")]
	[InlineData(8, "8 days with Zoom")]
	[InlineData(14, "14 days with Zoom")]
	[InlineData(30, "30 days (1 month) with Zoom")]
	[InlineData(31, "31 days with Zoom")]
	[InlineData(32, "32 days (just over 1 month) with Zoom")]
	[InlineData(45, "45 days with Zoom")]
	[InlineData(60, "60 days (2 months) with Zoom")]
	[InlineData(90, "90 days (3 months) with Zoom")]
	[InlineData(120, "120 days (4 months) with Zoom")]
	public async Task GetGraphData_ZoomWithVariousDateRanges_ShouldAdaptAggregation(
		int daysBack,
		string description)
	{
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

		var endDateTime = DateTime.UtcNow;
		var startDateTime = endDateTime.AddDays(-daysBack);

		var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
		{
			ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime
		};

		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);

		// Basic validations
		graphData.Should().NotBeNull();
		graphData.TimeStamps.Should().NotBeEmpty();
		graphData.TimeStamps.Should().HaveCountGreaterThan(1, "should have multiple data points for trending");
		
		// Calculate actual metrics
		var durationInSeconds = (endDateTime - startDateTime).TotalSeconds;
		var actualDataPoints = graphData.TimeStamps.Count;
		var averageInterval = actualDataPoints > 1 
			? durationInSeconds / (actualDataPoints - 1) 
			: graphData.Step;

		// Log detailed information for documentation
		Logger.LogInformation("=== {Description} ===", description);
		Logger.LogInformation("Date Range: {Days} days ({Duration:F1} hours)", daysBack, durationInSeconds / 3600);
		Logger.LogInformation("Step Size: {Step}s ({StepHuman})", graphData.Step, FormatDuration(graphData.Step));
		Logger.LogInformation("Data Points: {Count}", actualDataPoints);
		Logger.LogInformation("Average Interval: {Avg:F0}s ({AvgHuman})", averageInterval, FormatDuration(averageInterval));
		Logger.LogInformation("Data Point Density: {Density:F2} points per day", actualDataPoints / (double)daysBack);
		Logger.LogInformation("");

		// Validate reasonable behaviour
		graphData.Step.Should().BeGreaterThan(0, "step size must be positive");
		actualDataPoints.Should().BeLessThan(10000, "should not return excessive data points");
	}

	/// <summary>
	/// Diagnostic test to discover LogicMonitor's actual Step values for different TimePeriods.
	/// This test does NOT assert - it only logs the actual values returned by LogicMonitor.
	/// Use the output to update the expected values in GetGraphData_VariousTimePeriods_ShouldHaveExpectedResolution.
	/// </summary>
	[Fact]
	public async Task DiagnosticTest_DiscoverActualStepValuesForAllTimePeriods()
	{
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

		// Test all TimePeriod enum values
		var timePeriods = new[]
		{
			TimePeriod.OneHour,
			TimePeriod.TwoHours,
			TimePeriod.FiveHours,
			TimePeriod.OneDay,
			TimePeriod.TwoDays,
			TimePeriod.SevenDays,
			TimePeriod.OneMonth,
			TimePeriod.ThreeMonths
		};

		Logger.LogInformation("=== LogicMonitor Aggregation behaviour Diagnostic ===");
		Logger.LogInformation("Testing all TimePeriod values to discover actual Step sizes...");
		Logger.LogInformation("");

		foreach (var timePeriod in timePeriods)
		{
			try
			{
				// Calculate date range based on TimePeriod
				var endDateTime = DateTime.UtcNow;
				var startDateTime = timePeriod switch
				{
					TimePeriod.OneHour => endDateTime.AddHours(-1),
					TimePeriod.TwoHours => endDateTime.AddHours(-2),
					TimePeriod.FiveHours => endDateTime.AddHours(-5),
					TimePeriod.OneDay => endDateTime.AddDays(-1),
					TimePeriod.TwoDays => endDateTime.AddDays(-2),
					TimePeriod.SevenDays => endDateTime.AddDays(-7),
					TimePeriod.OneMonth => endDateTime.AddMonths(-1),
					TimePeriod.ThreeMonths => endDateTime.AddMonths(-3),
					_ => endDateTime.AddHours(-1)
				};

				var deviceGraphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
				{
					ResourceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
					DataSourceGraphId = dataSourceGraph.Id,
					TimePeriod = timePeriod,
					StartDateTime = startDateTime,
					EndDateTime = endDateTime
				};

				var graphData = await LogicMonitorClient
					.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken);

				// Calculate actual duration and expected points based on actual step
				var durationInSeconds = (endDateTime - startDateTime).TotalSeconds;
				var actualDataPoints = graphData.TimeStamps.Count;
				var averageInterval = actualDataPoints > 1 
					? durationInSeconds / (actualDataPoints - 1) 
					: graphData.Step;

				// Log results
				Logger.LogInformation("TimePeriod: {TimePeriod}", timePeriod);
				Logger.LogInformation("  Duration: {Duration:F0} seconds ({DurationHuman})", 
					durationInSeconds, 
					FormatDuration(durationInSeconds));
				Logger.LogInformation("  Actual Step: {Step} seconds ({StepHuman})", 
					graphData.Step, 
					FormatDuration(graphData.Step));
				Logger.LogInformation("  Data Points: {Count}", actualDataPoints);
				Logger.LogInformation("  Average Interval: {Avg:F0} seconds ({AvgHuman})", 
					averageInterval,
					FormatDuration(averageInterval));
				Logger.LogInformation("");
			}
			catch (Exception ex)
			{
				Logger.LogError("  FAILED: {Message}", ex.Message);
				Logger.LogInformation("");
			}
		}

		Logger.LogInformation("=== Diagnostic Complete ===");
		Logger.LogInformation("Use the 'Actual Step' values above to update the expected values in:");
		Logger.LogInformation("  GetGraphData_VariousTimePeriods_ShouldHaveExpectedResolution");
	}

	private static string FormatDuration(double seconds)
	{
		if (seconds < 60)
			return $"{seconds:F0}s";
		if (seconds < 3600)
			return $"{seconds / 60:F1}m";
		if (seconds < 86400)
			return $"{seconds / 3600:F1}h";
		return $"{seconds / 86400:F1}d";
	}
}
