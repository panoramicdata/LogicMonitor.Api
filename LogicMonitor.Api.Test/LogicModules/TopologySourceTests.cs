namespace LogicMonitor.Api.Test.LogicModules;

public class TopologySourceTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task BasicTopologySourceTest()
	{
		_ = await LogicMonitorClient
			.GetAllAsync<TopologySource>(default)
			.ConfigureAwait(true);
	}
}
