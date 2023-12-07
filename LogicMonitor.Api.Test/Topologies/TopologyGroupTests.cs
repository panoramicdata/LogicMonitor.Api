namespace LogicMonitor.Api.Test.Topologies;

public class TopologyGroupTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	private const string TestName = "Test Name";

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<TopologyGroup>(default)
			.ConfigureAwait(true);
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
		}, default).ConfigureAwait(true);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem, cancellationToken: default)
				.ConfigureAwait(true);
		}

		// Create it
		var newItem = await LogicMonitorClient
			.CreateAsync(
				new TopologyGroupCreationDto
				{
					Name = TestName,
					Description = "Test Description"
				},
				default
			)
			.ConfigureAwait(true);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem, cancellationToken: default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task GetTopologyGroup()
	{
		var topGroup = await LogicMonitorClient
			.GetTopologyGroupAsync(1, default)
			.ConfigureAwait(true);

		topGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetTopologiesFromGroup()
	{
		var topologies = await LogicMonitorClient
			.GetTopologiesFromGroupAsync(2, default)
			.ConfigureAwait(true);

		topologies.Items.Should().NotBeNullOrEmpty();
	}
}
