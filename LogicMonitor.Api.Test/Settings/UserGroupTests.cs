using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Users;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class UserGroupTests : TestWithOutput
	{
		private const string Value = "Unit Test User Group";

		public UserGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetUserGroups()
		{
			var userGroups = await PortalClient.GetAllAsync<UserGroup>().ConfigureAwait(false);
			Assert.NotNull(userGroups);
			Assert.NotEmpty(userGroups);

			foreach (var user in userGroups)
			{
				var refetchedUser = await PortalClient.GetAsync<UserGroup>(user.Id).ConfigureAwait(false);
				Assert.True(refetchedUser.Name == user.Name);
			}
		}

		[Fact]
		public async void CreateUpdateDelete()
		{
			// Ensure there is no existing UserGroup called "Test"
			var existingUserGroup = (await PortalClient
					.GetAllAsync(new Filter<UserGroup> { FilterItems = new List<FilterItem<UserGroup>> { new Eq<UserGroup>(nameof(UserGroup.Name), Value) } })
					.ConfigureAwait(false))
				.SingleOrDefault();
			if (existingUserGroup != null)
			{
				await PortalClient.DeleteAsync(existingUserGroup).ConfigureAwait(false);
			}

			var userGroup = await PortalClient.CreateAsync(new UserGroupCreationDto
			{
				Name = Value,
				Description = "Desc",
			}).ConfigureAwait(false);

			// Refetch
			var refetch = await PortalClient.GetAsync<UserGroup>(userGroup.Id).ConfigureAwait(false);
			Assert.NotNull(refetch);

			// Delete
			await PortalClient.DeleteAsync(userGroup).ConfigureAwait(false);
		}
	}
}