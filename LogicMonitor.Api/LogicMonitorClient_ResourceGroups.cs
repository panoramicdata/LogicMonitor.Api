namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets the full Resource tree
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceGroup> GetFullResourceTreeAsync(
		[Optional, DefaultParameterValue(-1)] int resourceGroupId,
		[Optional] CancellationToken cancellationToken)
	{
		var allResourceGroups = await GetAllAsync<ResourceGroup>(cancellationToken: cancellationToken).ConfigureAwait(false);
		var requestedRootGroup = allResourceGroups.SingleOrDefault(g => g.Id == resourceGroupId) ?? new ResourceGroup { Name = "Root", Id = -1 };
		if (resourceGroupId != requestedRootGroup.Id)
		{
			throw new ArgumentOutOfRangeException($"No ResourceGroup found with id {resourceGroupId}");
		}

		await LinkResourceGroupsAsync(allResourceGroups, requestedRootGroup, resourceGroupId, cancellationToken).ConfigureAwait(false);

		AttachResources(allResourceGroups, await GetAllAsync<Resource>(cancellationToken: cancellationToken).ConfigureAwait(false));

		return requestedRootGroup;
	}

	private async Task LinkResourceGroupsAsync(
		List<ResourceGroup> allResourceGroups,
		ResourceGroup requestedRootGroup,
		int resourceGroupId,
		CancellationToken cancellationToken)
	{
		foreach (var resourceGroup in allResourceGroups)
		{
			var parentGroup = allResourceGroups.SingleOrDefault(g => g.Id == resourceGroup.ParentId) ?? (resourceGroupId == -1 ? requestedRootGroup : null);
			if (parentGroup is null)
			{
				continue;
			}

			parentGroup.SubGroups ??= [];
			parentGroup.SubGroups.Add(resourceGroup);

			var detailedResourceGroup = await GetAsync<ResourceGroup>(
				resourceGroup.Id,
				cancellationToken)
				.ConfigureAwait(false);

			resourceGroup.AlertStatus = detailedResourceGroup.AlertStatus;
		}
	}

	private static void AttachResources(List<ResourceGroup> allResourceGroups, List<Resource> resources)
	{
		foreach (var resource in resources)
		{
			var resourceGroups = resource
				.ResourceGroupIdsString
				.Split(',')
				.Select(int.Parse)
				.Select(dgId => allResourceGroups.SingleOrDefault(g => g.Id == dgId))
				.OfType<ResourceGroup>();

			// A null group is skipped, which avoids a race condition
			foreach (var resourceGroup in resourceGroups)
			{
				resourceGroup.Resources ??= [];
				resourceGroup.Resources.Add(resource);
				resourceGroup.ResourceCount = resourceGroup.Resources.Count;
			}
		}
	}

	/// <summary>
	///     Gets a list of processes being monitored for a Resource
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="deviceProcessServiceTaskType">The process/service type</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<ResourceDataSourceInstance>> GetMonitoredResourceProcessesAsync(
		int resourceId,
		ResourceProcessServiceTaskType deviceProcessServiceTaskType,
		CancellationToken cancellationToken)
	{
		var dataSourceName = deviceProcessServiceTaskType switch
		{
			ResourceProcessServiceTaskType.LinuxProcess => "LinuxNewProcesses-",
			ResourceProcessServiceTaskType.WindowsProcess => "WinProcessStats-",
			ResourceProcessServiceTaskType.WindowsService => "Microsoft_Windows_Services",
			_ => throw new ArgumentException($"Only {ResourceProcessServiceTaskType.LinuxProcess}, {ResourceProcessServiceTaskType.WindowsProcess} and {ResourceProcessServiceTaskType.WindowsService} are supported", nameof(deviceProcessServiceTaskType)),
		};
		var filter = new Filter<ResourceDataSource>
		{
			FilterItems =
				[
					new Eq<ResourceDataSource>(nameof(ResourceDataSource.DataSourceName), dataSourceName),
					new Eq<ResourceDataSource>(nameof(ResourceDataSource.DataSourceType), "DS")
				]
		};
		var deviceDataSources = await GetAllAsync(filter, $"device/devices/{resourceId}/devicedatasources", cancellationToken).ConfigureAwait(false);

		return deviceDataSources.Count != 1
			? ([])
			: await GetAllResourceDataSourceInstancesAsync(
			resourceId,
			deviceDataSources.Single().Id,
			new Filter<ResourceDataSourceInstance>(),
			cancellationToken
		)
		.ConfigureAwait(false);
	}

	/// <summary>
	///     Initiates a task to fetch the list of current running processes on a device
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceProcessServiceTaskType">The device process/service task type</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceProcessServiceTask> GetProcessServiceTaskForResourceAsync(
		int resourceId,
		ResourceProcessServiceTaskType resourceProcessServiceTaskType,
		CancellationToken cancellationToken)
		=> PostAsync<ResourceProcessServiceTask, ResourceProcessServiceTask>(
			new ResourceProcessServiceTask
			{
				Type = resourceProcessServiceTaskType
			},
			$"device/devices/{resourceId}/fetchProcessServiceTask",
			cancellationToken
		);

	private const int ProcessServiceTaskRunningErrorCode = 1202;

	private static readonly TimeSpan DefaultProcessServiceTaskMaximumWait = TimeSpan.FromMinutes(5);

	/// <summary>
	///     Fetches a task result, waiting up to five minutes for the collector to finish
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="taskId">The task id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The task results</returns>
	public Task<Page<ResourceProcess>> GetProcessServiceTaskResultsAsync(
		int resourceId,
		int taskId,
		CancellationToken cancellationToken)
		=> GetProcessServiceTaskResultsAsync(resourceId, taskId, DefaultProcessServiceTaskMaximumWait, cancellationToken);

	/// <summary>
	///     Fetches a task result, waiting up to maximumWait for the collector to finish
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="taskId">The task id</param>
	/// <param name="maximumWait">How long to wait for the task before throwing a TimeoutException</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The task results</returns>
	public async Task<Page<ResourceProcess>> GetProcessServiceTaskResultsAsync(
		int resourceId,
		int taskId,
		TimeSpan maximumWait,
		CancellationToken cancellationToken)
	{
		var stopwatch = Stopwatch.StartNew();
		while (true)
		{
			// Uncached: each poll must reach the portal, and a finished result can only be read once
			var response = await GetAsync<JObject>(false, $"device/devices/{resourceId}/fetchProcessServiceTask/{taskId}", cancellationToken).ConfigureAwait(false);
			var page = ReadProcessServiceTaskResult(response);
			if (page is not null)
			{
				return page;
			}

			if (stopwatch.Elapsed >= maximumWait)
			{
				throw new TimeoutException($"Process/service task {taskId} on Resource {resourceId} was still running after {maximumWait}.");
			}

			await Task.Delay(500, cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>
	///     Interprets a fetchProcessServiceTask response: null while the task is running, the page once it has finished
	/// </summary>
	internal static Page<ResourceProcess>? ReadProcessServiceTaskResult(JObject response)
	{
		ArgumentNullException.ThrowIfNull(response);
		var errorCode = response.Value<int?>("errorCode");
		if (errorCode is null)
		{
			return response.ToObject<Page<ResourceProcess>>()
				?? throw new FormatException("Could not convert response to a page of device process.");
		}

		return errorCode == ProcessServiceTaskRunningErrorCode
			? null
			: throw new LogicMonitorApiException($"Process/service task failed ({errorCode}): {response.Value<string>("errorMessage")}");
	}

	/// <summary>
	///     Gets currently-running device processes
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceProcessServiceTaskType">The process/service type</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<Page<ResourceProcess>> GetResourceProcessesAsync(
		int resourceId,
		ResourceProcessServiceTaskType resourceProcessServiceTaskType,
		CancellationToken cancellationToken)
	{
		var task = await GetProcessServiceTaskForResourceAsync(resourceId, resourceProcessServiceTaskType, cancellationToken).ConfigureAwait(false);
		return await GetProcessServiceTaskResultsAsync(resourceId, task.TaskId, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets a Resource's DataPointConfigurations
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="cancellationToken"></param>
	public Task<List<DataPointConfiguration>> GetResourceDataPointConfigurationsAsync(
		int resourceId,
		CancellationToken cancellationToken)
		=> GetAllAsync<DataPointConfiguration>(
			$"device/devices/{resourceId}/alertsettings",
			cancellationToken);

	/// <summary>
	/// Sets alert thresholds for an entire Resource DataSource Instance Group
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The Resource DataSource id</param>
	/// <param name="resourceDataSourceInstanceGroupId">The deviceDataSource Instance Group Id (0 == default)</param>
	/// <param name="dataPointId">The DataPoint Id</param>
	/// <param name="alertExpression">The alert expression (e.g. >= "90 90 90")</param>
	/// <param name="alertExpressionNote">A note explaining the threshold</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceDataSourceInstanceGroupDataPointThresholdsAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceGroupId,
		int dataPointId,
		string alertExpression,
		string alertExpressionNote,
		CancellationToken cancellationToken)
		=> PutAsync($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/groups/{resourceDataSourceInstanceGroupId}/datapoints/{dataPointId}/alertconfig",
			new ThresholdSpecification
			{
				AlertExpression = alertExpression,
				AlertExpressionNote = alertExpressionNote
			}, cancellationToken);

	/// <summary>
	/// Sets alert thresholds for an entire Resource DataSource Instance Group
	/// </summary>
	/// <param name="resourceGroupId">The Resource id</param>
	/// <param name="dataSourceId">The DataSource id</param>
	/// <param name="dataPointId">The DataPoint Id</param>
	/// <param name="alertExpression">The alert expression (e.g. >= "90 90 90")</param>
	/// <param name="alertExpressionNote">A note explaining the threshold</param>
	/// <param name="disableAlerting">Disable alerting</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task SetResourceGroupDataSourceDataPointThresholdsAsync(
		int resourceGroupId,
		int dataSourceId,
		int dataPointId,
		string alertExpression,
		string alertExpressionNote,
		bool? disableAlerting,
		CancellationToken cancellationToken)
	{
		// Need to get the existing Alert config, modify it and then PUT it back
		var url = $"device/groups/{resourceGroupId}/datasources/{dataSourceId}/alertsettings";

		var dataPointConfigurationCollection = await GetBySubUrlAsync<DataPointConfigurationCollection>(url, cancellationToken).ConfigureAwait(false);

		var changeMade = false;

		foreach (var dataPointConfiguration in dataPointConfigurationCollection.Items.Where(c => c.DataPointId == dataPointId))
		{
			changeMade |= ApplyThresholdChanges(dataPointConfiguration, alertExpression, alertExpressionNote, disableAlerting);
		}

		if (changeMade)
		{
			await PutAsync(url, dataPointConfigurationCollection, cancellationToken).ConfigureAwait(false);
		}
	}

	private static bool ApplyThresholdChanges(
		DataPointConfiguration dataPointConfiguration,
		string alertExpression,
		string alertExpressionNote,
		bool? disableAlerting)
	{
		var changeMade = false;

		if (alertExpression is not null && dataPointConfiguration.AlertExpression != alertExpression)
		{
			dataPointConfiguration.AlertExpression = alertExpression;
			changeMade = true;
		}

		if (alertExpressionNote is not null && dataPointConfiguration.AlertExpressionNote != alertExpressionNote)
		{
			dataPointConfiguration.AlertExpressionNote = alertExpressionNote;
			changeMade = true;
		}

		if (disableAlerting.HasValue && dataPointConfiguration.DisableAlerting != disableAlerting)
		{
			dataPointConfiguration.DisableAlerting = disableAlerting.Value;
			changeMade = true;
		}

		return changeMade;
	}

	/// <summary>
	///     Gets a list of DataPointThresholdDetails
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="dataSourceId">The dataSource Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<DataPointConfiguration>> GetResourceGroupDataPointConfigurationAsync(
		int resourceGroupId,
		int dataSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<DataPointConfigurationCollection>($"device/groups/{resourceGroupId}/datasources/{dataSourceId}/alertsettings", cancellationToken).ConfigureAwait(false)).Items;

	/// <summary>
	/// get Resource instance list
	/// </summary>
	/// <param name="id">The Resource id</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ResourceDataSourceInstance>> GetResourceInstanceListAsync(
		int id,
		Filter<ResourceDataSourceInstance> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/devices/{id}/instances", filter, cancellationToken);

	/// <summary>
	/// get top talkers graph
	/// </summary>
	public Task<CustomGraphWidgetData> GetTopTalkersGraphAsync(
		int id,
		CancellationToken cancellationToken) => GetBySubUrlAsync<CustomGraphWidgetData>($"device/devices/{id}/topTalkersGraph", cancellationToken);

	/// <summary>
	/// Get Alerts for a Resource by ID
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="filter">The Alert Filter</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	/// <returns>A List of Alerts</returns>
	public async Task<List<Alert>> GetResourceAlertsByIdAsync(
		int resourceId,
		AlertFilter filter,
		CancellationToken cancellationToken)
	{
		// Ensure that the filter CANNOT override the device ID set!
		filter.RemoveMonitorObjectReferences();

		var (alerts, limitReached) = await
			GetResourceAlertsByIdNormalAsync(resourceId, filter, false, cancellationToken)
			.ConfigureAwait(false);

		if (limitReached)
		{
			// Fall back to the chunked method
			alerts = await GetResourceAlertsByIdChunkedAsync(
				resourceId,
				filter,
				TimeSpan.FromHours(24),
				cancellationToken)
				.ConfigureAwait(false);
		}

		if (filter?.IsCleared == true)
		{
			return [.. alerts.Where(a => a.IsCleared)];
		}

		return filter?.IsCleared == false ? [.. alerts.Where(a => !a.IsCleared)] : alerts;
	}

	/// <summary>
	///     This version of the call requests hourly chunks
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="filter"></param>
	/// <param name="chunkSize">The chunk size (TimeSpan)</param>
	/// <param name="cancellationToken"></param>
	internal Task<List<Alert>> GetResourceAlertsByIdChunkedAsync(
		int resourceId,
		AlertFilter filter,
		TimeSpan chunkSize,
		CancellationToken cancellationToken)
	{
		// Ensure that the filter CANNOT override the device ID set!
		filter.RemoveMonitorObjectReferences();

		return GetAlertsChunkedAsync(filter, chunkSize, f => GetResourceAlertsByIdNormalAsync(resourceId, f, true, cancellationToken));
	}

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
