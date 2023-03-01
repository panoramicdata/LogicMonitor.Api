namespace LogicMonitor.Api.Test.Settings;

public class UserTests : TestWithOutput
{
	public UserTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetUsers()
	{
		var users = await LogicMonitorClient.GetPageAsync(new Filter<User> { Skip = 0, Take = 300 }, default).ConfigureAwait(false);
		users.Should().NotBeNull();
		users.Items.Should().NotBeNullOrEmpty();

		foreach (var user in users.Items)
		{
			var refetchedUser = await LogicMonitorClient.GetAsync<User>(user.Id, default).ConfigureAwait(false);
			var roles = refetchedUser.Roles;
			roles.Should().NotBeNullOrEmpty();
			user.UserGroupIds.Should().NotBeNullOrEmpty();
		}
	}

	[Fact]
	public async Task CreateUser()
	{
		// Delete it if it already exists
		foreach (var existingUser in await LogicMonitorClient.GetAllAsync(new Filter<User>
		{
			FilterItems = new List<FilterItem<User>>
			{
				new Eq<User>(nameof(User.UserName), "test")
			}
		}, default).ConfigureAwait(false))
		{
			await LogicMonitorClient.DeleteAsync(existingUser, cancellationToken: default).ConfigureAwait(false);
		}

		var userCreationDto = new UserCreationDto
		{
			ViewPermission = new ViewPermissionUser
			{
				Alerts = true,
				Dashboards = true,
				Devices = true,
				Reports = true,
				Websites = true,
				Settings = true
			},
			Username = "test",
			FirstName = "first",
			LastName = "last",
			Email = "david@davidbond.net",
			Password = "Abc123!!!",
			Password1 = "Abc123!!!",
			ForcePasswordChange = true,
			TwoFAEnabled = false,
			SmsEmail = "",
			SmsEmailFormat = "sms",
			Timezone = "",
			ViewPermissions = new List<bool> { true, true, true, true, true, true },
			Status = "active",
			Note = "note",
			Roles = new List<Role>
				{
					new Role { Id = 1 }
				},
			ApiTokens = new List<object>(),
			Phone = "+447761503941",
			ApiOnly = false
		};

		var user = await LogicMonitorClient.CreateAsync(userCreationDto, default).ConfigureAwait(false);

		// Delete again
		await LogicMonitorClient.DeleteAsync(user, cancellationToken: default).ConfigureAwait(false);
	}
}
