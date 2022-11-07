namespace LogicMonitor.Api.Test.Settings;

public class UserGroupTests : TestWithOutput
{
	private const string Value = "Unit Test User Group";

	public UserGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetUserGroups()
	{
		var userGroups = await LogicMonitorClient.GetAllAsync<UserGroup>().ConfigureAwait(false);
		userGroups.Should().NotBeNull();
		userGroups.Should().NotBeNullOrEmpty();

		foreach (var user in userGroups)
		{
			var refetchedUser = await LogicMonitorClient.GetAsync<UserGroup>(user.Id).ConfigureAwait(false);
			refetchedUser.Name.Should().Be(user.Name);
		}
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing UserGroup called "Test"
		var existingUserGroup = (await LogicMonitorClient
				.GetAllAsync(new Filter<UserGroup> { FilterItems = new List<FilterItem<UserGroup>> { new Eq<UserGroup>(nameof(UserGroup.Name), Value) } })
				.ConfigureAwait(false))
			.SingleOrDefault();
		if (existingUserGroup is not null)
		{
			await LogicMonitorClient.DeleteAsync(existingUserGroup).ConfigureAwait(false);
		}

		var userGroup = await LogicMonitorClient.CreateAsync(new UserGroupCreationDto
		{
			Name = Value,
			Description = "Desc",
		}).ConfigureAwait(false);

		// Refetch
		var refetch = await LogicMonitorClient.GetAsync<UserGroup>(userGroup.Id).ConfigureAwait(false);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient.DeleteAsync(userGroup).ConfigureAwait(false);
	}
}
