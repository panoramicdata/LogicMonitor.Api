namespace LogicMonitor.Api.Test.Settings;

public class MessageSettingsTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task Get()
	{
		var messageTemplate = await LogicMonitorClient
			.GetAsync<NewUserMessageTemplate>(default)
			.ConfigureAwait(true);

		string.IsNullOrWhiteSpace(messageTemplate.Subject).Should().BeFalse();
		string.IsNullOrWhiteSpace(messageTemplate.Body).Should().BeFalse();
	}
}
