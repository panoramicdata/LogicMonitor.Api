using System.Management.Automation;
using LogicMonitor.Api;

namespace LogicMonitor.PowerShell.Commands
{
	/// <summary>
	/// Base class for all LogicMonitor PowerShell cmdlets
	/// </summary>
	public abstract class LogicMonitorCmdletBase : PSCmdlet
	{
		/// <summary>
		/// The LogicMonitor client instance
		/// </summary>
		protected static LogicMonitorClient? Client { get; set; }

		/// <summary>
		/// The connection information
		/// </summary>
		protected static LogicMonitorConnectionInfo? ConnectionInfo { get; set; }

		/// <summary>
		/// Ensures that a connection to LogicMonitor exists
		/// </summary>
		protected void EnsureConnection()
		{
			if (Client == null)
			{
				ThrowTerminatingError(new ErrorRecord(
					new InvalidOperationException("Not connected to LogicMonitor. Please run Connect-LogicMonitor first."),
					"NotConnected",
					ErrorCategory.ConnectionError,
					null));
			}
		}

		/// <summary>
		/// Writes a verbose message if VerbosePreference allows it
		/// </summary>
		/// <param name="message">The message to write</param>
		protected void WriteVerboseMessage(string message) => WriteVerbose($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");

		/// <summary>
		/// Writes a debug message if DebugPreference allows it
		/// </summary>
		/// <param name="message">The message to write</param>
		protected void WriteDebugMessage(string message) => WriteDebug($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");

		/// <summary>
		/// Handles common API exceptions and converts them to PowerShell errors
		/// </summary>
		/// <param name="ex">The exception to handle</param>
		/// <param name="target">The target object that caused the error</param>
		protected void HandleApiException(Exception ex, object? target = null)
		{
			var errorCategory = ex switch
			{
				LogicMonitorApiException => ErrorCategory.InvalidOperation,
				ArgumentException => ErrorCategory.InvalidArgument,
				UnauthorizedAccessException => ErrorCategory.PermissionDenied,
				TimeoutException => ErrorCategory.OperationTimeout,
				_ => ErrorCategory.NotSpecified
			};

			WriteError(new ErrorRecord(
				ex,
				ex.GetType().Name,
				errorCategory,
				target));
		}
	}

	/// <summary>
	/// Connection information for LogicMonitor
	/// </summary>
	public class LogicMonitorConnectionInfo
	{
		public string Account { get; set; } = string.Empty;
		public string AccessId { get; set; } = string.Empty;
		public DateTime ConnectedAt { get; set; }
		public bool IsConnected { get; set; }
	}
}