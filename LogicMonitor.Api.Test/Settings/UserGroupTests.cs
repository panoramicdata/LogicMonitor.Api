namespace LogicMonitor.Api.Test.Settings;

public class UserGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	private const string Value = "Unit Test User Group";

	[Fact]
	public async Task GetUserGroups()
	{
		var userGroups = await LogicMonitorClient
			.GetAllAsync<UserGroup>(default)
			.ConfigureAwait(true);
		userGroups.Should().NotBeNull();
		userGroups.Should().NotBeNullOrEmpty();

		foreach (var user in userGroups)
		{
			var refetchedUser = await LogicMonitorClient
				.GetAsync<UserGroup>(user.Id, default)
				.ConfigureAwait(true);
			refetchedUser.Name.Should().Be(user.Name);
		}
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing UserGroup called "Test"
		var existingUserGroup = (await LogicMonitorClient
				.GetAllAsync(new Filter<UserGroup> { FilterItems = [new Eq<UserGroup>(nameof(UserGroup.Name), Value)] }, default)
				.ConfigureAwait(true))
			.SingleOrDefault();
		if (existingUserGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingUserGroup, cancellationToken: default)
				.ConfigureAwait(true);
		}

		var userGroup = await LogicMonitorClient.CreateAsync(new UserGroupCreationDto
		{
			Name = Value,
			Description = "Desc",
		}, default).ConfigureAwait(true);

		// Refetch
		var refetch = await LogicMonitorClient
			.GetAsync<UserGroup>(userGroup.Id, default)
			.ConfigureAwait(true);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient
			.DeleteAsync(userGroup, cancellationToken: default)
			.ConfigureAwait(true);
	}
}
