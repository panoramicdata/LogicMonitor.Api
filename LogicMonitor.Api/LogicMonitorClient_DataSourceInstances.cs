namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets a list of DataSourceInstances
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The ResourceDataSource instance id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceDataSourceInstance> GetResourceDataSourceInstanceAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ResourceDataSourceInstance>($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}", cancellationToken);

	/// <summary>
	///     Get DataSource Instance Groups
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceDataSourceInstanceGroup>> GetResourceDataSourceInstanceGroupsAsync(
		int resourceId,
		int resourceDataSourceId,
		CancellationToken cancellationToken)
		=> GetAllAsync(
			filter: new Filter<ResourceDataSourceInstanceGroup>
			{
				FilterItems =
				[
					new Eq<ResourceDataSourceInstanceGroup>(nameof(ResourceDataSourceInstanceGroup.ResourceDataSourceId), resourceDataSourceId)
				]
			},
			subUrl: $"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/groups/",
			cancellationToken);

	/// <summary>
	///     Get DataSource Instance Groups
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="name">The device dataSource instanceGroup name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceDataSourceInstanceGroup?> GetResourceDataSourceInstanceGroupByNameAsync(
		int resourceId,
		int resourceDataSourceId,
		string name,
		CancellationToken cancellationToken)
		=> (await GetResourceDataSourceInstanceGroupsAsync(resourceId, resourceDataSourceId, cancellationToken).ConfigureAwait(false)).SingleOrDefault(ig => ig.Name == name);

	/// <summary>
	///     Get DataSource Instance Group details
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="resourceDataSourceInstanceGroupId"></param>
	/// <param name="sendNullIfError"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceDataSourceInstanceGroup?> GetResourceDataSourceInstanceGroupAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceGroupId,
		bool sendNullIfError,
		CancellationToken cancellationToken)
	{
		try
		{
			return await GetBySubUrlAsync<ResourceDataSourceInstanceGroup>(
				$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/groups/{resourceDataSourceInstanceGroupId}",
				cancellationToken).ConfigureAwait(false);
		}
		catch (Exception)
		{
			if (sendNullIfError)
			{
				return null;
			}

			throw;
		}
	}

	/// <summary>
	///     Get DataSourceInstanceGroup instances
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="resourceDataSourceInstanceGroupId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ResourceDataSourceInstance>> GetResourceDataSourceInstanceGroupInstancesPageAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceGroupId,
		Filter<ResourceDataSourceInstance> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<ResourceDataSourceInstance>>(
			$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/groups/{resourceDataSourceInstanceGroupId}/instances?{filter}",
			cancellationToken);

	/// <summary>
	///     Gets a list of DataSources that apply to a ResourceGroup
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceGroupDataSource>> GetAllResourceGroupDataSourcesAsync(
		int resourceGroupId,
		CancellationToken cancellationToken)
		=> GetAllAsync<ResourceGroupDataSource>($"device/groups/{resourceGroupId}/datasources", cancellationToken);

	/// <summary>
	///     Gets a list of DataPointConfiguration for a specific Resource, ResourceDataSource, and ResourceDataSourceInstance
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The ResourceDataSourceInstance id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<DataPointConfiguration>> GetResourceDataSourceInstanceDataPointConfigurationsAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		CancellationToken cancellationToken)
		=> GetAllAsync<DataPointConfiguration>(
			$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/alertsettings",
			cancellationToken);

	/// <summary>
	///     Gets a list of Configs for a specific Resource, ResourceDataSource, and ResourceDataSourceInstance
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The ResourceDataSourceInstance id</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceDataSourceInstanceConfig>> GetAllResourceDataSourceInstanceConfigsAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		Filter<ResourceDataSourceInstanceConfig> filter,
		CancellationToken cancellationToken)
		=> GetAllAsync(
			filter,
			$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/config",
			cancellationToken);

	/// <summary>
	///     Gets a list of DataPointConfiguration for a specific Resource, ResourceDataSource, and ResourceDataSourceInstance
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The ResourceDataSourceInstance id</param>
	/// <param name="dataPointId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DataPointConfiguration> GetSingleResourceDataSourceInstanceDataPointConfigurationAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		int dataPointId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DataPointConfiguration>(
			$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/alertsettings/{dataPointId}",
			cancellationToken);

	/// <summary>
	/// Sets alert thresholds for an entire Resource datasource instance group
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The Resource DataSource Instance Group Id (0 == default)</param>
	/// <param name="dataPointId">The DataPoint Id</param>
	/// <param name="dataPointConfiguration"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceDataSourceInstanceDataPointConfigurationAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		int dataPointId,
		DataPointConfiguration dataPointConfiguration,
		CancellationToken cancellationToken)
		=> PutAsync($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/alertsettings/{dataPointId}",
			dataPointConfiguration, cancellationToken);

	/// <summary>
	///     Update a DataPointConfiguration
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The Resource DataSource Id</param>
	/// <param name="resourceDataSourceInstanceId">The Resource DataSourceInstance Id</param>
	/// <param name="dataPointConfiguration">The DataPointConfiguration</param>
	/// <param name="dataPointId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task UpdateDataPointConfigurationAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		int dataPointId,
		DataPointConfigurationCreationDTO dataPointConfiguration,
		CancellationToken cancellationToken)
		=> PutAsync(
			$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/alertsettings/{dataPointId}",
			dataPointConfiguration,
			cancellationToken);

	/// <summary>
	///     Gets a ResourceDataSource
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="dataSourceId"></param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public async Task<ResourceDataSource?> GetResourceDataSourceByResourceIdAndDataSourceIdAsync(
	int resourceId,
	int dataSourceId,
	CancellationToken cancellationToken)
	{
		var deviceDataSources = await GetAllResourceDataSourcesAsync(resourceId, new Filter<ResourceDataSource>
		{
			FilterItems =
				[
					new Eq<ResourceDataSource>(nameof(ResourceDataSource.DataSourceId), dataSourceId.ToString(CultureInfo.InvariantCulture))
				]
		}, cancellationToken).ConfigureAwait(false);
		return deviceDataSources.SingleOrDefault();
	}

	/// <summary>
	///     Adds a ResourceDataSource instance
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="deviceDataSourceInstanceCreationDto"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceDataSourceInstance> AddResourceDataSourceInstanceAsync(
		int resourceId,
		int resourceDataSourceId,
		ResourceDataSourceInstanceCreationDto deviceDataSourceInstanceCreationDto,
		CancellationToken cancellationToken) => PostAsync<ResourceDataSourceInstanceCreationDto, ResourceDataSourceInstance>(
			deviceDataSourceInstanceCreationDto,
			$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances",
			cancellationToken);

	/// <summary>
	/// add audit version
	/// </summary>
	/// <param name="id">The datasource id</param>
	/// <param name="body">The audit to be added</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DataSource> AddDataSourceAuditVersionAsync(
		int id,
		Audit body,
		CancellationToken cancellationToken)
		=> PostAsync<Audit, DataSource>(body,
			$"setting/datasources/{id}/audit",
			cancellationToken);

	/// <summary>
	/// collect a config for a Resource
	/// </summary>
	/// <param name="resourceId">The deviceId</param>
	/// <param name="resourceDataSourceId">The device datasource id</param>
	/// <param name="instanceId">The instanceId</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<object> CollectResourceConfigSourceConfig(
		int resourceId,
		int resourceDataSourceId,
		int instanceId,
		CancellationToken cancellationToken)
		=> PostAsync<object?, object>(null, $"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{instanceId}/config/configCollection", cancellationToken);

	/// <summary>
	/// get update history for a DataSource
	/// </summary>
	/// <param name="id"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<DataSourceUpdateReason>> GetDataSourceUpdateReasonAsync(
		int id,
		Filter<DataSourceUpdateReason> filter,
		CancellationToken cancellationToken)
		=> GetPageAsync(filter, $"setting/datasources/{id}/updatereasons", cancellationToken);

	/// <summary>
	/// Updates a ResourceDataSourceInstance
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="resourceDataSourceInstanceId"></param>
	/// <param name="resourceDataSourceInstance"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task UpdateResourceDataSourceInstanceAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		ResourceDataSourceInstance resourceDataSourceInstance,
		CancellationToken cancellationToken) => PutAsync(
				$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}",
				resourceDataSourceInstance,
				cancellationToken);

	/// <summary>
	/// Updates individual fields on a ResourceDataSourceInstance, leaving every other field alone.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Prefer this over <see cref="UpdateResourceDataSourceInstanceAsync(int, int, int, ResourceDataSourceInstance, CancellationToken)"/>
	/// when changing one or two fields. That method issues a full PUT, so it sends back every field
	/// of the instance it was handed - including any the caller did not mean to write, and any that
	/// were stale by the time the call was made.
	/// </para>
	/// <para>
	/// The instance endpoint is nested under its resource and resource DataSource, which the flat
	/// <see cref="IHasEndpoint.Endpoint"/> contract cannot express - hence a named method rather than
	/// the generic patch path. See issue #29.
	/// </para>
	/// </remarks>
	/// <param name="resourceId">The resource id</param>
	/// <param name="resourceDataSourceId">The resource DataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The instance id</param>
	/// <param name="fieldsToUpdate">The fields to update, keyed by their JSON names</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task PatchResourceDataSourceInstanceAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		Dictionary<string, object> fieldsToUpdate,
		CancellationToken cancellationToken) => PatchAsync(
			$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}",
			fieldsToUpdate,
			cancellationToken);

	/// <summary>
	/// Enables or disables alerting on a ResourceDataSourceInstance.
	/// </summary>
	/// <remarks>
	/// This is the standard mechanism for per-instance alert suppression (for example silencing a
	/// single switch port), and is normally applied in bulk. Only the disableAlerting field is sent.
	/// </remarks>
	/// <param name="resourceId">The resource id</param>
	/// <param name="resourceDataSourceId">The resource DataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The instance id</param>
	/// <param name="disableAlerting">True to suppress alerting on the instance</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceDataSourceInstanceAlertingAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		bool disableAlerting,
		CancellationToken cancellationToken) => PatchResourceDataSourceInstanceAsync(
			resourceId,
			resourceDataSourceId,
			resourceDataSourceInstanceId,
			new Dictionary<string, object> { ["disableAlerting"] = disableAlerting },
			cancellationToken);

	/// <summary>
	/// Enables or disables alerting on a ResourceDataSourceInstance, taking the ids from the instance.
	/// </summary>
	/// <param name="resourceDataSourceInstance">The instance to update</param>
	/// <param name="disableAlerting">True to suppress alerting on the instance</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceDataSourceInstanceAlertingAsync(
		ResourceDataSourceInstance resourceDataSourceInstance,
		bool disableAlerting,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(resourceDataSourceInstance);

		return SetResourceDataSourceInstanceAlertingAsync(
			resourceDataSourceInstance.ResourceId,
			resourceDataSourceInstance.ResourceDataSourceId,
			resourceDataSourceInstance.Id,
			disableAlerting,
			cancellationToken);
	}

	/// <summary>
	/// Starts or stops monitoring on a ResourceDataSourceInstance.
	/// </summary>
	/// <remarks>Only the stopMonitoring field is sent.</remarks>
	/// <param name="resourceId">The resource id</param>
	/// <param name="resourceDataSourceId">The resource DataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The instance id</param>
	/// <param name="stopMonitoring">True to stop monitoring the instance</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceDataSourceInstanceMonitoringAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		bool stopMonitoring,
		CancellationToken cancellationToken) => PatchResourceDataSourceInstanceAsync(
			resourceId,
			resourceDataSourceId,
			resourceDataSourceInstanceId,
			new Dictionary<string, object> { ["stopMonitoring"] = stopMonitoring },
			cancellationToken);

	/// <summary>
	/// Starts or stops monitoring on a ResourceDataSourceInstance, taking the ids from the instance.
	/// </summary>
	/// <param name="resourceDataSourceInstance">The instance to update</param>
	/// <param name="stopMonitoring">True to stop monitoring the instance</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceDataSourceInstanceMonitoringAsync(
		ResourceDataSourceInstance resourceDataSourceInstance,
		bool stopMonitoring,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(resourceDataSourceInstance);

		return SetResourceDataSourceInstanceMonitoringAsync(
			resourceDataSourceInstance.ResourceId,
			resourceDataSourceInstance.ResourceDataSourceId,
			resourceDataSourceInstance.Id,
			stopMonitoring,
			cancellationToken);
	}
}
