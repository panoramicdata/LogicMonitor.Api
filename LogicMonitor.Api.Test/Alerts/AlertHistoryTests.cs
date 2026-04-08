namespace LogicMonitor.Api.Test.Alerts;

public class AlertHistoryTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private async Task<string> GetRecentAlertIdAsync()
	{
		var alerts = await LogicMonitorClient
			.GetPageAsync(new Filter<Alert> { Skip = 0, Take = 1 }, CancellationToken);
		alerts.Items.Should().NotBeNullOrEmpty("at least one alert must exist in the portal");
		return alerts.Items[0].Id;
	}

	[Fact]
	public async Task GetAlertHistory_Last24Hours_Returns25Items()
	{
		var alertId = await GetRecentAlertIdAsync();
		var request = new AlertHistoryRequest
		{
			Id = alertId,
			HistoryPeriod = AlertHistoryPeriod.Last24Hours
		};

		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		history.Should().NotBeNull();
		history.Histogram.Values.Should().HaveCount(25);
	}

	[Fact]
	public async Task GetAlertHistory_Last7Days_Returns8Items()
	{
		var alertId = await GetRecentAlertIdAsync();
		var request = new AlertHistoryRequest
		{
			Id = alertId,
			HistoryPeriod = AlertHistoryPeriod.Last7Days
		};

		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		history.Should().NotBeNull();
		history.Histogram.Values.Should().HaveCount(8);
	}

	[Fact]
	public async Task GetAlertHistory_Last30Days_Returns31Items()
	{
		var alertId = await GetRecentAlertIdAsync();
		var request = new AlertHistoryRequest
		{
			Id = alertId,
			HistoryPeriod = AlertHistoryPeriod.Last30Days
		};

		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, CancellationToken);

		history.Should().NotBeNull();
		history.Histogram.Values.Should().HaveCount(31);
	}

	[Fact]
	public async Task GetAlertHistory_Custom24Hours_Returns25Items()
	{
		var alertId = await GetRecentAlertIdAsync();
		var start = DateTime.UtcNow.AddDays(-3);
		var end = start.AddDays(1);

		var request = new AlertHistoryRequest
		{
			Id = alertId,
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
			Id = "any-alert-id",
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
			Id = "any-alert-id",
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
			Id = "any-alert-id",
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
