using LogicMonitor.Api.Resources;

namespace LogicMonitor.Api;

/// <summary>
///     DataSource portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets a list of all DataSources.
	/// </summary>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetAsync(Filter<DataSource>) instead.", true)]
	public async Task<List<DataSource>> GetDataSourcesAsync(Filter<DataSource> filter, CancellationToken cancellationToken)
		=> (await GetAsync(filter, cancellationToken).ConfigureAwait(false)).Items ?? [];

	/// <summary>
	///     Gets a list of all graphs for a DataSource.
	/// </summary>
	/// <param name="dataSourceId"></param>
	/// <param name="cancellationToken"></param>
	public async Task<List<DataSourceGraph>> GetDataSourceGraphsAsync(
		int dataSourceId,
		CancellationToken cancellationToken)
	{
		var page = await GetBySubUrlAsync<Page<DataSourceGraph>>($"setting/datasources/{dataSourceId}/graphs", cancellationToken).ConfigureAwait(false);
		if (page.Items is null)
		{
			return [];
		}

		// DataSourceId is no longer sent, but needed for backups.  Re-add.
		foreach (var item in page.Items)
		{
			item.DataSourceId = dataSourceId;
		}

		return page.Items;
	}

	/// <summary>
	///     Gets a list of all graphs for a DataSource.
	/// </summary>
	/// <param name="dataSourceId">The datasource id</param>
	/// <param name="graphName">The graph name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<DataSourceGraph> GetDataSourceGraphByNameAsync(
		int dataSourceId,
		string graphName,
		CancellationToken cancellationToken)
		=> (await GetDataSourceGraphsAsync(dataSourceId, cancellationToken).ConfigureAwait(false))
			.SingleOrDefault(g => g.Name == graphName);

	/// <summary>
	///     Gets the XML for a DataSource.
	/// </summary>
	/// <param name="dataSourceId">The datasource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<string> GetDataSourceXmlAsync(
		int dataSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/datasources/{dataSourceId}?format=xml", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets an overview graph for a DataSource by graph name.
	/// </summary>
	/// <param name="dataSourceId">The datasource id</param>
	/// <param name="graphName">The graph name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<DataSourceGraph> GetDataSourceOverviewGraphByNameAsync(
		int dataSourceId,
		string graphName,
		CancellationToken cancellationToken)
		=> (await GetDataSourceOverviewGraphsPageAsync(dataSourceId, null, cancellationToken).ConfigureAwait(false))
			.Items
			.SingleOrDefault(g => g.Name == graphName);

	/// <summary>
	///     Gets a DataSource by name OR DisplayName.
	///	    This is a bad approach as it provides a fuzzy match
	///     Deprecated in favour of GetDataSourceByUniqueNameAsync(string dataSourceName)
	/// </summary>
	/// <param name="dataSourceName"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetDataSourceByUniqueNameAsync instead.", true)]
	public Task<DataSource> GetDataSourceByNameAsync(
		string dataSourceName,
		CancellationToken cancellationToken) => throw new NotSupportedException();

	/// <summary>
	///     Gets a DataSource by Name
	/// </summary>
	/// <param name="dataSourceName">The DataSource name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The DataSource or null if no match</returns>
	public async Task<DataSource?> GetDataSourceByUniqueNameAsync(
		string dataSourceName,
		CancellationToken cancellationToken)
	{
		// The following line is very slow UNLESS requesting just the bare minimum of fields.
		// Workaround strategy: get the bare minimum fields required, then fetch the full details for just that datasource

		// Fetch mostly-empty objects with only the required fields
		var filter = new Filter<DataSource>
		{
			Properties =
				[
					nameof(DataSource.Id),
					nameof(DataSource.Name)
				],
			FilterItems =
				[
					new FilterItem<DataSource>
					{
						Property = nameof(DataSource.Name),
						Operation = ":",
						Value = dataSourceName
					}
				]
		};

		var dataSources = await GetAllAsync(filter, cancellationToken).ConfigureAwait(false);

		return dataSources.Count switch
		{
			1 => await GetAsync<DataSource>(dataSources.Single().Id, cancellationToken).ConfigureAwait(false),
			0 => null,
			_ => throw new InvalidOperationException($"Unexpected result count {dataSources.Count}"),
		};
	}

	/// <summary>
	/// Obsolete
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetResourceGroupDataSourceByIdAsync instead", true)]
	public Task<ResourceGroupDataSource> GetDeviceGroupDataSourceByIdAsync(
		int resourceGroupId,
		int id,
		CancellationToken cancellationToken)
		=> GetResourceGroupDataSourceByIdAsync(resourceGroupId, id, cancellationToken);

	/// <summary>
	/// get ResourceGroup datasource
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	public Task<ResourceGroupDataSource> GetResourceGroupDataSourceByIdAsync(
		int resourceGroupId,
		int id,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ResourceGroupDataSource>($"device/groups/{resourceGroupId}/datasources/{id}", cancellationToken);

	/// <summary>
	///     Gets a DataSource graph given dataSourceId and graphId
	/// </summary>
	/// <param name="dataSourceId">The DataSource Id</param>
	/// <param name="graphId">The Graph Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DataSourceGraph> GetDataSourceGraphAsync(int dataSourceId, int graphId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DataSourceGraph>($"setting/datasources/{dataSourceId}/graphs/{graphId}", cancellationToken);

	/// <summary>
	///     Gets a list of DataSource graphs given its dataSourceId
	/// </summary>
	/// <param name="dataSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken"></param>
	public async Task<Page<DataSourceGraph>> GetDataSourceOverviewGraphsPageAsync(
		int dataSourceId,
		Filter<DataSourceGraph>? filter,
		CancellationToken cancellationToken)
	{
		filter ??= new Filter<DataSourceGraph> { Skip = 0, Take = 300 };

		var dataSourceOverviewGraphsPageAsync = await GetBySubUrlAsync<Page<DataSourceGraph>>($"setting/datasources/{dataSourceId}/ographs?{filter}", cancellationToken).ConfigureAwait(false);

		// DataSourceId is no longer sent, but needed for backups.  Re-add.
		foreach (var item in dataSourceOverviewGraphsPageAsync.Items)
		{
			item.DataSourceId = dataSourceId;
		}

		return dataSourceOverviewGraphsPageAsync;
	}

	/// <summary>
	///     Gets a DataSource overview graph given dataSourceId and overviewGraphId
	/// </summary>
	/// <param name="dataSourceId">The DataSource Id</param>
	/// <param name="overviewGraphId">The overview graph Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DataSourceGraph> GetDataSourceOverviewGraphAsync(
		int dataSourceId,
		int overviewGraphId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DataSourceGraph>($"setting/datasources/{dataSourceId}/ographs/{overviewGraphId}", cancellationToken);

	/// <summary>
	///     Gets a DataSource's dataPoints given the DataSourceId
	/// </summary>
	/// <param name="dataSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DataPoint>> GetDataSourceDataPointsPageAsync(
		int dataSourceId,
		Filter<DataPoint> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DataPoint>>($"setting/datasources/{dataSourceId}/datapoints?{filter}", cancellationToken);

	/// <summary>
	/// Gets a page of Device DataSources
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetAllDeviceDataSourcesAsync instead", false)]
	public Task<Page<ResourceDataSource>> GetDeviceDataSourcesPageAsync(
		int resourceId,
		Filter<ResourceDataSource> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<ResourceDataSource>>($"device/devices/{resourceId}/devicedatasources?{filter}", cancellationToken);

	/// <summary>
	/// Gets all DeviceDataSources that match a filter
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceDataSource>> GetAllDeviceDataSourcesAsync(
		int resourceId,
		Filter<ResourceDataSource>? filter,
		CancellationToken cancellationToken)
		=> GetAllAsync(filter, $"device/devices/{resourceId}/devicedatasources", cancellationToken);

	/// <summary>
	///     Gets the deviceDataSource
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceDataSource> GetDeviceDataSourceAsync(
		int resourceId,
		int resourceDataSourceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ResourceDataSource>($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}", cancellationToken);

	/// <summary>
	///     Gets a page of device DataSource groups
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ResourceDataSourceGroup>> GetDeviceDataSourceGroupsPageAsync(
		int resourceId,
		int resourceDataSourceId,
		Filter<ResourceDataSourceGroup> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<ResourceDataSourceGroup>>($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/groups?{filter}", cancellationToken);

	/// <summary>
	///     Gets a page of DataSourceInstances
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetAllDeviceDataSourceInstancesAsync() instead", true)]
	public Task<Page<ResourceDataSourceInstance>> GetDeviceDataSourceInstancesPageAsync(
		int resourceId,
		int resourceDataSourceId,
		Filter<ResourceDataSourceInstance> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances", filter, cancellationToken);

	/// <summary>
	///     Gets a list of DataSourceInstances
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId"></param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<InstanceProperty>> GetDeviceDataSourceInstancePropertiesPageAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		Filter<InstanceProperty> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/properties", filter, cancellationToken);

	/// <summary>
	///     Get all DeviceDataSourceInstances given a Device id and DeviceDataSource id
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceDataSourceInstance>> GetAllDeviceDataSourceInstancesAsync(
		int resourceId,
		int resourceDataSourceId,
		CancellationToken cancellationToken)
		=> GetAllDeviceDataSourceInstancesAsync(resourceId, resourceDataSourceId, new(), cancellationToken);

	/// <summary>
	///     Get all DeviceDataSourceInstances given a Device id and DeviceDataSource id
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="filter">The optional filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<ResourceDataSourceInstance>> GetAllDeviceDataSourceInstancesAsync(
		int resourceId,
		int resourceDataSourceId,
		Filter<ResourceDataSourceInstance> filter,
		CancellationToken cancellationToken)
	{
		filter.Take = 1000; // LogicMonitor limitation as of 2020-02-12
		filter.Skip = 0;

		var items = new List<ResourceDataSourceInstance>();
		while (true)
		{
			var itemsThisTime = await FilteredGetAsync($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances", filter, cancellationToken)
				.ConfigureAwait(false);
			items.AddRange(itemsThisTime.Items);
			if (itemsThisTime.Items.Count < filter.Take)
			{
				if (StrictPagingTotalChecking && itemsThisTime.TotalCount != items.Count)
				{
					throw new PagingException($"Mismatch between API declared total: {itemsThisTime.TotalCount} and received count: {items.Count}");
				}

				return items;
			}

			filter.Skip += filter.Take;
		}
	}

	/// <summary>
	///     Get all DeviceDataSourceInstances given a Device id
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceDataSourceInstance>> GetAllDeviceDataSourceInstancesAsync(
		int resourceId,
		CancellationToken cancellationToken)
		=> GetAllDeviceDataSourceInstancesAsync(resourceId, new Filter<ResourceDataSourceInstance>(), cancellationToken);

	/// <summary>
	///     Get all DeviceDataSourceInstances given a Device id
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<ResourceDataSourceInstance>> GetAllDeviceDataSourceInstancesAsync(
		int resourceId,
		Filter<ResourceDataSourceInstance> filter,
		CancellationToken cancellationToken)
	{
		filter.Take = 1000; // LogicMonitor limitation as of 2020-02-12
		filter.Skip = 0;

		var items = new List<ResourceDataSourceInstance>();
		while (true)
		{
			var itemsThisTime = await FilteredGetAsync($"device/devices/{resourceId}/instances", filter, cancellationToken)
				.ConfigureAwait(false);
			items.AddRange(itemsThisTime.Items);
			if (itemsThisTime.Items.Count < filter.Take)
			{
				if (StrictPagingTotalChecking && itemsThisTime.TotalCount != items.Count)
				{
					throw new PagingException($"Mismatch between API declared total: {itemsThisTime.TotalCount} and received count: {items.Count}");
				}

				return items;
			}

			filter.Skip += filter.Take;
		}
	}

	/// <summary>
	/// Obsolete
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="resourceDataSourceInstanceId"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetAllResourceDataSourceInstancePropertiesAsync instead", true)]
	public Task<List<InstanceProperty>> GetAllDeviceDataSourceInstancePropertiesAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		Filter<InstanceProperty> filter,
		CancellationToken cancellationToken)
		=> GetAllResourceDataSourceInstancePropertiesAsync(
			resourceId,
			resourceDataSourceId,
			resourceDataSourceInstanceId,
			filter,
			cancellationToken);

	/// <summary>
	///     GetAllDeviceDataSourceInstanceProperties
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId"></param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<InstanceProperty>> GetAllResourceDataSourceInstancePropertiesAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		Filter<InstanceProperty> filter,
		CancellationToken cancellationToken)
	{
		filter.Take = 50; // LogicMonitor hard-coded value
		filter.Skip = 0;

		var items = new List<InstanceProperty>();
		while (true)
		{
			var itemsThisTime = await GetDeviceDataSourceInstancePropertiesPageAsync(resourceId, resourceDataSourceId, resourceDataSourceInstanceId, filter, cancellationToken).ConfigureAwait(false);
			if (itemsThisTime.Items.Count == 0)
			{
				return items;
			}

			items.AddRange(itemsThisTime.Items);
			filter.Skip += filter.Take;
		}
	}

	/// <summary>
	/// Gets a minimal amount of information about all LogicModule instances within a ResourceGroup
	/// </summary>
	public async Task<List<ResourceDataSourceInstance>> GetInstancesAsync(
		LogicModuleType logicModuleType,
		int rootResourceGroupId,
		List<int> logicModuleIds,
		string? instanceProperty,
		Regex? instancePropertyValueRegex,
		Filter<InstanceProperty> filter,
		CancellationToken cancellationToken)
	{
		// TODO - Move this inline and support other LogicModule types
		switch (logicModuleType)
		{
			case LogicModuleType.DataSource:
				break;
			default:
				throw new NotSupportedException($"LogicModuleType {logicModuleType} not yet supported.");
		}

		var resourceGroupFilter = new Filter<ResourceGroup>
		{
			Properties = [
				nameof(ResourceGroup.Id),
				nameof(ResourceGroup.SubGroups),
				nameof(ResourceGroup.Resources)
			]
		};

		var instanceFilter = new Filter<ResourceDataSourceInstance>
		{
			Properties = [
				nameof(ResourceDataSourceInstance.Id),
				nameof(ResourceDataSourceInstance.DataSourceId),
				nameof(ResourceDataSourceInstance.ResourceId)
			]
		};

		var appliesToResourceIds = new Dictionary<int, List<int>>();
		foreach (var logicModuleId in logicModuleIds)
		{
			// TODO - support more than just this LogicModuleType
			var appliesToFunction = (await GetAsync<DataSource>(logicModuleId, cancellationToken).ConfigureAwait(false)).AppliesTo;
			appliesToResourceIds[logicModuleId] = (await GetAppliesToAsync(appliesToFunction, cancellationToken).ConfigureAwait(false)).ConvertAll(atr => atr.Id);
		}

		// Get a list of resourceGroupIds
		var resourceGroupIds = new List<int> { rootResourceGroupId };
		var resourceDataSourceInstances = new List<ResourceDataSourceInstance>();

		for (var listIndex = 0; listIndex < resourceGroupIds.Count; listIndex++)
		{
			// Add the child ResourceGroup Ids to the list
			var resourceGroup = await GetAsync(resourceGroupIds[listIndex], resourceGroupFilter, cancellationToken).ConfigureAwait(false);
			resourceGroupIds.AddRange(resourceGroup.SubGroups.Select(subGroup => subGroup.Id));

			var resourceGroupResourceIds = (await GetResourcesByResourceGroupIdAsync(resourceGroup.Id, new Filter<Resource>
			{
				Properties = [nameof(Resource.Id)]
			}, cancellationToken).ConfigureAwait(false))
				.ConvertAll(d => d.Id)
;

			// Iterate over all devices where the appliesTo matches
			foreach (var logicModuleId in logicModuleIds)
			{
				var resourceIds = appliesToResourceIds[logicModuleId];
				foreach (var deviceId in resourceGroupResourceIds.Where(id => resourceIds.Contains(id)))
				{
					var resourceDataSource = await GetResourceDataSourceByResourceIdAndDataSourceIdAsync(deviceId, logicModuleId, cancellationToken).ConfigureAwait(false);

					if (instanceProperty is null)
					{
						resourceDataSourceInstances.AddRange(await GetAllDeviceDataSourceInstancesAsync(deviceId, resourceDataSource.Id, instanceFilter, cancellationToken).ConfigureAwait(false));
					}
					else
					{
						var thisDeviceDataSourceInstances = await GetAllDeviceDataSourceInstancesAsync(deviceId, resourceDataSource.Id, instanceFilter, cancellationToken).ConfigureAwait(false);
						foreach (var deviceDataSourceInstance in thisDeviceDataSourceInstances)
						{
							var instanceCustomProperties = await GetAllResourceDataSourceInstancePropertiesAsync(deviceId, resourceDataSource.Id, deviceDataSourceInstance.Id, filter, cancellationToken).ConfigureAwait(false);
							if (instancePropertyValueRegex is not null)
							{
								if (!instanceCustomProperties.Any(cp => cp.Name == instanceProperty && instancePropertyValueRegex.IsMatch(cp.Value)))
								{
									continue;
								}
							}

							resourceDataSourceInstances.Add(deviceDataSourceInstance);
						}
					}
				}
			}
		}

		return resourceDataSourceInstances;
	}

	/// <summary>
	/// Obsolete
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="resourceDataSourceInstanceId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetResourceDataSourceInstanceAsync instead", true)]
	public Task<ResourceDataSourceInstance> GetDeviceDataSourceInstanceAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		CancellationToken cancellationToken)
		=> GetResourceDataSourceInstanceAsync(resourceId, resourceDataSourceId, resourceDataSourceInstanceId, cancellationToken);

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
	public Task<List<ResourceDataSourceInstanceGroup>> GetDeviceDataSourceInstanceGroupsAsync(
		int resourceId,
		int resourceDataSourceId,
		CancellationToken cancellationToken)
		=> GetAllAsync(
			filter: new Filter<ResourceDataSourceInstanceGroup>
			{
				FilterItems =
				[
					new Eq<ResourceDataSourceInstanceGroup>(nameof(ResourceDataSourceInstanceGroup.DeviceDataSourceId), resourceDataSourceId)
				]
			},
			subUrl: $"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/groups/",
			cancellationToken);

	// DO NOT use this one - it only appears to return 50 items!
	//=> (await GetBySubUrlAsync<Page<DeviceDataSourceInstanceGroup>>(
	//	$"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/groups", cancellationToken).ConfigureAwait(false)).Items;

	/// <summary>
	///     Get DataSource Instance Groups
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="name">The device dataSource instanceGroup name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceDataSourceInstanceGroup> GetDeviceDataSourceInstanceGroupByNameAsync(
		int resourceId,
		int resourceDataSourceId,
		string name,
		CancellationToken cancellationToken)
		=> (await GetDeviceDataSourceInstanceGroupsAsync(resourceId, resourceDataSourceId, cancellationToken).ConfigureAwait(false)).SingleOrDefault(ig => ig.Name == name);

	/// <summary>
	///     Get DataSource Instance Group details
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="resourceDataSourceInstanceGroupId"></param>
	/// <param name="sendNullIfError"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ResourceDataSourceInstanceGroup?> GetDeviceDataSourceInstanceGroupAsync(
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
	public Task<Page<ResourceDataSourceInstance>> GetDeviceDataSourceInstanceGroupInstancesPageAsync(
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
	/// Obsolete
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetAllResourceGroupDataSourcesAsync instead", true)]
	public Task<List<ResourceGroupDataSource>> GetAllDeviceGroupDataSourcesAsync(
		int resourceGroupId,
		CancellationToken cancellationToken)
		=> GetAllResourceGroupDataSourcesAsync(resourceGroupId, cancellationToken);

	/// <summary>
	/// Obsolete
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="resourceDataSourceInstanceId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetResourceDataSourceInstanceDataPointConfigurationsAsync instead", true)]
	public Task<List<DataPointConfiguration>> GetDeviceDataSourceInstanceDataPointConfigurationsAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		CancellationToken cancellationToken)
		=> GetResourceDataSourceInstanceDataPointConfigurationsAsync(resourceId, resourceDataSourceId, resourceDataSourceInstanceId, cancellationToken);

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
	public Task<List<ResourceDataSourceInstanceConfig>> GetAllDeviceDataSourceInstanceConfigsAsync(
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
	public Task<DataPointConfiguration> GetSingleDeviceDataSourceInstanceDataPointConfigurationAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		int dataPointId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DataPointConfiguration>(
			$"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/alertsettings/{dataPointId}",
			cancellationToken);

	/// <summary>
	/// Obsolete
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="resourceDataSourceInstanceId"></param>
	/// <param name="dataPointId"></param>
	/// <param name="dataPointConfiguration"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use SetResourceDataSourceInstanceDataPointConfigurationAsync instead", true)]
	public Task SetSingleDeviceDataSourceInstanceDataPointConfigurationAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		int dataPointId,
		DataPointConfiguration dataPointConfiguration,
		CancellationToken cancellationToken)
		=> SetResourceDataSourceInstanceDataPointConfigurationAsync(
			resourceId,
			resourceDataSourceId,
			resourceDataSourceInstanceId,
			dataPointId,
			dataPointConfiguration,
			cancellationToken);

	/// <summary>
	/// Sets alert thresholds for an entire device datasource instance group
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The DeviceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId">The deviceDataSource Instance Group Id (0 == default)</param>
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
	/// <param name="resourceDataSourceId">The DeviceDataSource Id</param>
	/// <param name="resourceDataSourceInstanceId">The DeviceDataSourceInstance Id</param>
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
	/// Obsolete
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="dataSourceId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetResourceDataSourceByDeviceIdAndDataSourceIdAsync instead", true)]
	public Task<ResourceDataSource> GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(
		int resourceId,
		int dataSourceId,
		CancellationToken cancellationToken)
		=> GetResourceDataSourceByResourceIdAndDataSourceIdAsync(
			resourceId,
			dataSourceId,
			cancellationToken);

	/// <summary>
	///     Gets a ResourceDataSource
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="dataSourceId"></param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public async Task<ResourceDataSource> GetResourceDataSourceByResourceIdAndDataSourceIdAsync(
	int resourceId,
	int dataSourceId,
	CancellationToken cancellationToken)
	{
		var deviceDataSources = await GetAllDeviceDataSourcesAsync(resourceId, new Filter<ResourceDataSource>
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
	public Task<ResourceDataSourceInstance> AddDeviceDataSourceInstanceAsync(
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
	public Task<object> CollectDeviceConfigSourceConfig(
		int resourceId,
		int resourceDataSourceId,
		int instanceId,
		CancellationToken cancellationToken)
		=> PostAsync<object?, object>(null, $"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{instanceId}/config/configCollection", cancellationToken);

	/// <summary>
	/// Get DataSource list
	/// </summary>
	/// <param name="filter"></param>
	[Obsolete("Use GetAllAsync<DataSource> instead.", true)]
	public Task<Page<DataSource>> GetDatasourceListAsync(Filter<DataSource> filter)
		=> GetDataSourceListAsync(filter, CancellationToken.None);

	/// <summary>
	/// get DataSource list
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	[Obsolete("Use GetAllAsync<DataSource> instead.", true)]
	public Task<Page<DataSource>> GetDatasourceListAsync(
		Filter<DataSource> filter,
		CancellationToken cancellationToken)
		=> GetDataSourceListAsync(filter, cancellationToken);

	/// <summary>
	/// Get a list of DataSources
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetAllAsync<DataSource> instead.", true)]
	public Task<Page<DataSource>> GetDataSourceListAsync(
		Filter<DataSource> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"setting/datasources", filter, cancellationToken);

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
}
