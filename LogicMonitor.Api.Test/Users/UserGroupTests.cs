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
			.GetAllAsync<UserGroup>(CancellationToken.None)
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
			FilterItems = new List<FilterItem<UserGroup>>
				{
					new Eq<UserGroup>(nameof(ReportGroup.Name), TestName)
				}
		}, CancellationToken.None).ConfigureAwait(false);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem, cancellationToken: CancellationToken.None)
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
				CancellationToken.None
			)
			.ConfigureAwait(false);

		await Task.Delay(TimeSpan.FromSeconds(2))
			.ConfigureAwait(false);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
	}
}
