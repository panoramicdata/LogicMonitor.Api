namespace LogicMonitor.Api.Test.Settings;

public class SingleSignOnTests : TestWithOutput
{
	public SingleSignOnTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetSingleSignOnData()
	{
		var allOpsNotes = await LogicMonitorClient.GetAsync<SingleSignOn>(CancellationToken.None).ConfigureAwait(false);

		// Text should be set
		allOpsNotes.SamlVersion.Should().NotBeNullOrEmpty();
	}
}
