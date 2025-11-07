using System.Management.Automation;

namespace LogicMonitor.PowerShell.Commands
{
	/// <summary>
	/// Disconnects from LogicMonitor
	/// </summary>
	[Cmdlet(VerbsCommunications.Disconnect, "LogicMonitor")]
	public class DisconnectLogicMonitorCommand : LogicMonitorCmdletBase
	{
		protected override void ProcessRecord()
		{
			try
			{
				if (Client == null)
				{
					WriteWarning("Not currently connected to LogicMonitor.");
					return;
				}

				WriteVerboseMessage("Disconnecting from LogicMonitor...");

				// Dispose the client
				Client.Dispose();
				Client = null;
				ConnectionInfo = null;

				WriteVerboseMessage("Successfully disconnected from LogicMonitor.");
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Tests the current connection to LogicMonitor
	/// </summary>
	[Cmdlet(VerbsDiagnostic.Test, "LMConnection")]
	[OutputType(typeof(bool))]
	public class TestLMConnectionCommand : LogicMonitorCmdletBase
	{
		protected override void ProcessRecord()
		{
			try
			{
				if (Client == null)
				{
					WriteObject(false);
					return;
				}

				WriteVerboseMessage("Testing LogicMonitor connection...");

				// Try to get account settings to test connection
				var accountInfo = Client.GetAsync<LogicMonitor.Api.Settings.AccountSettings>(CancellationToken.None)
				  .GetAwaiter().GetResult();

				WriteVerboseMessage("Connection test successful.");
				WriteObject(true);
			}
			catch (Exception ex)
			{
				WriteVerboseMessage($"Connection test failed: {ex.Message}");
				WriteObject(false);
			}
		}
	}
}