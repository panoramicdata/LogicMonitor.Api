namespace LogicMonitor.Api.Test.Alerts;

public class AlertHistoryTests : TestWithOutput
{
	public AlertHistoryTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAlertHistory_Last24Hours_Returns24Items()
	{
		var request = new AlertHistoryRequest
		{
			Id = "DS25658424",
			HistoryPeriod = AlertHistoryPeriod.Last24Hours
		};

		// Get alert history
		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, default)
			.ConfigureAwait(false);

		history.Should().NotBeNull();
		history.Histogram.Values.Count.Should().Be(25);
	}

	[Fact]
	public async Task GetAlertHistory_Last7Days_Returns8Items()
	{
		var request = new AlertHistoryRequest
		{
			Id = "DS25658424",
			HistoryPeriod = AlertHistoryPeriod.Last7Days
		};

		// Get alert history
		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, default)
			.ConfigureAwait(false);

		history.Should().NotBeNull();
		history.Histogram.Values.Count.Should().Be(8);
	}

	[Fact]
	public async Task GetAlertHistory_Last30Days_Returns31Items()
	{
		var request = new AlertHistoryRequest
		{
			Id = "DS25658424",
			HistoryPeriod = AlertHistoryPeriod.Last30Days
		};

		// Get alert history
		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, default)
			.ConfigureAwait(false);

		history.Should().NotBeNull();
		history.Histogram.Values.Count.Should().Be(31);
	}

	[Fact]
	public async Task GetAlertHistory_Custom24Hours_Returns25Items()
	{
		var start = DateTime.UtcNow.AddDays(-3);
		var end = start.AddDays(1);

		var request = new AlertHistoryRequest
		{
			Id = "DS25658424",
			HistoryPeriod = AlertHistoryPeriod.Custom,
			StartDateTimeUtc = start,
			EndDateTimeUtc = end
		};

		// Get alert history
		var history = await LogicMonitorClient
			.GetAlertHistoryAsync(request, default)
			.ConfigureAwait(false);

		history.Should().NotBeNull();
		history.Histogram.Values.Count.Should().Be(25);
	}

	[Fact]
	public async Task GetAlertHistory_Custom24HoursWithNoStartSpecified_ThrowsArgumentException()
	{
		var start = DateTime.UtcNow.AddDays(-3);
		var end = start.AddDays(1);

		var request = new AlertHistoryRequest
		{
			Id = "DS25658424",
			HistoryPeriod = AlertHistoryPeriod.Custom,
			StartDateTimeUtc = null,
			EndDateTimeUtc = end
		};

		// Set up to attempt retrieval that should throw exception
		var act = async () => await LogicMonitorClient
			.GetAlertHistoryAsync(request, default)
			.ConfigureAwait(false);

		await act
			.Should()
			.ThrowAsync<ArgumentException>()
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task GetAlertHistory_Custom24HoursWithNoEndSpecified_ThrowsArgumentException()
	{
		var start = DateTime.UtcNow.AddDays(-3);
		var end = start.AddDays(1);

		var request = new AlertHistoryRequest
		{
			Id = "DS25658424",
			HistoryPeriod = AlertHistoryPeriod.Custom,
			StartDateTimeUtc = start,
			EndDateTimeUtc = null
		};

		// Set up to attempt retrieval that should throw exception
		var act = async () => await LogicMonitorClient
			.GetAlertHistoryAsync(request, default)
			.ConfigureAwait(false);

		await act
			.Should()
			.ThrowAsync<ArgumentException>()
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task GetAlertHistory_Custom24HoursWithEndBeforeStart_ThrowsArgumentException()
	{
		var start = DateTime.UtcNow.AddDays(-3);
		var end = start.AddDays(-1);

		var request = new AlertHistoryRequest
		{
			Id = "DS25658424",
			HistoryPeriod = AlertHistoryPeriod.Custom,
			StartDateTimeUtc = start,
			EndDateTimeUtc = end
		};

		// Set up to attempt retrieval that should throw exception
		var act = async () => await LogicMonitorClient
			.GetAlertHistoryAsync(request, default)
			.ConfigureAwait(false);

		await act
			.Should()
			.ThrowAsync<ArgumentException>()
			.ConfigureAwait(false);
	}
}
