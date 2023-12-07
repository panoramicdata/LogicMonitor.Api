namespace LogicMonitor.Api.Test.Users;

public class UserGroupTests : TestWithOutput
{
	private const string TestName = "Test Name";

	public UserGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<UserGroup>(default)
			.ConfigureAwait(false);
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
		}, default).ConfigureAwait(false);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem, cancellationToken: default)
				.ConfigureAwait(false);
		}

		await Task.Delay(TimeSpan.FromSeconds(2))
			.ConfigureAwait(false);

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
			.ConfigureAwait(false);

		await Task.Delay(TimeSpan.FromSeconds(2))
			.ConfigureAwait(false);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem, cancellationToken: default)
			.ConfigureAwait(false);
	}
}
