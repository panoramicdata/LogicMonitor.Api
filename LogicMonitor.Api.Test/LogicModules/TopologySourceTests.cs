namespace LogicMonitor.Api.Test.LogicModules;

public class TopologySourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public Task BasicTopologySourceTest() =>
		_ = LogicMonitorClient.GetAllAsync<TopologySource>(default);
}
