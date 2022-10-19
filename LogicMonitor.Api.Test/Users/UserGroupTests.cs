namespace LogicMonitor.Api.Test.Users;

public class UserGroupTests : TestWithOutput
{
	private const string TestName = "Test Name";

	public UserGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<UserGroup>()
			.ConfigureAwait(false);
		items.Should().NotBeNull();
		items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void Crud()
	{
		// Delete it if it already exists
		var existingItems = await LogicMonitorClient.GetAllAsync(new Filter<UserGroup>
		{
			FilterItems = new List<FilterItem<UserGroup>>
				{
					new Eq<UserGroup>(nameof(ReportGroup.Name), TestName)
				}
		}).ConfigureAwait(false);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem)
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
				}
			)
			.ConfigureAwait(false);

		await Task.Delay(TimeSpan.FromSeconds(2))
			.ConfigureAwait(false);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem)
			.ConfigureAwait(false);
	}
}
