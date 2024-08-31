namespace LogicMonitor.Api.Test.Settings;

public class MessageSettingsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task Get()
	{
		var messageTemplate = await LogicMonitorClient
			.GetAsync<NewUserMessageTemplate>(default)
			.ConfigureAwait(true);

		messageTemplate.Subject.Should().NotBeNullOrWhiteSpace();
		messageTemplate.Body.Should().NotBeNullOrWhiteSpace();
	}
}
