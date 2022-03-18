namespace LogicMonitor.Api.Test.Settings;

public class IntegrationTests : TestWithOutput
{
	public IntegrationTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetIntegrations()
	{
		var integrations = await LogicMonitorClient.GetAllAsync<Integration>().ConfigureAwait(false);

		// Text should be set
		Assert.All(integrations, on => string.IsNullOrWhiteSpace(on.Name).Should().BeFalse());
	}
}
