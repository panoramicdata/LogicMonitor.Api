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

	/// <summary>
	/// Get the result of a report generation task started by <see cref="RunReportById"/>
	/// </summary>
	/// <param name="reportId">The report ID</param>
	/// <param name="taskId">The task ID</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ReportTaskResult> GetReportTaskResultAsync(int reportId, long taskId, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ReportTaskResult>($"report/reports/{reportId}/tasks/{taskId}", cancellationToken);
}
