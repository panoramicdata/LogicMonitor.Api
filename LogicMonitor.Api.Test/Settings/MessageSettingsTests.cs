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

		Assert.False(string.IsNullOrWhiteSpace(messageTemplate.Subject));
		Assert.False(string.IsNullOrWhiteSpace(messageTemplate.Body));
	}
}
