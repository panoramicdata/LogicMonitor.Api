namespace LogicMonitor.Api.Test.Users;

public class RoleGroupTests : TestWithOutput
{
	private const string TestName = "Test Name";

	public RoleGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<RoleGroup>(default)
			.ConfigureAwait(false);
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
				new RoleGroupCreationDto
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
}
