namespace LogicMonitor.Api.Test.LogicModules;

public class TopologySourceTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public Task BasicTopologySourceTest() =>
		_ = LogicMonitorClient.GetAllAsync<TopologySource>(default);
}
