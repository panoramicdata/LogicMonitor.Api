namespace LogicMonitor.Api.Test.Topologies;

public class TopologyTests : TestWithOutput
{
	private const string TestName = "Test Name";

	public TopologyTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<Topology>(default)
			.ConfigureAwait(false);
		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}
}
