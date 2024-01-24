namespace LogicMonitor.Api;

/// <summary>
///    Report interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///    Runs a Report by ID and returns 
	/// </summary>
	/// <param name="reportId">The report ID to run</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The website</returns>
	public Task<RunReportResponse> RunReportById(int reportId, CancellationToken cancellationToken = default)
	  => PostAsync<RunReportRequest, RunReportResponse>(new RunReportRequest(), $"report/reports/{reportId}/executions", cancellationToken);
}
