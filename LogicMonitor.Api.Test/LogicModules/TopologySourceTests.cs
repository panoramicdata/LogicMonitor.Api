namespace LogicMonitor.Api.Test.LogicModules;

public class TopologySourceTests : TestWithOutput
{
	public TopologySourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task BasicTopologySourceTest()
	{
		var _ = await LogicMonitorClient.GetAllAsync<TopologySource>(default).ConfigureAwait(false);
	}
}
