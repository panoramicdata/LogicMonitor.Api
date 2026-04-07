namespace LogicMonitor.Api.Test.PowerShell;

/// <summary>
/// Tests for LogicMonitor resource PowerShell cmdlets
/// </summary>
public class ResourceCommandsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: PowerShellTestBase(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public void GetLMResource_WithoutConnection_ShouldFail()
	{
		// Ensure disconnected state by explicitly disconnecting
		DisconnectFromLogicMonitor();

		// Act & Assert - when not connected, the cmdlet should throw
		var action = () => InvokePowerShell("Get-LMResource", []);

		action.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void GetLMResource_WithConnection_ShouldReturnResources()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Act
		var results = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Take", 5 }
		});

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCountLessThanOrEqualTo(5);

		if (results.Count > 0)
		{
			var resource = results.First().BaseObject;
			resource.Should().NotBeNull();

			// Check that it has expected properties
			var resourceProperties = resource.GetType().GetProperties();
			resourceProperties.Should().Contain(p => p.Name == "Id");
			resourceProperties.Should().Contain(p => p.Name == "Name" || p.Name == "DisplayName");
		}
	}

	[Fact]
	public void GetLMResource_WithSpecificId_ShouldReturnSingleResource()
	{
		// Arrange
		ConnectToLogicMonitor();

		// First get a list to find a valid ID
		var allResources = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		allResources.Should().ContainSingle();
		dynamic? firstResource = allResources.First().BaseObject;
		var resourceId = (int)firstResource!.Id;

		// Act
		var results = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Id", resourceId }
		});

		// Assert
		results.Should().ContainSingle();
		dynamic? resource = results.First().BaseObject;
		((int)resource!.Id).Should().Be(resourceId);
	}

	[Fact]
	public void GetLMResourceGroup_WithConnection_ShouldReturnResourceGroups()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Act
		var results = InvokePowerShell("Get-LMResourceGroup", new Dictionary<string, object>
		{
			{ "Take", 5 }
		});

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCountLessThanOrEqualTo(5);

		if (results.Count > 0)
		{
			var resourceGroup = results.First().BaseObject;
			resourceGroup.Should().NotBeNull();

			// Check that it has expected properties
			var properties = resourceGroup.GetType().GetProperties();
			properties.Should().Contain(p => p.Name == "Id");
			properties.Should().Contain(p => p.Name == "Name");
		}
	}

	[Fact]
	public void NewLMResourceGroup_AndRemove_ShouldWorkEndToEnd()
	{
		// Arrange
		ConnectToLogicMonitor();
		var testGroupName = $"PowerShell-Test-Group-{DateTime.Now:yyyyMMdd-HHmmss}";

		try
		{
			// Act - Create
			var createResults = InvokePowerShell("New-LMResourceGroup", new Dictionary<string, object>
			{
				{ "Name", testGroupName },
				{ "Description", "Test group created by PowerShell tests" }
			});

			// Assert - Create
			createResults.Should().ContainSingle();
			var createdGroup = createResults.First().BaseObject;
			createdGroup.Should().NotBeNull();
			createdGroup.Should().BeOfType<Api.Resources.ResourceGroup>();
			var typedGroup = (Api.Resources.ResourceGroup)createdGroup;
			typedGroup.Name.Should().Be(testGroupName);
			var groupId = typedGroup.Id;

			// Act - Verify it exists
			var getResults = InvokePowerShell("Get-LMResourceGroup", new Dictionary<string, object>
			{
				{ "Id", groupId }
			});

			// Assert - Verify
			getResults.Should().ContainSingle();
			var retrievedGroup = (Api.Resources.ResourceGroup)getResults.First().BaseObject;
			retrievedGroup.Id.Should().Be(groupId);

			// Act - Delete
			var removeResults = InvokePowerShell("Remove-LMResourceGroup", new Dictionary<string, object>
			{
				{ "Id", groupId },
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
				var cleanupResults = InvokePowerShell("Get-LMResourceGroup", new Dictionary<string, object>
				{
					{ "Name", testGroupName }
				});

				if (cleanupResults.Count > 0)
				{
					var groupToCleanup = (Api.Resources.ResourceGroup)cleanupResults.First().BaseObject;
					var groupId = groupToCleanup.Id;

					InvokePowerShell("Remove-LMResourceGroup", new Dictionary<string, object>
					{
						{ "Id", groupId },
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