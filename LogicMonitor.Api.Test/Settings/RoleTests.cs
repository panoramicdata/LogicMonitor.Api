using System.Globalization;

namespace LogicMonitor.Api.Test.Settings;

public class RoleTests : TestWithOutput
{
	private const string Value = "Unit Test Role";

	public RoleTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetRoles()
	{
		var roles = await LogicMonitorClient.GetPageAsync(new Filter<Role> { Skip = 0, Take = 300 }, default).ConfigureAwait(false);
		roles.Should().NotBeNull();
		roles.Items.Should().NotBeNullOrEmpty();

		foreach (var role in roles.Items)
		{
			var refetchedRole = await LogicMonitorClient.GetAsync<Role>(role.Id, default).ConfigureAwait(false);
			refetchedRole.Name.Should().Be(role.Name);
		}
	}

	[Fact]
	public async Task GetForCurrentUser()
	{
		var roles = await LogicMonitorClient.GetRolesForCurrentUserPageAsync(new Filter<Role> { Skip = 0, Take = 300 }, default).ConfigureAwait(false);
		roles.Should().NotBeNull();
		roles.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task CreateUpdateDelete()
	{
		// Ensure there is no existing role called "Test"
		var existingRole = (await LogicMonitorClient
				.GetAllAsync(new Filter<Role> { FilterItems = [new Eq<Role>(nameof(Role.Name), Value)] }, default)
				.ConfigureAwait(false))
			.SingleOrDefault();
		if (existingRole is not null)
		{
			await LogicMonitorClient.DeleteAsync(existingRole, cancellationToken: default).ConfigureAwait(false);
		}

		var dashboardGroup = (await LogicMonitorClient.GetAllAsync(new Filter<DashboardGroup> { Take = 1 }, default).ConfigureAwait(false)).SingleOrDefault();
		dashboardGroup ??= new();
		var deviceGroup = (await LogicMonitorClient.GetAllAsync(new Filter<DeviceGroup> { Take = 1 }, default).ConfigureAwait(false)).SingleOrDefault();
		deviceGroup ??= new();
		var websiteGroup = (await LogicMonitorClient.GetAllAsync(new Filter<WebsiteGroup> { Take = 1 }, default).ConfigureAwait(false)).SingleOrDefault();
		websiteGroup ??= new();
		var reportGroup = (await LogicMonitorClient.GetAllAsync(new Filter<ReportGroup> { Take = 1 }, default).ConfigureAwait(false)).SingleOrDefault();
		reportGroup ??= new();
		var role = await LogicMonitorClient.CreateAsync(new RoleCreationDto
		{
			CustomHelpLabel = "",
			CustomHelpUrl = "",
			Description = "Desc",
			Name = Value,
			RequireEULA = false,
			TwoFactorAuthenticationRequired = false,
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
						ObjectType = PrivilegeObjectType.DeviceGroup,
						ObjectId = deviceGroup.Id.ToString(CultureInfo.InvariantCulture),
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
						ObjectType = PrivilegeObjectType.DeviceDashboard,
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.ConfigNeedDeviceManagePermission,
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
		}, default).ConfigureAwait(false);

		// Refetch
		var refetch = await LogicMonitorClient.GetAsync<Role>(role.Id, default).ConfigureAwait(false);
		refetch.Should().NotBeNull();

		// Delete
		await LogicMonitorClient.DeleteAsync(role, cancellationToken: default).ConfigureAwait(false);
	}
}
