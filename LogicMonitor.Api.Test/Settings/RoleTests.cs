using LogicMonitor.Api.Dashboards;
using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Reports;
using LogicMonitor.Api.Users;
using LogicMonitor.Api.Websites;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class RoleTests : TestWithOutput
	{
		public RoleTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetRoles()
		{
			var roles = await PortalClient.GetPageAsync(new Filter<Role> { Skip = 0, Take = 300 }).ConfigureAwait(false);
			Assert.NotNull(roles);
			Assert.NotNull(roles.Items);
			Assert.True(roles.Items.Count > 0);

			foreach (var role in roles.Items)
			{
				var refetchedRole = await PortalClient.GetAsync<Role>(role.Id).ConfigureAwait(false);
				Assert.True(refetchedRole.Name == role.Name);
			}
		}

		[Fact]
		public async void GetForCurrentUser()
		{
			var roles = await PortalClient.GetRolesForCurrentUserPageAsync(new Filter<Role> { Skip = 0, Take = 300 }).ConfigureAwait(false);
			Assert.NotNull(roles);
			Assert.NotNull(roles.Items);
			Assert.True(roles.Items.Count > 0);
		}

		[Fact]
		public async void CreateUpdateDelete()
		{
			var dashboardGroup = (await PortalClient.GetAllAsync(new Filter<DashboardGroup> { Take = 1 }).ConfigureAwait(false)).SingleOrDefault();
			var deviceGroup = (await PortalClient.GetAllAsync(new Filter<DeviceGroup> { Take = 1 }).ConfigureAwait(false)).SingleOrDefault();
			var websiteGroup = (await PortalClient.GetAllAsync(new Filter<WebsiteGroup> { Take = 1 }).ConfigureAwait(false)).SingleOrDefault();
			var reportGroup = (await PortalClient.GetAllAsync(new Filter<ReportGroup> { Take = 1 }).ConfigureAwait(false)).SingleOrDefault();
			var role = await PortalClient.CreateAsync(new RoleCreationDto
			{
				CustomHelpLabel = "",
				CustomHelpUrl = "",
				Description = "Desc",
				Name = "Test",
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
			var refetch = await PortalClient.GetAsync<Role>(role.Id).ConfigureAwait(false);
			Assert.NotNull(refetch);

			// Delete
			await PortalClient.DeleteAsync(role).ConfigureAwait(false);
		}
	}
}