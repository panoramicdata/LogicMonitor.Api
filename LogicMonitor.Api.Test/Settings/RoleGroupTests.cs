namespace LogicMonitor.Api.Test.Settings;

public class RoleGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	private const string Value = "Unit Test Role Group";

	[Fact]
	public async Task GetRoleGroups()
	{
		var roleGroups = await LogicMonitorClient
			.GetAllAsync<RoleGroup>(default)
			.ConfigureAwait(true);
		roleGroups.Should().NotBeNull();
		roleGroups.Should().NotBeNullOrEmpty();

		foreach (var role in roleGroups)
		{
			var refetchedRole = await LogicMonitorClient
				.GetAsync<RoleGroup>(role.Id, default)
				.ConfigureAwait(true);
			refetchedRole.Name.Should().Be(role.Name);
		}
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing RoleGroup called "Test"
		var existingRoleGroup = (await LogicMonitorClient
				.GetAllAsync(new Filter<RoleGroup> { FilterItems = [new Eq<RoleGroup>(nameof(RoleGroup.Name), Value)] }, default)
				.ConfigureAwait(true))
			.SingleOrDefault();
		if (existingRoleGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingRoleGroup, cancellationToken: default)
				.ConfigureAwait(true);
		}

		var roleGroup = await LogicMonitorClient.CreateAsync(new RoleGroupCreationDto
		{
			Name = Value,
			Description = "Desc",
		}, default).ConfigureAwait(true);

		// Refetch
		var refetch = await LogicMonitorClient
			.GetAsync<RoleGroup>(roleGroup.Id, default)
			.ConfigureAwait(true);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient
			.DeleteAsync(roleGroup, cancellationToken: default)
			.ConfigureAwait(true);
	}
}
