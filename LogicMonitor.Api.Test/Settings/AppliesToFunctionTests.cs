namespace LogicMonitor.Api.Test.Settings;

public class AppliesToFunctionTests : TestWithOutput
{
	public AppliesToFunctionTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAppliesToFunctionListSucceeds()
	{
		var appliesToFunctions = await LogicMonitorClient.GetAppliesToFunctionListAsync(new Filter<AppliesToFunction>(), default).ConfigureAwait(false);
		appliesToFunctions.Should().NotBeNull();
		appliesToFunctions.Items.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetSpecificAppliesToFunctionSucceeds()
	{
		var appliesToFunction = await LogicMonitorClient.GetAppliesToFunctionAsync(1, default).ConfigureAwait(false);
		appliesToFunction.Name.Should().Be("NetSNMPComputers");
	}

}
