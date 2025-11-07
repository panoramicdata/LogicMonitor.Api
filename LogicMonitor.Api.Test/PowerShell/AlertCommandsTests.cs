using System.Management.Automation;

namespace LogicMonitor.Api.Test.PowerShell;

/// <summary>
/// Tests for LogicMonitor alert PowerShell cmdlets
/// </summary>
public class AlertCommandsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: PowerShellTestBase(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public void GetLMAlert_WithConnection_ShouldReturnAlerts()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Act
		var results = InvokePowerShell("Get-LMAlert", new Dictionary<string, object>
		{
			{ "Take", 5 }
		});

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCountLessThanOrEqualTo(5);

		if (results.Count > 0)
		{
			var alert = results.First().BaseObject;
			alert.Should().NotBeNull();

			// Check that it has expected properties
			var properties = alert.GetType().GetProperties();
			properties.Should().Contain(p => p.Name == "Id");
			properties.Should().Contain(p => p.Name == "MonitorObjectName");
		}
	}

	[Fact]
	public void GetLMAlert_WithSpecificId_ShouldReturnSingleAlert()
	{
		// Arrange
		ConnectToLogicMonitor();

		// First get a list to find a valid ID
		var allAlerts = InvokePowerShell("Get-LMAlert", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		if (allAlerts.Count == 0)
		{
			// Skip test if no alerts are available
			return;
		}

		dynamic? firstAlert = allAlerts.First().BaseObject;
		var alertId = (string)firstAlert!.Id;

		// Act
		var results = InvokePowerShell("Get-LMAlert", new Dictionary<string, object>
		{
			{ "Id", alertId }
		});

		// Assert
		results.Should().ContainSingle();
		dynamic? alert = results.First().BaseObject;
		((string)alert!.Id).Should().Be(alertId);
	}

	[Fact]
	public void ConfirmLMAlert_WithValidAlert_ShouldSucceed()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Get an active alert to acknowledge
		var activeAlerts = InvokePowerShell("Get-LMAlert", new Dictionary<string, object>
		{
			{ "Take", 10 }
		});

		// Find an unacknowledged alert
		PSObject? unackedAlert = null;
		foreach (var alertObj in activeAlerts)
		{
			dynamic? alert = alertObj.BaseObject;
			if (alert != null && alert?.Acked == false)
			{
				unackedAlert = alertObj;
				break;
			}
		}

		if (unackedAlert == null)
		{
			// Skip test if no unacknowledged alerts are available
			return;
		}

		dynamic? alertToAck = unackedAlert.BaseObject;
		var alertId = (string)alertToAck!.Id;

		// Act
		var results = InvokePowerShell("Confirm-LMAlert", new Dictionary<string, object>
		{
			{ "Id", alertId },
			{ "Note", "Acknowledged by PowerShell test" }
		});

		// Assert
		results.Should().NotBeNull();
		// The command should complete without errors
	}

	[Fact]
	public void AddLMAlertNote_WithValidAlert_ShouldSucceed()
	{
		// Arrange
		ConnectToLogicMonitor();

		// Get any alert to add a note to
		var alerts = InvokePowerShell("Get-LMAlert", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		if (alerts.Count == 0)
		{
			// Skip test if no alerts are available
			return;
		}

		dynamic? alert = alerts.First().BaseObject;
		var alertId = (string)alert!.Id;

		// Act
		var results = InvokePowerShell("Add-LMAlertNote", new Dictionary<string, object>
		{
			{ "Id", alertId },
			{ "Note", $"Test note added by PowerShell at {DateTime.Now}" }
		});

		// Assert
		results.Should().NotBeNull();
		// The command should complete without errors
	}

	[Fact]
	public void GetLMAlert_WithInvalidId_ShouldFail()
	{
		// Arrange
		ConnectToLogicMonitor();
		var invalidAlertId = "INVALID_ALERT_ID_12345";

		// Act & Assert
		var action = () => InvokePowerShell("Get-LMAlert", new Dictionary<string, object>
		{
			{ "Id", invalidAlertId }
		});

		action.Should().Throw<InvalidOperationException>()
			.WithMessage("*failed*");
	}

	[Fact]
	public void ConfirmLMAlert_WithInvalidId_ShouldFail()
	{
		// Arrange
		ConnectToLogicMonitor();
		var invalidAlertId = "INVALID_ALERT_ID_12345";

		// Act & Assert
		var action = () => InvokePowerShell("Confirm-LMAlert", new Dictionary<string, object>
		{
			{ "Id", invalidAlertId },
			{ "Note", "Test note" }
		});

		action.Should().Throw<InvalidOperationException>()
			.WithMessage("*failed*");
	}
}