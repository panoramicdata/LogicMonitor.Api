namespace LogicMonitor.Api.Test.Functions;

public class FunctionTests : TestWithOutput
{
	public FunctionTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetApplications()
	{
		var things = await LogicMonitorClient.GetAppliesToAsync("isCisco()").ConfigureAwait(false);
		Assert.NotNull(things);
		Assert.NotEmpty(things);
	}
}
