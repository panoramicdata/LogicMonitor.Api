using LogicMonitor.Api.Alerts;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Alerts
{
	public class AlertStatTests : TestWithOutput
	{
		public AlertStatTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetAlertStat()
		{
			var alertStat = await LogicMonitorClient.GetAsync<AlertStat>().ConfigureAwait(false);
			Assert.NotNull(alertStat);
		}
	}
}