namespace LogicMonitor.Api.Test.Settings;

public class UserGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string Value = "Unit Test User Group";

	[Fact]
	public async Task GetUserGroups()
	{
		var userGroups = await LogicMonitorClient
			.GetAllAsync<UserGroup>(CancellationToken);
		userGroups.Should().NotBeNull();
		userGroups.Should().NotBeNullOrEmpty();

		foreach (var user in userGroups)
		{
			var refetchedUser = await LogicMonitorClient
				.GetAsync<UserGroup>(user.Id, CancellationToken);
			refetchedUser.Name.Should().Be(user.Name);
		}
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing UserGroup called "Test"
		var existingUserGroup = (await LogicMonitorClient
				.GetAllAsync(new Filter<UserGroup> { FilterItems = [new Eq<UserGroup>(nameof(UserGroup.Name), Value)] }, CancellationToken)
				)
			.SingleOrDefault();
		if (existingUserGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingUserGroup, CancellationToken)
				;
		}

		var userGroup = await LogicMonitorClient.CreateAsync(new UserGroupCreationDto
		{
			Name = Value,
			Description = "Desc",
		}, CancellationToken);

		// Refetch
		var refetch = await LogicMonitorClient
			.GetAsync<UserGroup>(userGroup.Id, CancellationToken);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient
			.DeleteAsync(userGroup, CancellationToken)
			;
	}
}
