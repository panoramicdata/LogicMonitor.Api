namespace LogicMonitor.Api.Test.Topologies;

public class TopologyGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TestName = "Test Name";

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<TopologyGroup>(CancellationToken);
		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Crud()
	{
		// Delete it if it already exists
		var existingItems = await LogicMonitorClient.GetAllAsync(new Filter<TopologyGroup>
		{
			FilterItems =
			[
				new Eq<TopologyGroup>(nameof(TopologyGroup.Name), TestName)
			]
		}, CancellationToken);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem, CancellationToken);
		}

		// Create it
		var newItem = await LogicMonitorClient
			.CreateAsync(
				new TopologyGroupCreationDto
				{
					Name = TestName,
					Description = "Test Description"
				},
				CancellationToken
			);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem, CancellationToken);
	}

	[Fact]
	public async Task GetTopologyGroup()
	{
		var topGroup = await LogicMonitorClient
			.GetTopologyGroupAsync(1, CancellationToken);

		topGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetTopologiesFromGroup()
	{
		var topologies = await LogicMonitorClient
			.GetTopologiesFromGroupAsync(2, CancellationToken);

		topologies.Items.Should().NotBeNullOrEmpty();
	}
}
