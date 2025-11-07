namespace LogicMonitor.Api.Test.Functions;

public class FunctionTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetApplications()
	{
		var things = await LogicMonitorClient
			.GetAppliesToAsync("isCisco()", CancellationToken);
		things.Should().NotBeNull();
		things.Should().NotBeNullOrEmpty();
	}
}
