namespace LogicMonitor.Api.Test.Functions;

public class FunctionTests : TestWithOutput
{
	public FunctionTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

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
