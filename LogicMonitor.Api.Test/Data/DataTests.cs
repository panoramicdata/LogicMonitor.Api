using System.Globalization;

namespace LogicMonitor.Api.Test.Data;

public partial class DataTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{

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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
				await LogStepsForTimePeriodAsync(timePeriod, deviceDataSourceInstances, dataSourceGraph);
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

	private async Task LogStepsForTimePeriodAsync(TimePeriod timePeriod, List<ResourceDataSourceInstance> deviceDataSourceInstances, DataSourceGraph dataSourceGraph)
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

	[Fact]
	[Trait("Category", "Long")]
	public async Task GetForecastGraphData()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();

		var dataSourceGraphs = await LogicMonitorClient
			.GetDataSourceGraphsAsync(dataSource.Id, CancellationToken);

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();

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
	[Trait("Category", "Long")]
	public async Task GetOverviewGraphData()
	{
		var device = await GetSnmpResourceAsync(CancellationToken);
		device.Should().NotBeNull();
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("snmp64_If-", CancellationToken);
		dataSource.Should().NotBeNull();
		dataSource ??= new();
		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstanceGroups = await LogicMonitorClient
			.GetResourceDataSourceInstanceGroupsAsync(device.Id, deviceDataSource.Id, CancellationToken);
		deviceDataSourceInstanceGroups.Should().NotBeNull();
		deviceDataSourceInstanceGroups.Should().NotBeNullOrEmpty();
		var deviceDataSourceInstanceGroup = deviceDataSourceInstanceGroups.First();
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
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
	[Trait("Category", "Long")]
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
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken) ?? throw new InvalidOperationException();
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new Filter<ResourceDataSourceInstance>(), CancellationToken);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var startDateTime = DateTime.UtcNow.AddHours(-1);
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
		graphData.TimeStamps.Should().HaveCountGreaterThanOrEqualTo(30,
			"a 1-hour window of Ping data with ~60s intervals should yield at least 30 timestamps");
	}
}
