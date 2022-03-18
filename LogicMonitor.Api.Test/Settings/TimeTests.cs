namespace LogicMonitor.Api.Test.Settings;

public class TimeTests : TestWithOutput
{
	public TimeTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetTimeZoneSetting()
	{
		var timeZoneSetting = await LogicMonitorClient.GetTimeZoneSettingAsync().ConfigureAwait(false);

		// Text should be set
		string.IsNullOrWhiteSpace(timeZoneSetting.TimeZone).Should().BeFalse();

		// Offset should be present (include to verify with non-GMT)
		//Assert.True(timeZoneSetting.UtcOffsetSeconds != 0);
	}
}
