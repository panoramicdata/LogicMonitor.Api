namespace LogicMonitor.Api.Test.PowerShell;

/// <summary>
/// Tests for LogicMonitor connection PowerShell cmdlets
/// </summary>
public class ConnectionCommandsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: PowerShellTestBase(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public void ConnectLogicMonitor_WithValidCredentials_ShouldSucceed()
	{
		// Act
		var results = InvokePowerShell("Connect-LogicMonitor", new Dictionary<string, object>
		{
			{ "Account", Account },
			{ "AccessId", AccessId },
			{ "AccessKey", AccessKey }
		});

		// Assert
		results.Should().NotBeNull();

		// Test connection status
		var connectionTest = InvokePowerShell("Test-LMConnection", []);
		connectionTest.Should().ContainSingle();
		var isConnected = connectionTest.First().BaseObject as bool?;
		isConnected.Should().BeTrue();
	}

	[Fact]
	public void ConnectLogicMonitor_WithInvalidCredentials_ShouldFail()
	{
		// Act & Assert
		var action = () => InvokePowerShell("Connect-LogicMonitor", new Dictionary<string, object>
		{
			{ "Account", "invalid-account" },
			{ "AccessId", "invalid-id" },
			{ "AccessKey", "invalid-key" }
		});

		action.Should().Throw<InvalidOperationException>()
			.WithMessage("*failed*");
	}

	[Fact]
	public void DisconnectLogicMonitor_WhenConnected_ShouldSucceed()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Act
		var results = InvokePowerShell("Disconnect-LogicMonitor", []);

		// Assert
		results.Should().NotBeNull();

		// Test connection status
		var connectionTest = InvokePowerShell("Test-LMConnection", []);
		connectionTest.Should().ContainSingle();
		var isConnected = connectionTest.First().BaseObject as bool?;
		isConnected.Should().BeFalse();
	}

	[Fact]
	public void TestLMConnection_WhenNotConnected_ShouldReturnFalse()
	{
		// Act
		var results = InvokePowerShell("Test-LMConnection", []);

		// Assert
		results.Should().ContainSingle();
		var isConnected = results.First().BaseObject as bool?;
		isConnected.Should().BeFalse();
	}
}