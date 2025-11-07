namespace LogicMonitor.Api.Test.Settings;

public class RoleGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string Value = "Unit Test Role Group";

	[Fact]
	public async Task GetRoleGroups()
	{
		var roleGroups = await LogicMonitorClient
			.GetAllAsync<RoleGroup>(CancellationToken);
		roleGroups.Should().NotBeNull();
		roleGroups.Should().NotBeNullOrEmpty();

		foreach (var role in roleGroups)
		{
			var refetchedRole = await LogicMonitorClient
				.GetAsync<RoleGroup>(role.Id, CancellationToken);
			refetchedRole.Name.Should().Be(role.Name);
		}
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing RoleGroup called "Test"
		var existingRoleGroup = (await LogicMonitorClient
				.GetAllAsync(new Filter<RoleGroup> { FilterItems = [new Eq<RoleGroup>(nameof(RoleGroup.Name), Value)] }, CancellationToken)
				)
			.SingleOrDefault();
		if (existingRoleGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingRoleGroup, cancellationToken: default)
				;
		}

		var roleGroup = await LogicMonitorClient.CreateAsync(new RoleGroupCreationDto
		{
			Name = Value,
			Description = "Desc",
		}, CancellationToken);

		// Refetch
		var refetch = await LogicMonitorClient
			.GetAsync<RoleGroup>(roleGroup.Id, CancellationToken);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient
			.DeleteAsync(roleGroup, cancellationToken: default)
			;
	}
}
