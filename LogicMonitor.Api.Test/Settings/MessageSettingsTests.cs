namespace LogicMonitor.Api.Test.Settings;

public class MessageSettingsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task Get()
	{
		var messageTemplate = await LogicMonitorClient
			.GetAsync<NewUserMessageTemplate>(CancellationToken);

		messageTemplate.Subject.Should().NotBeNullOrWhiteSpace();
		messageTemplate.Body.Should().NotBeNullOrWhiteSpace();
	}
}
