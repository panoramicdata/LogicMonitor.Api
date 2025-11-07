using LogicMonitor.Api.Alerts;
using System.Management.Automation;

namespace LogicMonitor.PowerShell.Commands
{
	/// <summary>
	/// Gets LogicMonitor alerts
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "LMAlert")]
	[OutputType(typeof(Alert[]))]
	public class GetLMAlertCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Alert ID to retrieve
		/// </summary>
		[Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public string? Id { get; set; }

		/// <summary>
		/// Maximum number of results to return
		/// </summary>
		[Parameter()]
		public int Take { get; set; } = 100;

		/// <summary>
		/// Number of results to skip
		/// </summary>
		[Parameter()]
		public int Skip { get; set; } = 0;

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage("Retrieving LogicMonitor alerts...");

				if (!string.IsNullOrEmpty(Id))
				{
					// Get specific alert by ID
					var alert = Client!.GetAlertAsync(Id, CancellationToken.None)
							.GetAwaiter().GetResult();
					WriteObject(alert);
					return;
				}

				// Build filter for all alerts
				var alertFilter = new AlertFilter
				{
					Take = Take,
					Skip = Skip
				};

				// Get alerts
				var alerts = Client!.GetAlertsAsync(alertFilter, CancellationToken.None)
				   .GetAwaiter().GetResult();

				WriteVerboseMessage($"Retrieved {alerts.Count} alerts.");
				WriteObject(alerts, true);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Acknowledges a LogicMonitor alert
	/// </summary>
	[Cmdlet(VerbsLifecycle.Confirm, "LMAlert")]
	[OutputType(typeof(void))]
	public class ConfirmLMAlertCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Alert ID to acknowledge
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public string Id { get; set; } = string.Empty;

		/// <summary>
		/// Acknowledgment note
		/// </summary>
		[Parameter()]
		public string? Note { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Acknowledging alert ID: {Id}");

				// Acknowledge the alert
				Client!.AcknowledgeAlertAsync(Id, Note ?? "Acknowledged via PowerShell", CancellationToken.None)
				  .GetAwaiter().GetResult();

				WriteVerboseMessage($"Successfully acknowledged alert: {Id}");
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Adds a note to a LogicMonitor alert
	/// </summary>
	[Cmdlet(VerbsCommon.Add, "LMAlertNote")]
	[OutputType(typeof(void))]
	public class AddLMAlertNoteCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Alert ID to add note to
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public string Id { get; set; } = string.Empty;

		/// <summary>
		/// Note text
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		[ValidateNotNullOrEmpty]
		public string Note { get; set; } = string.Empty;

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Adding note to alert ID: {Id}");

				// Add note to the alert
				Client!.SetAlertNoteAsync([Id], Note, CancellationToken.None)
				  .GetAwaiter().GetResult();

				WriteVerboseMessage($"Successfully added note to alert: {Id}");
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}
}