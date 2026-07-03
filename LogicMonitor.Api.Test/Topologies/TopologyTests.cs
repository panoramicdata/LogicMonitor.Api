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
	public async Task Crud()
	{
		// A topology group to hold the map
		var group = await LogicMonitorClient
			.CreateAsync(
				new TopologyGroupCreationDto { Name = "Test Topology Map Group", Description = "Test" },
				CancellationToken);
		try
		{
			// Seed the map with the first available device
			var devices = await LogicMonitorClient.GetAllAsync<Resource>(CancellationToken);
			devices.Should().NotBeNullOrEmpty();
			var deviceId = devices[0].Id;

			// Create the map
			var created = await LogicMonitorClient
				.CreateAsync(
					new TopologyCreationDto
					{
						Name = "Test Topology Map",
						GroupId = group.Id,
						Layout = new TopologyLayout { Mode = TopologyLayoutMode.Radial },
						Views =
						[
							new TopologyView
							{
								ResourceId = Guid.NewGuid().ToString(),
								Resource = $"device.{deviceId}",
								Algorithm = TopologyAlgorithm.FirstDegreeAway,
								EdgeTypes = [new EdgeTypeAndDirection { Type = "Network", Direction = EdgeDirection.In }],
							}
						],
					},
					CancellationToken);
			created.Id.Should().BeGreaterThan(0);

			// Get it back by id
			var fetched = await LogicMonitorClient.GetTopologyAsync(created.Id, CancellationToken);
			fetched.Should().NotBeNull();
			fetched.Name.Should().Be("Test Topology Map");
			fetched.GroupId.Should().Be(group.Id);

			// Patch it
			await LogicMonitorClient
				.PatchAsync(fetched, new Dictionary<string, object> { ["showEdgeStatus"] = true }, CancellationToken);

			// Delete the map
			await LogicMonitorClient.DeleteAsync(fetched, CancellationToken);
		}
		finally
		{
			await LogicMonitorClient.DeleteAsync(group, CancellationToken);
		}
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
					CancellationToken
				);
			item.Should().NotBeNull();
		}
	}
}
