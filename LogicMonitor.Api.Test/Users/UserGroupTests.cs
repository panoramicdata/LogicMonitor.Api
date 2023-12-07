namespace LogicMonitor.Api.Test.Users;

public class UserGroupTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	private const string TestName = "Test Name";

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<UserGroup>(default)
			.ConfigureAwait(true);
		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Crud()
	{
		// Delete it if it already exists
		var existingItems = await LogicMonitorClient.GetAllAsync(new Filter<UserGroup>
		{
			FilterItems =
				[
					new Eq<UserGroup>(nameof(ReportGroup.Name), TestName)
				]
		}, default).ConfigureAwait(true);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem, cancellationToken: default)
				.ConfigureAwait(true);
		}

		await Task.Delay(TimeSpan.FromSeconds(2))
			.ConfigureAwait(true);

		// Create it
		var newItem = await LogicMonitorClient
			.CreateAsync(
				new ReportGroupCreationDto
				{
					Name = TestName,
					Description = "Test Description"
				},
				default
			)
			.ConfigureAwait(true);

		await Task.Delay(TimeSpan.FromSeconds(2))
			.ConfigureAwait(true);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem, cancellationToken: default)
			.ConfigureAwait(true);
	}
}
