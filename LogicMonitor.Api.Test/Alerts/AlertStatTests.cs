namespace LogicMonitor.Api.Test.Alerts;

public class AlertStatTests : TestWithOutput
{
	public AlertStatTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAlertStat()
	{
		var alertStat = await LogicMonitorClient.GetAsync<AlertStat>().ConfigureAwait(false);
		alertStat.Should().NotBeNull();
	}
}
