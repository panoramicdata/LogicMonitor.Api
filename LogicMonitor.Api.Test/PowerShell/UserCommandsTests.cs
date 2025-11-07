namespace LogicMonitor.Api.Test.PowerShell;

/// <summary>
/// Tests for LogicMonitor user PowerShell cmdlets
/// </summary>
public class UserCommandsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: PowerShellTestBase(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public void GetLMUser_WithConnection_ShouldReturnUsers()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Act
		var results = InvokePowerShell("Get-LMUser", new Dictionary<string, object>
		{
			{ "Take", 5 }
		});

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCountLessThanOrEqualTo(5);

		if (results.Count > 0)
		{
			var user = results.First().BaseObject;
			user.Should().NotBeNull();

			// Check that it has expected properties
			var properties = user.GetType().GetProperties();
			properties.Should().Contain(p => p.Name == "Id");
			properties.Should().Contain(p => p.Name == "UserName");
			properties.Should().Contain(p => p.Name == "Email");
		}
	}

	[Fact]
	public void GetLMUser_WithSpecificId_ShouldReturnSingleUser()
	{
		// Arrange
		ConnectToLogicMonitor();

		// First get a list to find a valid ID
		var allUsers = InvokePowerShell("Get-LMUser", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		allUsers.Should().ContainSingle();
		dynamic? firstUser = allUsers.First().BaseObject;
		var userId = (int)firstUser!.Id;

		// Act
		var results = InvokePowerShell("Get-LMUser", new Dictionary<string, object>
		{
			{ "Id", userId }
		});

		// Assert
		results.Should().ContainSingle();
		dynamic? user = results.First().BaseObject;
		((int)user!.Id).Should().Be(userId);
	}

	[Fact]
	public void GetLMUser_WithUsernameFilter_ShouldReturnFilteredResults()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Get all users to find a valid username
		var allUsers = InvokePowerShell("Get-LMUser", new Dictionary<string, object>
		{
			{ "Take", 10 }
		});

		allUsers.Should().HaveCountGreaterThan(0);
		dynamic? firstUser = allUsers.First().BaseObject;
		var username = (string)firstUser!.UserName;

		// Act
		var results = InvokePowerShell("Get-LMUser", new Dictionary<string, object>
		{
			{ "Username", username }
		});

		// Assert
		results.Should().ContainSingle();
		dynamic? user = results.First().BaseObject;
		((string)user!.UserName).Should().Be(username);
	}

	[Fact]
	public void NewLMUser_AndRemove_ShouldWorkEndToEnd()
	{
		// Arrange
		ConnectToLogicMonitor();
		var testUsername = $"pstest-{DateTime.Now:yyyyMMddHHmmss}";
		var testEmail = $"{testUsername}@example.com";

		try
		{
			// Act - Create
			var createResults = InvokePowerShell("New-LMUser", new Dictionary<string, object>
			{
				{ "Username", testUsername },
				{ "FirstName", "PowerShell" },
				{ "LastName", "Test" },
				{ "Email", testEmail },
				{ "Password", "TempPassword123!" },
				{ "ForcePasswordChange", true }
			});

			// Assert - Create
			createResults.Should().ContainSingle();
			dynamic? createdUser = createResults.First().BaseObject;
			createdUser.Should().NotBeNull();
			((string)createdUser!.UserName).Should().Be(testUsername);
			((string)createdUser.Email).Should().Be(testEmail);
			var userId = (int)createdUser.Id;

			// Act - Update
			var updateResults = InvokePowerShell("Set-LMUser", new Dictionary<string, object>
			{
				{ "Id", userId },
				{ "FirstName", "UpdatedPowerShell" }
			});

			// Assert - Update
			updateResults.Should().ContainSingle();
			dynamic? updatedUser = updateResults.First().BaseObject;
			((string)updatedUser!.FirstName).Should().Be("UpdatedPowerShell");

			// Act - Delete
			var removeResults = InvokePowerShell("Remove-LMUser", new Dictionary<string, object>
			{
				{ "Id", userId },
				{ "Confirm", false }
			});

			// Assert - Delete should succeed without error
			removeResults.Should().NotBeNull();
		}
		catch
		{
			// Cleanup in case of failure
			try
			{
				var cleanupResults = InvokePowerShell("Get-LMUser", new Dictionary<string, object>
				{
					{ "Username", testUsername }
				});

				if (cleanupResults.Count > 0)
				{
					dynamic? userToCleanup = cleanupResults.First().BaseObject;
					var userId = (int)userToCleanup!.Id;

					InvokePowerShell("Remove-LMUser", new Dictionary<string, object>
					{
						{ "Id", userId },
						{ "Confirm", false }
					});
				}
			}
			catch
			{
				// Ignore cleanup errors
			}

			throw;
		}
	}
}