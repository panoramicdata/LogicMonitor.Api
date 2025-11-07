namespace LogicMonitor.Api.Test.Settings;

public class UserTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetUsers()
	{
		var users = await LogicMonitorClient
			.GetPageAsync(new Filter<User> { Skip = 0, Take = 300 }, CancellationToken);
		users.Should().NotBeNull();
		users.Items.Should().NotBeNullOrEmpty();

		foreach (var user in users.Items)
		{
			var refetchedUser = await LogicMonitorClient
				.GetAsync<User>(user.Id, CancellationToken);
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
			FilterItems =
			[
				new Eq<User>(nameof(User.UserName), "test")
			]
		}, CancellationToken))
		{
			await LogicMonitorClient
				.DeleteAsync(existingUser, CancellationToken);
		}

		var userCreationDto = new UserCreationDto
		{
			ViewPermission = new ViewPermission
			{
				Alerts = true,
				Dashboards = true,
				Resources = true,
				Reports = true,
				Websites = true,
				Settings = true
			},
			Username = "test",
			FirstName = "first",
			LastName = "last",
			Email = "david@davidbond.net",
			Password = "0snAP9GQIMGUG68%",
			Password1 = "0snAP9GQIMGUG68%",
			ForcePasswordChange = true,
			TwoFAEnabled = false,
			SmsEmail = "",
			SmsEmailFormat = "sms",
			Timezone = "",
			ViewPermissions = [true, true, true, true, true, true],
			Status = "active",
			Note = "note",
			Roles =
				[
					new Role { Id = 1 }
				],
			ApiTokens = [],
			Phone = "+447761503941",
			ApiOnly = false
		};

		var user = await LogicMonitorClient
			.CreateAsync(userCreationDto, CancellationToken);

		// Delete again
		await LogicMonitorClient
			.DeleteAsync(user, CancellationToken);
	}

	[Fact]
	public async Task GetAdmins()
	{
		var users = await LogicMonitorClient
			.GetAllAsync<User>(CancellationToken);

		users.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetApiTokens()
	{
		var users = await LogicMonitorClient
			.GetAllAsync<User>(CancellationToken);

		var tokens = await LogicMonitorClient
			.GetAllAsync(new Filter<ApiToken>
			{
				FilterItems = [new Eq<ApiToken>(nameof(ApiToken.UserId), users.First().Id)]
			}, CancellationToken);

		tokens.Should().NotBeEmpty();

		var allTokens = await LogicMonitorClient
			.GetApiTokenListAsync(new Filter<ApiToken>(), CancellationToken);
		allTokens.Items.Should().NotBeEmpty();
	}
}
