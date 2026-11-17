using System.Globalization;

namespace LogicMonitor.Api.Test.Data;

public partial class DataTests
{

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

	private static string FormatDuration(double seconds)
	{
		if (seconds < 60)
		{
			return $"{seconds:F0}s";
		}

		if (seconds < 3600)
		{
			return $"{seconds / 60:F1}m";
		}

		if (seconds < 86400)
		{
			return $"{seconds / 3600:F1}h";
		}

		return $"{seconds / 86400:F1}d";
	}
}
