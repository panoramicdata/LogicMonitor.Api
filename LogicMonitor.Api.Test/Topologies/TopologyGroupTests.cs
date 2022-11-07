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
			.GetAllAsync<TopologyGroup>(CancellationToken.None)
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
		}, CancellationToken.None).ConfigureAwait(false);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem, cancellationToken: CancellationToken.None)
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
				CancellationToken.None
			)
			.ConfigureAwait(false);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
	}
}
