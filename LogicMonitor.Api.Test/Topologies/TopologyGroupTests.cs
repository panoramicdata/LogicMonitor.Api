namespace LogicMonitor.Api.Test.Topologies;

public class TopologyGroupTests : TestWithOutput
{
	private const string TestName = "Test Name";

	public TopologyGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<TopologyGroup>(default)
			.ConfigureAwait(false);
		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Crud()
	{
		// Delete it if it already exists
		var existingItems = await LogicMonitorClient.GetAllAsync(new Filter<TopologyGroup>
		{
			FilterItems = new List<FilterItem<TopologyGroup>>
				{
					new Eq<TopologyGroup>(nameof(TopologyGroup.Name), TestName)
				}
		}, default).ConfigureAwait(false);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem, cancellationToken: default)
				.ConfigureAwait(false);
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
			.ConfigureAwait(false);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem, cancellationToken: default)
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task GetTopologyGroup()
	{
		var topGroup = await LogicMonitorClient
			.GetTopologyGroupAsync(1, default)
			.ConfigureAwait(false);

		topGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetTopologiesFromGroup()
	{
		var topologies = await LogicMonitorClient
			.GetTopologiesFromGroupAsync(2, default)
			.ConfigureAwait(false);

		topologies.Items.Should().NotBeNullOrEmpty();
	}
}
