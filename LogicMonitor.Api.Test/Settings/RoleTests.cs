using System.Globalization;

namespace LogicMonitor.Api.Test.Settings;

public class RoleTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string Value = "Unit Test Role";

	[Fact]
	public async Task GetRoles()
	{
		var roles = await LogicMonitorClient
			.GetPageAsync(new Filter<Role> { Skip = 0, Take = 300 }, CancellationToken);
		roles.Should().NotBeNull();
		roles.Items.Should().NotBeNullOrEmpty();

		foreach (var role in roles.Items)
		{
			var refetchedRole = await LogicMonitorClient
				.GetAsync<Role>(role.Id, CancellationToken);
			refetchedRole.Name.Should().Be(role.Name);
		}
	}

	[Fact]
	public async Task GetForCurrentUser()
	{
		var roles = await LogicMonitorClient
			.GetRolesForCurrentUserPageAsync(new Filter<Role> { Skip = 0, Take = 300 }, CancellationToken);
		roles.Should().NotBeNull();
		roles.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing role called "Test"
		var existingRole = (await LogicMonitorClient
				.GetAllAsync(new Filter<Role> { FilterItems = [new Eq<Role>(nameof(Role.Name), Value)] }, CancellationToken)
				)
			.SingleOrDefault();
		if (existingRole is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingRole, CancellationToken)
				;
		}

		var dashboardGroup = (await LogicMonitorClient.GetAllAsync(new Filter<DashboardGroup> { Take = 1 }, CancellationToken)).SingleOrDefault();
		dashboardGroup ??= new();
		var resourceGroup = (await LogicMonitorClient.GetAllAsync(new Filter<ResourceGroup> { Take = 1 }, CancellationToken)).SingleOrDefault();
		resourceGroup ??= new();
		var websiteGroup = (await LogicMonitorClient.GetAllAsync(new Filter<WebsiteGroup> { Take = 1 }, CancellationToken)).SingleOrDefault();
		websiteGroup ??= new();
		var reportGroup = (await LogicMonitorClient.GetAllAsync(new Filter<ReportGroup> { Take = 1 }, CancellationToken)).SingleOrDefault();
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
		}, CancellationToken);

		// Refetch
		var refetch = await LogicMonitorClient
			.GetAsync<Role>(role.Id, CancellationToken);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient
			.DeleteAsync(role, CancellationToken)
			;
	}
}
