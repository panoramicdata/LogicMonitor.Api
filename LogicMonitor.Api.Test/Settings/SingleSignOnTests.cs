namespace LogicMonitor.Api.Test.Settings;

public class SingleSignOnTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetSingleSignOnData()
	{
		var allOpsNotes = await LogicMonitorClient.GetAsync<SingleSignOn>(default).ConfigureAwait(false);

		// Text should be set
		allOpsNotes.SamlVersion.Should().NotBeNullOrEmpty();
	}
}
