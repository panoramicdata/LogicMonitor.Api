namespace LogicMonitor.Api.Test.Settings;

public class SingleSignOnTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetSingleSignOnData()
	{
		var allOpsNotes = await LogicMonitorClient.GetAsync<SingleSignOn>(CancellationToken);

		// Text should be set
		allOpsNotes.SamlVersion.Should().NotBeNullOrEmpty();
	}
}
