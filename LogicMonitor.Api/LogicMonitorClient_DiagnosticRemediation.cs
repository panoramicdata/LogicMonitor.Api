namespace LogicMonitor.Api;

/// <summary>
///     Automated diagnostics and remediation portal interaction
/// </summary>
/// <remarks>RemediationSource CRUD uses the generic methods, for example GetAllAsync&lt;RemediationSource&gt;</remarks>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Trigger a RemediationSource for a resource
	/// </summary>
	/// <param name="execution">The resource, RemediationSource and optional alert</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<RemediationSourceExecution> ExecuteRemediationSourceAsync(
		RemediationSourceExecution execution,
		CancellationToken cancellationToken)
		=> PostAsync<RemediationSourceExecution, RemediationSourceExecution>(
			execution,
			"setting/remediationsources/executemanually",
			cancellationToken);

	/// <summary>
	///     Get diagnostic and remediation execution results for exactly one of an alert or a resource
	/// </summary>
	/// <param name="alertId">The alert id, or null when resourceId is given</param>
	/// <param name="resourceId">The resource id, or null when alertId is given</param>
	/// <param name="skip">The number of results to skip</param>
	/// <param name="take">The number of results to return</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DiagnosticRemediationExecutionResultPage> GetDiagnosticRemediationExecutionResultsAsync(
		string? alertId,
		int? resourceId,
		int skip,
		int take,
		CancellationToken cancellationToken)
	{
		if ((alertId is null) == (resourceId is null))
		{
			throw new ArgumentException($"Provide exactly one of {nameof(alertId)} or {nameof(resourceId)}.");
		}

		var target = alertId is null ? $"hostId={resourceId}" : $"alertId={Uri.EscapeDataString(alertId)}";
		return GetBySubUrlAsync<DiagnosticRemediationExecutionResultPage>(
			$"setting/diagnosticRemediation/executionResults?{target}&pageOffsetCount={skip}&perPageCount={take}",
			cancellationToken);
	}

	/// <summary>
	///     List the diagnostic and remediation sources assigned to a resource or alert
	/// </summary>
	/// <param name="resourceId">The resource id, or null</param>
	/// <param name="alertId">The alert id, or null; at least one of resourceId and alertId is required</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DiagnosticRemediationAssignedSource>> GetDiagnosticRemediationAssignedSourcesAsync(
		int? resourceId,
		string? alertId,
		CancellationToken cancellationToken)
	{
		if (resourceId is null && alertId is null)
		{
			throw new ArgumentException($"Provide {nameof(resourceId)}, {nameof(alertId)} or both.");
		}

		var query = new List<string>();
		if (resourceId is not null)
		{
			query.Add($"resourceId={resourceId}");
		}

		if (alertId is not null)
		{
			query.Add($"alertId={Uri.EscapeDataString(alertId)}");
		}

		return GetBySubUrlAsync<Page<DiagnosticRemediationAssignedSource>>(
			$"setting/diagnosticRemediation/list?{string.Join("&", query)}",
			cancellationToken);
	}
}
