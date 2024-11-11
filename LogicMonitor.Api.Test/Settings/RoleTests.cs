using System.Globalization;

namespace LogicMonitor.Api.Test.Settings;

public class RoleTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	private const string Value = "Unit Test Role";

	[Fact]
	public async Task GetRoles()
	{
		var roles = await LogicMonitorClient
			.GetPageAsync(new Filter<Role> { Skip = 0, Take = 300 }, default)
			.ConfigureAwait(true);
		roles.Should().NotBeNull();
		roles.Items.Should().NotBeNullOrEmpty();

		foreach (var role in roles.Items)
		{
			var refetchedRole = await LogicMonitorClient
				.GetAsync<Role>(role.Id, default)
				.ConfigureAwait(true);
			refetchedRole.Name.Should().Be(role.Name);
		}
	}

	[Fact]
	public async Task GetForCurrentUser()
	{
		var roles = await LogicMonitorClient
			.GetRolesForCurrentUserPageAsync(new Filter<Role> { Skip = 0, Take = 300 }, default)
			.ConfigureAwait(true);
		roles.Should().NotBeNull();
		roles.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing role called "Test"
		var existingRole = (await LogicMonitorClient
				.GetAllAsync(new Filter<Role> { FilterItems = [new Eq<Role>(nameof(Role.Name), Value)] }, default)
				.ConfigureAwait(true))
			.SingleOrDefault();
		if (existingRole is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingRole, cancellationToken: default)
				.ConfigureAwait(true);
		}

		var dashboardGroup = (await LogicMonitorClient.GetAllAsync(new Filter<DashboardGroup> { Take = 1 }, default).ConfigureAwait(true)).SingleOrDefault();
		dashboardGroup ??= new();
		var resourceGroup = (await LogicMonitorClient.GetAllAsync(new Filter<ResourceGroup> { Take = 1 }, default).ConfigureAwait(true)).SingleOrDefault();
		resourceGroup ??= new();
		var websiteGroup = (await LogicMonitorClient.GetAllAsync(new Filter<WebsiteGroup> { Take = 1 }, default).ConfigureAwait(true)).SingleOrDefault();
		websiteGroup ??= new();
		var reportGroup = (await LogicMonitorClient.GetAllAsync(new Filter<ReportGroup> { Take = 1 }, default).ConfigureAwait(true)).SingleOrDefault();
		reportGroup ??= new();
		var role = await LogicMonitorClient.CreateAsync(new RoleCreationDto
		{
			CustomHelpLabel = "",
			CustomHelpUrl = "",
			Description = "Desc",
			Name = Value,
			RequireEULA = false,
			TwoFactorAuthenticationRequired = true,
			Privileges =
				[
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.DashboardGroup,
						ObjectId = dashboardGroup.Id.ToString(CultureInfo.InvariantCulture),
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.ResourceGroup,
						ObjectId = resourceGroup.Id.ToString(CultureInfo.InvariantCulture),
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.WebsiteGroup,
						ObjectId = websiteGroup.Id.ToString(CultureInfo.InvariantCulture),
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.ReportGroup,
						ObjectId = reportGroup.Id.ToString(CultureInfo.InvariantCulture),
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.Help,
						ObjectId = "*",
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.ResourceDashboard,
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.ConfigNeedResourceManagePermission,
						ObjectId = "",
						Operation = RolePrivilegeOperation.Write
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.ReportGroup,
						ObjectId = reportGroup.Id.ToString(CultureInfo.InvariantCulture),
						Operation = RolePrivilegeOperation.Read
					},
				]
		}, default).ConfigureAwait(true);

		// Refetch
		var refetch = await LogicMonitorClient
			.GetAsync<Role>(role.Id, default)
			.ConfigureAwait(true);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient
			.DeleteAsync(role, cancellationToken: default)
			.ConfigureAwait(true);
	}
}
