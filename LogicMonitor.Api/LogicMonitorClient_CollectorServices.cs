namespace LogicMonitor.Api;

/// <summary>
///     Collector events, logs, status checks, restarts, log levels, upgrade history and sizing
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Get a collector's events
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<CollectorEvent>> GetCollectorEventsAsync(
		int collectorId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<CollectorEvent>>($"setting/collector/collectors/{collectorId}/events", cancellationToken);

	/// <summary>
	///     List a collector's log files
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<CollectorLogFile>> GetCollectorLogFilesAsync(
		int collectorId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<CollectorLogFile>>($"setting/collector/collectors/{collectorId}/logs", cancellationToken);

	/// <summary>
	///     Get one of a collector's log files
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="fileName">The log file name, as listed by <see cref="GetCollectorLogFilesAsync"/></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<CollectorLogFile> GetCollectorLogFileAsync(
		int collectorId,
		string fileName,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<CollectorLogFile>($"setting/collector/collectors/{collectorId}/logs/{Uri.EscapeDataString(fileName)}", cancellationToken);

	/// <summary>
	///     Get the result of the most recent collector status check
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<CollectorStatusCheck> GetCollectorStatusCheckAsync(
		int collectorId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<CollectorStatusCheck>($"setting/collector/collectors/{collectorId}/services/getStatusCheck", cancellationToken);

	/// <summary>
	///     Start a collector status check; read the outcome with <see cref="GetCollectorStatusCheckAsync"/>
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The portal's response text</returns>
	public Task<string> TriggerCollectorStatusCheckAsync(
		int collectorId,
		CancellationToken cancellationToken)
		=> SendRawAsync(HttpMethod.Post, $"setting/collector/collectors/{collectorId}/services/triggerStatusCheck", null, "application/json", cancellationToken);

	/// <summary>
	///     Restart a collector, optionally replacing its configuration files
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="restartRequest">Replacement configuration, or an empty request to restart unchanged</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task RestartCollectorAsync(
		int collectorId,
		CollectorRestartRequest restartRequest,
		CancellationToken cancellationToken)
		=> SendRawJsonAsync<JToken>(HttpMethod.Post, $"setting/collector/collectors/{collectorId}/services/restart", restartRequest, cancellationToken);

	/// <summary>
	///     Get the log levels of all of a collector's agent components
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<CollectorAgentLogLevel>> GetCollectorAgentLogLevelsAsync(
		int collectorId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<CollectorAgentLogLevel>>($"setting/collector/collectors/{collectorId}/agentloglevels", cancellationToken);

	/// <summary>
	///     Get the log level of one collector agent component
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="component">The component name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<CollectorAgentLogLevel> GetCollectorAgentLogLevelAsync(
		int collectorId,
		string component,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<CollectorAgentLogLevel>($"setting/collector/collectors/{collectorId}/agentloglevels/{Uri.EscapeDataString(component)}", cancellationToken);

	/// <summary>
	///     Set the log levels of collector agent components
	/// </summary>
	/// <param name="collectorId">The collector id</param>
	/// <param name="logLevels">The components and their new log levels</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<CollectorAgentLogLevel>> SetCollectorAgentLogLevelsAsync(
		int collectorId,
		List<CollectorAgentLogLevel> logLevels,
		CancellationToken cancellationToken)
		=> await SendRawJsonAsync<List<CollectorAgentLogLevel>>(
			HttpMethod.Put,
			$"setting/collector/collectors/{collectorId}/agentloglevels",
			logLevels,
			cancellationToken).ConfigureAwait(false) ?? [];

	/// <summary>
	///     Get the collector upgrade history
	/// </summary>
	/// <param name="filter">An optional filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<CollectorUpgradeHistoryItem>> GetCollectorUpgradeHistoryAsync(
		Filter<CollectorUpgradeHistoryItem>? filter,
		CancellationToken cancellationToken)
		=> GetPageAsync(filter, "setting/collector/collectors/upgradeHistory", cancellationToken);

	/// <summary>
	///     Calculate how many collectors of which size a workload needs
	/// </summary>
	/// <param name="sizingRequest">The workload</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<CollectorSizingResponse> CalculateCollectorSizingAsync(
		CollectorSizingRequest sizingRequest,
		CancellationToken cancellationToken)
		=> PostAsync<CollectorSizingRequest, CollectorSizingResponse>(sizingRequest, "setting/collector/sizing", cancellationToken);
}
