namespace LogicMonitor.Api.Test.Alerts;

public class AlertStatTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAlertStat()
	{
		var alertStat = await LogicMonitorClient
			.GetAsync<AlertStat>(default)
			.ConfigureAwait(true);
		alertStat.Should().NotBeNull();
	}
}
