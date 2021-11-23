namespace LogicMonitor.Api.Test.Topologies;

public class TopologyGroupTests : TestWithOutput
{
	private const string TestName = "Test Name";

	public TopologyGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<TopologyGroup>()
			.ConfigureAwait(false);
		Assert.NotNull(items);
		Assert.NotEmpty(items);
	}

	[Fact]
	public async void Crud()
	{
		// Delete it if it already exists
		var existingItems = await LogicMonitorClient.GetAllAsync(new Filter<TopologyGroup>
		{
			FilterItems = new List<FilterItem<TopologyGroup>>
				{
					new Eq<TopologyGroup>(nameof(TopologyGroup.Name), TestName)
				}
		}).ConfigureAwait(false);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem)
				.ConfigureAwait(false);
		}

		// Create it
		var newItem = await LogicMonitorClient
			.CreateAsync(
				new TopologyGroupCreationDto
				{
					Name = TestName,
					Description = "Test Description"
				}
			)
			.ConfigureAwait(false);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem)
			.ConfigureAwait(false);
	}
}
