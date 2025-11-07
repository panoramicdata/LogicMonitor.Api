namespace LogicMonitor.Api.Test.LogicModules;

public class TopologySourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public Task BasicTopologySourceTest() =>
		_ = LogicMonitorClient.GetAllAsync<TopologySource>(CancellationToken);
}
