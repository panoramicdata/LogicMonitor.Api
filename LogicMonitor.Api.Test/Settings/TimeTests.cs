namespace LogicMonitor.Api.Test.Settings;

public class TimeTests : TestWithOutput
{
	public TimeTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetTimeZoneSetting()
	{
		var timeZoneSetting = await LogicMonitorClient.GetTimeZoneSettingAsync().ConfigureAwait(false);

		// Text should be set
		string.IsNullOrWhiteSpace(timeZoneSetting.TimeZone).Should().BeFalse();
	}
}
