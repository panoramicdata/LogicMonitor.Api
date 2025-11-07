namespace LogicMonitor.Api.Test.Settings;

public class TimeTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetTimeZoneSetting()
	{
		var timeZoneSetting = await LogicMonitorClient
			.GetTimeZoneSettingAsync(CancellationToken);

		// Text should be set
		timeZoneSetting.TimeZone.Should().NotBeNullOrWhiteSpace();
	}
}
