namespace LogicMonitor.Api.Test.Settings;

public class ApiTokensTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetAllAsync_Succeeds()
	{
		var logicMonitorClient = LogicMonitorClient;
		var apiTokens = await logicMonitorClient
			.GetAllAsync<ApiToken>(cancellationToken: default)
			.ConfigureAwait(true);

		apiTokens.Should().NotBeNullOrEmpty();
	}
}
