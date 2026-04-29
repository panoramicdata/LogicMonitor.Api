namespace LogicMonitor.Api;

/// <summary>
/// DiagnosticSource operations.
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	/// Triggers a diagnostic source execution manually for a host.
	/// </summary>
	/// <param name="execution">Execution request payload.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Execution details returned by the API.</returns>
	public Task<DiagnosticSourceExecution> ExecuteDiagnosticSourceManuallyAsync(
		DiagnosticSourceExecution execution,
		CancellationToken cancellationToken)
		=> PostAsync<DiagnosticSourceExecution, DiagnosticSourceExecution>(
			execution,
			"setting/diagnosticsources/executemanually",
			cancellationToken);
}
