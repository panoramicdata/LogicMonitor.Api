namespace LogicMonitor.Api.Test.Alerts;

public class AlertHistoryTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAlertHistory_Last24Hours_Returns24Items()
	{
		var request = new AlertHistoryRequest
		{
			Id = "DS26985243",
			HistoryPeriod = AlertHistoryPeriod.Last24Hours
		};

		// Get alert history
		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		history.Should().NotBeNull();
		history.Histogram.Values.Should().HaveCount(25);
	}

	[Fact]
	public async Task GetAlertHistory_Last7Days_Returns8Items()
	{
		var request = new AlertHistoryRequest
		{
			Id = "DS26985243",
			HistoryPeriod = AlertHistoryPeriod.Last7Days
		};

		// Get alert history
		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		history.Should().NotBeNull();
		history.Histogram.Values.Should().HaveCount(8);
	}

	[Fact]
	public async Task GetAlertHistory_Last30Days_Returns31Items()
	{
		var request = new AlertHistoryRequest
		{
			Id = "DS26985243",
			HistoryPeriod = AlertHistoryPeriod.Last30Days
		};

		// Get alert history
		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		history.Should().NotBeNull();
		history.Histogram.Values.Should().HaveCount(31);
	}

	[Fact]
	public async Task GetAlertHistory_Custom24Hours_Returns25Items()
	{
		var start = DateTime.UtcNow.AddDays(-3);
		var end = start.AddDays(1);

		var request = new AlertHistoryRequest
		{
			Id = "DS26985243",
			HistoryPeriod = AlertHistoryPeriod.Custom,
			StartDateTimeUtc = start,
			EndDateTimeUtc = end
		};

		// Get alert history
		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		history.Should().NotBeNull();
		history.Histogram.Values.Should().HaveCount(25);
	}

	[Fact]
	public Task GetAlertHistory_Custom24HoursWithNoStartSpecified_ThrowsArgumentException()
	{
		var start = DateTime.UtcNow.AddDays(-3);
		var end = start.AddDays(1);

		var request = new AlertHistoryRequest
		{
			Id = "DS26985243",
			HistoryPeriod = AlertHistoryPeriod.Custom,
			StartDateTimeUtc = null,
			EndDateTimeUtc = end
		};

		// Set up to attempt retrieval that should throw exception
		var act = async () => await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		return act
			.Should()
			.ThrowAsync<ArgumentException>();
	}

	[Fact]
	public Task GetAlertHistory_Custom24HoursWithNoEndSpecified_ThrowsArgumentException()
	{
		var start = DateTime.UtcNow.AddDays(-3);

		var request = new AlertHistoryRequest
		{
			Id = "DS26985243",
			HistoryPeriod = AlertHistoryPeriod.Custom,
			StartDateTimeUtc = start,
			EndDateTimeUtc = null
		};

		// Set up to attempt retrieval that should throw exception
		var act = async () => await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		return act
			.Should()
			.ThrowAsync<ArgumentException>();
	}

	[Fact]
	public Task GetAlertHistory_Custom24HoursWithEndBeforeStart_ThrowsArgumentException()
	{
		var start = DateTime.UtcNow.AddDays(-3);
		var end = start.AddDays(-1);

		var request = new AlertHistoryRequest
		{
			Id = "DS26985243",
			HistoryPeriod = AlertHistoryPeriod.Custom,
			StartDateTimeUtc = start,
			EndDateTimeUtc = end
		};

		// Set up to attempt retrieval that should throw exception
		var act = async () => await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		return act
			.Should()
			.ThrowAsync<ArgumentException>();
	}
}
