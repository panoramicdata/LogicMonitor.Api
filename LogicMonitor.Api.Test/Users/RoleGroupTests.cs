namespace LogicMonitor.Api.Test.Users;

public class RoleGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TestName = "Test Name";

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<RoleGroup>(CancellationToken);
		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Crud()
	{
		// Delete it if it already exists
		var existingItems = await LogicMonitorClient.GetAllAsync(new Filter<RoleGroup>
		{
			FilterItems =
			[
				new Eq<RoleGroup>(nameof(RoleGroup.Name), TestName)
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
				new RoleGroupCreationDto
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
}
