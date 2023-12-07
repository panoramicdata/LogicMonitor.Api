namespace LogicMonitor.Api.Test.Topologies;

public class TopologyTests : TestWithOutput
{
	public TopologyTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllTopologies()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<Topology>(default)
			.ConfigureAwait(true);
		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetTopologyDataForDevice()
	{
		var resourceIds = new[] { 1138, 988, 228 };

		foreach (var resourceId in resourceIds)
		{
			var item = await LogicMonitorClient
				.GetTopologyDataAsync(
					new TopologyDataRequest
					{
						ResourceId = resourceId,
						Algorithm = TopologyAlgorithm.SecondDegreeAway,
					},
					default
				)
				.ConfigureAwait(true);
			item.Should().NotBeNull();
		}
	}
}
