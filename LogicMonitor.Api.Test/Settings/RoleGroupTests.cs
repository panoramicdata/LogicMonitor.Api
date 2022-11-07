namespace LogicMonitor.Api.Test.Settings;

public class RoleGroupTests : TestWithOutput
{
	private const string Value = "Unit Test Role Group";

	public RoleGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetRoleGroups()
	{
		var roleGroups = await LogicMonitorClient.GetAllAsync<RoleGroup>().ConfigureAwait(false);
		roleGroups.Should().NotBeNull();
		roleGroups.Should().NotBeNullOrEmpty();

		foreach (var role in roleGroups)
		{
			var refetchedRole = await LogicMonitorClient.GetAsync<RoleGroup>(role.Id).ConfigureAwait(false);
			refetchedRole.Name.Should().Be(role.Name);
		}
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing RoleGroup called "Test"
		var existingRoleGroup = (await LogicMonitorClient
				.GetAllAsync(new Filter<RoleGroup> { FilterItems = new List<FilterItem<RoleGroup>> { new Eq<RoleGroup>(nameof(RoleGroup.Name), Value) } })
				.ConfigureAwait(false))
			.SingleOrDefault();
		if (existingRoleGroup is not null)
		{
			await LogicMonitorClient.DeleteAsync(existingRoleGroup).ConfigureAwait(false);
		}

		var roleGroup = await LogicMonitorClient.CreateAsync(new RoleGroupCreationDto
		{
			Name = Value,
			Description = "Desc",
		}).ConfigureAwait(false);

		// Refetch
		var refetch = await LogicMonitorClient.GetAsync<RoleGroup>(roleGroup.Id).ConfigureAwait(false);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient.DeleteAsync(roleGroup).ConfigureAwait(false);
	}
}
