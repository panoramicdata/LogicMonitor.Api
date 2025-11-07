namespace LogicMonitor.Api.Test.Alerts;

public class AlertStatTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAlertStat()
	{
		var alertStat = await LogicMonitorClient
			.GetAsync<AlertStat>(CancellationToken);
		alertStat.Should().NotBeNull();
	}
}
