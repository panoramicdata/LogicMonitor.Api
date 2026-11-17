namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{

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
}
