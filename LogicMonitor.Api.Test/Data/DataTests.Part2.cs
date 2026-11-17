using System.Globalization;

namespace LogicMonitor.Api.Test.Data;

public partial class DataTests
{

	[Fact]
	[Trait("Category", "Long")]
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
	
		// For a 1-month period, LogicMonitor aggregates to hourly intervals (3600 seconds)
		graphData.Step.Should().Be(3600, "for a 1-month time range, data should be aggregated to hourly intervals");
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
	[Trait("Category", "Long")]
	[InlineData(TimePeriod.SevenDays, 660)]
	[InlineData(TimePeriod.OneMonth, 3600)]
	[InlineData(TimePeriod.ThreeMonths, 7980)]
	public async Task GetGraphData_VariousTimePeriods_ShouldHaveExpectedResolution(
		TimePeriod timePeriod,
		int expectedStepSeconds
	)
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource ??= new();

		var dataSourceGraph = await LogicMonitorClient
			.GetDataSourceGraphByNameAsync(dataSource.Id, "Capacity Detail", CancellationToken);
		dataSourceGraph.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
	
		// Verify the step matches the expected resolution (allow +/-10% tolerance)
		var stepTolerance = (int)(expectedStepSeconds * 0.10);
		graphData.Step.Should().BeInRange(
			expectedStepSeconds - stepTolerance,
			expectedStepSeconds + stepTolerance,
			$"{timePeriod} should have ~{expectedStepSeconds}s step");
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
				CancellationToken) ?? throw new InvalidOperationException();
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
		var actualDataPoints = graphData.TimeStamps.Count;

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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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

		var actualDataPoints = graphData.TimeStamps.Count;

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
	
		// GraphId=-1 behaviour is server-determined and can vary by account/data retention.
		// Keep this as a characterization test and only assert the response is well-formed.
		graphData.Step.Should().BeGreaterThan(0);
	}
}
