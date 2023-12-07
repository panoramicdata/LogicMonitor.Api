namespace LogicMonitor.Api.Test.Settings;

public class TimeTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetTimeZoneSetting()
	{
		var timeZoneSetting = await LogicMonitorClient
			.GetTimeZoneSettingAsync(default)
			.ConfigureAwait(true);

		// Text should be set
		timeZoneSetting.TimeZone.Should().NotBeNullOrWhiteSpace();
	}
}
