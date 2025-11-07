namespace LogicMonitor.Api.Test.Topologies;

public class TopologyTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllTopologies()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<Topology>(CancellationToken);
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
				;
			item.Should().NotBeNull();
		}
	}
}
