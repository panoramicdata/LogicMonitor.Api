namespace LogicMonitor.Api.Test.Settings;

public class IntegrationTests : TestWithOutput
{
	public IntegrationTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetIntegrations()
	{
		var integrations = await LogicMonitorClient.GetAllAsync<Integration>(CancellationToken.None).ConfigureAwait(false);

		// Text should be set
		integrations.Should().AllSatisfy(on => string.IsNullOrWhiteSpace(on.Name).Should().BeFalse());
	}
}
