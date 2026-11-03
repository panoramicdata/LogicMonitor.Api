namespace LogicMonitor.Api;

/// <summary>
///     LM Logs pipeline, partition and saved query portal interaction
/// </summary>
/// <remarks>
///     CRUD for LogPipeline, LogPipelineProcessor, LogPartition, LogQueryGroup and TrackedQueryGroup uses the generic methods;
///     use PutStringIdentifiedItemAsync and DeleteStringIdentifiedAsync for the string-identified LogPipeline and LogPartition.
/// </remarks>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Enable or disable a log pipeline processor (LogAlert)
	/// </summary>
	/// <param name="processorId">The processor id</param>
	/// <param name="enabled">Whether the processor should be enabled</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetLogPipelineProcessorEnabledAsync(
		int processorId,
		bool enabled,
		CancellationToken cancellationToken)
		=> PutAsync(
			$"logpipelines/processors/{processorId}/{(enabled ? "enable" : "disable")}",
			new JObject { ["value"] = !enabled },
			cancellationToken);

	/// <summary>
	///     Perform an action on a log partition
	/// </summary>
	/// <param name="partitionId">The partition id</param>
	/// <param name="action">The action, for example pause or resume</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<LogPartition> PerformLogPartitionActionAsync(
		string partitionId,
		string action,
		CancellationToken cancellationToken)
		=> PostAsync<EmptyRequest, LogPartition>(
			new EmptyRequest(),
			$"log/partitions/{Uri.EscapeDataString(partitionId)}/{Uri.EscapeDataString(action)}",
			cancellationToken);

	/// <summary>
	///     Get the log retention options available to log partitions
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<JObject>> GetLogPartitionRetentionsAsync(CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<JObject>>("log/partitions/retentions", cancellationToken);

	/// <summary>
	///     Get the saved log queries in a log query group
	/// </summary>
	/// <param name="logQueryGroupId">The log query group id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<SavedLogQuery>> GetLogQueriesByGroupIdAsync(
		int logQueryGroupId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<SavedLogQuery>>($"log/logquerygroups/{logQueryGroupId}/logqueries", cancellationToken);

	/// <summary>
	///     Get the log query groups of one group type
	/// </summary>
	/// <param name="groupType">The group type: PRIVATE, SHARED or PUBLIC</param>
	/// <param name="allGroups">Whether to include all groups the user can see</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<LogQueryGroup>> GetLogQueryGroupsByGroupTypeAsync(
		string groupType,
		bool allGroups,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<LogQueryGroup>>(
			$"log/logquerygroups/grouptype/{Uri.EscapeDataString(groupType)}?allGroups={(allGroups ? "true" : "false")}",
			cancellationToken);

	/// <summary>
	///     Move saved log queries into a log query group
	/// </summary>
	/// <param name="targetLogQueryGroupId">The id of the group to move the queries to</param>
	/// <param name="logQueryIds">The ids of the queries to move</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task MoveLogQueriesAsync(
		int targetLogQueryGroupId,
		List<string> logQueryIds,
		CancellationToken cancellationToken)
		=> SendRawJsonAsync<JToken>(HttpMethod.Post, $"log/logquerygroups/{targetLogQueryGroupId}/move", logQueryIds, cancellationToken);

	/// <summary>
	///     Import log query groups and their queries; each group is validated and imported independently
	/// </summary>
	/// <param name="groupType">The group type to import into: PRIVATE, SHARED or PUBLIC</param>
	/// <param name="logQueryGroups">The groups to import</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<LogQueryGroupImportResult>> ImportLogQueryGroupsAsync(
		string groupType,
		List<LogQueryGroupImportItem> logQueryGroups,
		CancellationToken cancellationToken)
		=> await SendRawJsonAsync<List<LogQueryGroupImportResult>>(
			HttpMethod.Post,
			$"log/logquerygroups/import?groupType={Uri.EscapeDataString(groupType)}",
			logQueryGroups,
			cancellationToken).ConfigureAwait(false) ?? [];
}
