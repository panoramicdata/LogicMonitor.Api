namespace LogicMonitor.Api.Test.Settings;

public class ApiTokensTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllAsync_Succeeds()
	{
		var logicMonitorClient = LogicMonitorClient;
		var apiTokens = await logicMonitorClient
			.GetAllAsync<ApiToken>(cancellationToken: default)
			;

		apiTokens.Should().NotBeNullOrEmpty();
	}
}
