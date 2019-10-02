using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Users;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class RoleGroupTests : TestWithOutput
	{
		private const string Value = "Unit Test Role Group";

		public RoleGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetRoleGroups()
		{
			var roleGroups = await PortalClient.GetAllAsync<RoleGroup>().ConfigureAwait(false);
			Assert.NotNull(roleGroups);
			Assert.NotEmpty(roleGroups);

			foreach (var role in roleGroups)
			{
				var refetchedRole = await PortalClient.GetAsync<RoleGroup>(role.Id).ConfigureAwait(false);
				Assert.True(refetchedRole.Name == role.Name);
			}
		}

		[Fact]
		public async void CreateUpdateDelete()
		{
			// Ensure there is no existing RoleGroup called "Test"
			var existingRoleGroup = (await PortalClient
					.GetAllAsync(new Filter<RoleGroup> { FilterItems = new List<FilterItem<RoleGroup>> { new Eq<RoleGroup>(nameof(RoleGroup.Name), Value) } })
					.ConfigureAwait(false))
				.SingleOrDefault();
			if (existingRoleGroup != null)
			{
				await PortalClient.DeleteAsync(existingRoleGroup).ConfigureAwait(false);
			}

			var roleGroup = await PortalClient.CreateAsync(new RoleGroupCreationDto
			{
				Name = Value,
				Description = "Desc",
			}).ConfigureAwait(false);

			// Refetch
			var refetch = await PortalClient.GetAsync<RoleGroup>(roleGroup.Id).ConfigureAwait(false);
			Assert.NotNull(refetch);

			// Delete
			await PortalClient.DeleteAsync(roleGroup).ConfigureAwait(false);
		}
	}
}