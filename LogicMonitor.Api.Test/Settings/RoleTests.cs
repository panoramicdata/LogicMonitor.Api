namespace LogicMonitor.Api.Test.Settings;

public class RoleTests : TestWithOutput
{
	private const string Value = "Unit Test Role";

	public RoleTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetRoles()
	{
		var roles = await LogicMonitorClient.GetPageAsync(new Filter<Role> { Skip = 0, Take = 300 }).ConfigureAwait(false);
		Assert.NotNull(roles);
		Assert.NotNull(roles.Items);
		Assert.True(roles.Items.Count > 0);

		foreach (var role in roles.Items)
		{
			var refetchedRole = await LogicMonitorClient.GetAsync<Role>(role.Id).ConfigureAwait(false);
			Assert.True(refetchedRole.Name == role.Name);
		}
	}

	[Fact]
	public async void GetForCurrentUser()
	{
		var roles = await LogicMonitorClient.GetRolesForCurrentUserPageAsync(new Filter<Role> { Skip = 0, Take = 300 }).ConfigureAwait(false);
		Assert.NotNull(roles);
		Assert.NotNull(roles.Items);
		Assert.True(roles.Items.Count > 0);
	}

	[Fact]
	public async void CreateUpdateDelete()
	{
		// Ensure there is no existing role called "Test"
		var existingRole = (await LogicMonitorClient
				.GetAllAsync(new Filter<Role> { FilterItems = new List<FilterItem<Role>> { new Eq<Role>(nameof(Role.Name), Value) } })
				.ConfigureAwait(false))
			.SingleOrDefault();
		if (existingRole != null)
		{
			await LogicMonitorClient.DeleteAsync(existingRole).ConfigureAwait(false);
		}

		var dashboardGroup = (await LogicMonitorClient.GetAllAsync(new Filter<DashboardGroup> { Take = 1 }).ConfigureAwait(false)).SingleOrDefault();
		var deviceGroup = (await LogicMonitorClient.GetAllAsync(new Filter<DeviceGroup> { Take = 1 }).ConfigureAwait(false)).SingleOrDefault();
		var websiteGroup = (await LogicMonitorClient.GetAllAsync(new Filter<WebsiteGroup> { Take = 1 }).ConfigureAwait(false)).SingleOrDefault();
		var reportGroup = (await LogicMonitorClient.GetAllAsync(new Filter<ReportGroup> { Take = 1 }).ConfigureAwait(false)).SingleOrDefault();
		var role = await LogicMonitorClient.CreateAsync(new RoleCreationDto
		{
			CustomHelpLabel = "",
			CustomHelpUrl = "",
			Description = "Desc",
			Name = Value,
			RequireEULA = false,
			TwoFactorAuthenticationRequired = false,
			Privileges = new List<RolePrivilege>
				{
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.DashboardGroup,
						ObjectId = dashboardGroup.Id.ToString(),
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.DeviceGroup,
						ObjectId = deviceGroup.Id.ToString(),
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.WebsiteGroup,
						ObjectId = websiteGroup.Id.ToString(),
						Operation = RolePrivilegeOperation.Read
					},
					new RolePrivilege
					{
						ObjectType = PrivilegeObjectType.ReportGroup,
						ObjectId = reportGroup.Id.ToString(),
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
						ObjectId = reportGroup.Id.ToString(),
						Operation = RolePrivilegeOperation.Read
					},
				}
		}).ConfigureAwait(false);

		// Refetch
		var refetch = await LogicMonitorClient.GetAsync<Role>(role.Id).ConfigureAwait(false);
		Assert.NotNull(refetch);

		// Delete
		await LogicMonitorClient.DeleteAsync(role).ConfigureAwait(false);
	}
}
