namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{

	/// <summary>
	///		Get device alerts with an alert filter (not all properties in the filter are used)
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="filter">An AlertFilter</param>
	/// <param name="calledFromChunked"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	/// <returns>A List of Alerts and whether the limit was reached</returns>
	internal Task<(List<Alert> alerts, bool limitReached)> GetResourceAlertsByIdNormalAsync(
		int resourceId,
		AlertFilter filter,
		bool calledFromChunked,
		CancellationToken cancellationToken)
		=> GetEntityAlertPagesAsync($"device/devices/{resourceId}/alerts", filter, calledFromChunked, cancellationToken);

	/// <summary>
	/// Get AWS external id
	/// </summary>
	public Task<AwsExternalId> GetExternalIdAsync(CancellationToken cancellationToken)
		=> GetBySubUrlAsync<AwsExternalId>($"aws/externalId", cancellationToken);

	/// <summary>
	/// Get ResourceGroup SDTs
	/// </summary>
	/// <param name="id"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<ScheduledDownTime>> GetResourceGroupSdtsAsync(
		int id,
		Filter<ScheduledDownTime> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/groups/{id}/sdts", filter, cancellationToken);

	/// <summary>
	/// Get unmonitored device list
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<UnmonitoredResource>> GetUnmonitoredResourcesAsync(
		Filter<UnmonitoredResource> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/unmonitoreddevices", filter, cancellationToken);

	/// <summary>
	/// Get all cluster alert configurations for a ResourceGroup
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<ResourceGroupClusterAlertConfig>> GetAllResourceGroupClusterAlertConfigsAsync(
		int resourceGroupId,
		CancellationToken cancellationToken)
	{
		var items = await GetAllAsync<ResourceGroupClusterAlertConfig>(
			$"device/groups/{resourceGroupId}/clusterAlertConf",
			cancellationToken).ConfigureAwait(false);
		foreach (var item in items)
		{
			item.ResourceGroupId = resourceGroupId;
		}

		return items;
	}

	/// <summary>
	/// Get a filtered page of cluster alert configurations for a ResourceGroup
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<Page<ResourceGroupClusterAlertConfig>> GetResourceGroupClusterAlertConfigsPageAsync(
		int resourceGroupId,
		Filter<ResourceGroupClusterAlertConfig>? filter,
		CancellationToken cancellationToken)
	{
		var page = await FilteredGetAsync(
			$"device/groups/{resourceGroupId}/clusterAlertConf",
			filter,
			cancellationToken).ConfigureAwait(false);
		foreach (var item in page.Items ?? [])
		{
			item.ResourceGroupId = resourceGroupId;
		}

		return page;
	}

	/// <summary>
	/// Get a single cluster alert configuration by Id
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="id">The cluster alert configuration Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceGroupClusterAlertConfig> GetResourceGroupClusterAlertConfigAsync(
		int resourceGroupId,
		int id,
		CancellationToken cancellationToken)
	{
		var item = await GetBySubUrlAsync<ResourceGroupClusterAlertConfig>(
			$"device/groups/{resourceGroupId}/clusterAlertConf/{id}",
			cancellationToken).ConfigureAwait(false);
		item.ResourceGroupId = resourceGroupId;
		return item;
	}

	/// <summary>
	/// Create a cluster alert configuration for a ResourceGroup
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="config">The configuration to create</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceGroupClusterAlertConfig> CreateResourceGroupClusterAlertConfigAsync(
		int resourceGroupId,
		ResourceGroupClusterAlertConfig config,
		CancellationToken cancellationToken)
	{
		var result = await PostAsync<ResourceGroupClusterAlertConfig, ResourceGroupClusterAlertConfig>(
			config,
			$"device/groups/{resourceGroupId}/clusterAlertConf",
			cancellationToken).ConfigureAwait(false);
		result.ResourceGroupId = resourceGroupId;
		return result;
	}

	/// <summary>
	/// Replace a cluster alert configuration (full update via PUT)
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="config">The updated configuration (must have Id set)</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task UpdateResourceGroupClusterAlertConfigAsync(
		int resourceGroupId,
		ResourceGroupClusterAlertConfig config,
		CancellationToken cancellationToken)
		=> PutAsync(
			$"device/groups/{resourceGroupId}/clusterAlertConf/{config.Id}",
			config,
			cancellationToken);

	/// <summary>
	/// Partially update a cluster alert configuration (PATCH)
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="id">The cluster alert configuration Id</param>
	/// <param name="fieldsToUpdate">The fields to update</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task PatchResourceGroupClusterAlertConfigAsync(
		int resourceGroupId,
		int id,
		Dictionary<string, object> fieldsToUpdate,
		CancellationToken cancellationToken)
		=> PatchAsync(
			$"device/groups/{resourceGroupId}/clusterAlertConf/{id}",
			fieldsToUpdate,
			cancellationToken);

	/// <summary>
	/// Delete a cluster alert configuration from a ResourceGroup
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="id">The cluster alert configuration Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task DeleteResourceGroupClusterAlertConfigAsync(
		int resourceGroupId,
		int id,
		CancellationToken cancellationToken)
		=> DeleteAsync(
			$"device/groups/{resourceGroupId}/clusterAlertConf/{id}",
			cancellationToken);

	/// <summary>
	/// Get Resource SDTs
	/// </summary>
	/// <param name="id">The Resource id</param>
	/// <param name="filter">An optional filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ScheduledDownTime>> GetResourceSdtsAsync(
		int id,
		Filter<ScheduledDownTime>? filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/devices/{id}/sdts", filter, cancellationToken);

	/// <summary>
	/// Start tracking Resource changes: returns the matching Resources and a delta id for <see cref="GetResourceDeltaAsync"/>
	/// </summary>
	/// <param name="filter">An optional filter selecting which Resources to track</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceDeltaPage> StartResourceDeltaAsync(
		Filter<Resource>? filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ResourceDeltaPage>($"device/devices/delta{(filter is null ? string.Empty : $"?{filter}")}", cancellationToken);

	/// <summary>
	/// Get the Resources changed since a delta was started
	/// </summary>
	/// <param name="deltaId">The delta id returned by <see cref="StartResourceDeltaAsync"/></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceDeltaPage> GetResourceDeltaAsync(
		string deltaId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ResourceDeltaPage>($"device/devices/delta/{Uri.EscapeDataString(deltaId)}", cancellationToken);
}
