namespace LogicMonitor.Api.Test.Settings;

public class MessageSettingsTests : TestWithOutput
{
	public MessageSettingsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void Get()
	{
		var messageTemplate = await LogicMonitorClient.GetAsync<NewUserMessageTemplate>().ConfigureAwait(false);

		string.IsNullOrWhiteSpace(messageTemplate.Subject).Should().BeFalse();
		string.IsNullOrWhiteSpace(messageTemplate.Body).Should().BeFalse();
	}
}
