namespace LogicMonitor.Api.Test.PowerShell;

/// <summary>
/// Tests for LogicMonitor property PowerShell cmdlets
/// </summary>
public class PropertyCommandsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: PowerShellTestBase(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public void GetLMResourceProperty_WithValidResource_ShouldReturnProperties()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Get a valid resource ID first
		var resources = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		resources.Should().ContainSingle();
		dynamic? resource = resources.First().BaseObject;
		var resourceId = (int)resource!.Id;

		// Act
		var results = InvokePowerShell("Get-LMResourceProperty", new Dictionary<string, object>
		{
			{ "ResourceId", resourceId }
		});

		// Assert
		results.Should().NotBeNull();

		if (results.Count > 0)
		{
			var property = results.First().BaseObject;
			property.Should().NotBeNull();

			// Check that it has expected properties
			var properties = property.GetType().GetProperties();
			properties.Should().Contain(p => p.Name == "Name");
			properties.Should().Contain(p => p.Name == "Value");
		}
	}

	[Fact]
	public void SetLMResourceProperty_AndGet_ShouldWorkEndToEnd()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Get a valid resource ID first
		var resources = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		resources.Should().ContainSingle();
		dynamic? resource = resources.First().BaseObject;
		var resourceId = (int)resource!.Id;

		var testPropertyName = $"PowerShell.Test.{DateTime.Now:yyyyMMddHHmmss}";
		var testPropertyValue = "PowerShell Test Value";

		try
		{
			// Act - Set property
			var setResults = InvokePowerShell("Set-LMResourceProperty", new Dictionary<string, object>
			{
				{ "ResourceId", resourceId },
				{ "PropertyName", testPropertyName },
				{ "PropertyValue", testPropertyValue },
				{ "CreateIfNotExists", true }
			});

			// Assert - Set
			setResults.Should().ContainSingle();
			dynamic? setProperty = setResults.First().BaseObject;
			((string)setProperty!.Name).Should().Be(testPropertyName);
			((string)setProperty.Value).Should().Be(testPropertyValue);

			// Act - Get specific property
			var getResults = InvokePowerShell("Get-LMResourceProperty", new Dictionary<string, object>
			{
				{ "ResourceId", resourceId },
				{ "PropertyName", testPropertyName }
			});

			// Assert - Get
			getResults.Should().ContainSingle();
			dynamic? getProperty = getResults.First().BaseObject;
			((string)getProperty!.Name).Should().Be(testPropertyName);
			((string)getProperty.Value).Should().Be(testPropertyValue);

			// Act - Remove property
			var removeResults = InvokePowerShell("Remove-LMResourceProperty", new Dictionary<string, object>
			{
				{ "ResourceId", resourceId },
				{ "PropertyName", testPropertyName },
				{ "Confirm", false }
			});

			// Assert - Remove should succeed
			removeResults.Should().NotBeNull();
		}
		catch
		{
			// Cleanup in case of failure
			try
			{
				InvokePowerShell("Remove-LMResourceProperty", new Dictionary<string, object>
				{
					{ "ResourceId", resourceId },
					{ "PropertyName", testPropertyName },
					{ "Confirm", false }
				});
			}
			catch
			{
				// Ignore cleanup errors
			}

			throw;
		}
	}

	[Fact]
	public void GetLMResourceProperty_WithSpecificPropertyName_ShouldReturnSingleProperty()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Get a valid resource ID first
		var resources = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		resources.Should().ContainSingle();
		dynamic? resource = resources.First().BaseObject;
		var resourceId = (int)resource!.Id;

		// Get all properties to find a valid property name
		var allProperties = InvokePowerShell("Get-LMResourceProperty", new Dictionary<string, object>
		{
			{ "ResourceId", resourceId }
		});

		allProperties.Should().HaveCountGreaterThan(0);
		dynamic? firstProperty = allProperties.First().BaseObject;
		var propertyName = (string)firstProperty!.Name;

		// Act
		var results = InvokePowerShell("Get-LMResourceProperty", new Dictionary<string, object>
		{
			{ "ResourceId", resourceId },
			{ "PropertyName", propertyName }
		});

		// Assert
		results.Should().ContainSingle();
		dynamic? property = results.First().BaseObject;
		((string)property!.Name).Should().Be(propertyName);
	}

	[Fact]
	public void RemoveLMResourceProperty_WithNonExistentProperty_ShouldFail()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Get a valid resource ID first
		var resources = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		resources.Should().ContainSingle();
		dynamic? resource = resources.First().BaseObject;
		var resourceId = (int)resource!.Id;

		var nonExistentPropertyName = $"NonExistent.Property.{DateTime.Now:yyyyMMddHHmmss}";

		// Act & Assert
		var action = () => InvokePowerShell("Remove-LMResourceProperty", new Dictionary<string, object>
		{
			{ "ResourceId", resourceId },
			{ "PropertyName", nonExistentPropertyName },
			{ "Confirm", false }
		});

		action.Should().Throw<InvalidOperationException>()
			.WithMessage("*failed*");
	}
}