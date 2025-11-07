namespace LogicMonitor.Api.Test.Users;

public class UserGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TestName = "Test Name";

	[Fact]
	public async Task GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<UserGroup>(CancellationToken);
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
		}, CancellationToken);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem, cancellationToken: default)
				;
		}

		await Task.Delay(TimeSpan.FromSeconds(2))
			;

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
			;

		await Task.Delay(TimeSpan.FromSeconds(2))
			;

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem, cancellationToken: default)
			;
	}
}
