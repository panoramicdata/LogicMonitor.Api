namespace LogicMonitor.Api.Test.Functions;

public class FunctionTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetApplications()
	{
		var things = await LogicMonitorClient
			.GetAppliesToAsync("isCisco()", default)
			.ConfigureAwait(true);
		things.Should().NotBeNull();
		things.Should().NotBeNullOrEmpty();
	}
}
