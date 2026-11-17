namespace LogicMonitor.Api;

/// <summary>
///     DataSource portal interaction
/// </summary>
public partial class LogicMonitorClient
{
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
	public async Task<DataSourceGraph?> GetDataSourceGraphByNameAsync(
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
	public async Task<DataSourceGraph?> GetDataSourceOverviewGraphByNameAsync(
		int dataSourceId,
		string graphName,
		CancellationToken cancellationToken)
		=> (await GetDataSourceOverviewGraphsPageAsync(dataSourceId, null, cancellationToken).ConfigureAwait(false))
			.Items
			.SingleOrDefault(g => g.Name == graphName);

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
	///     Creates a graph on a DataSource.
	/// </summary>
	/// <param name="dataSourceId">The DataSource Id</param>
	/// <param name="dataSourceGraph">The graph to create</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The created graph, including its server-assigned Id.</returns>
	public Task<DataSourceGraph> CreateDataSourceGraphAsync(
		int dataSourceId,
		DataSourceGraph dataSourceGraph,
		CancellationToken cancellationToken)
		=> PostAsync<DataSourceGraph, DataSourceGraph>(dataSourceGraph, $"setting/datasources/{dataSourceId}/graphs", cancellationToken);

	/// <summary>
	///     Updates an existing graph on a DataSource.
	/// </summary>
	/// <param name="dataSourceId">The DataSource Id</param>
	/// <param name="dataSourceGraph">The graph to update (its Id identifies the graph)</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task UpdateDataSourceGraphAsync(
		int dataSourceId,
		DataSourceGraph dataSourceGraph,
		CancellationToken cancellationToken)
		=> PutAsync($"setting/datasources/{dataSourceId}/graphs/{dataSourceGraph.Id}", dataSourceGraph, cancellationToken);

	/// <summary>
	///     Deletes a graph from a DataSource.
	/// </summary>
	/// <param name="dataSourceId">The DataSource Id</param>
	/// <param name="graphId">The graph Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task DeleteDataSourceGraphAsync(
		int dataSourceId,
		int graphId,
		CancellationToken cancellationToken)
		=> DeleteAsync($"setting/datasources/{dataSourceId}/graphs/{graphId}", cancellationToken);

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
	/// Gets all ResourceDataSources that match a filter
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceDataSource>> GetAllResourceDataSourcesAsync(
		int resourceId,
		Filter<ResourceDataSource>? filter,
		CancellationToken cancellationToken)
		=> GetAllAsync(filter, $"device/devices/{resourceId}/devicedatasources", cancellationToken);

	/// <summary>
	///     Gets the ResourceDataSource
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceDataSource> GetResourceDataSourceAsync(
		int resourceId,
		int resourceDataSourceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ResourceDataSource>($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}", cancellationToken);

	/// <summary>
	///     Gets a page of Resource DataSource groups
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ResourceDataSourceGroup>> GetResourceDataSourceGroupsPageAsync(
		int resourceId,
		int resourceDataSourceId,
		Filter<ResourceDataSourceGroup> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<ResourceDataSourceGroup>>($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/groups?{filter}", cancellationToken);

	/// <summary>
	///     Gets a list of DataSourceInstances
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="resourceDataSourceInstanceId"></param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<InstanceProperty>> GetResourceDataSourceInstancePropertiesPageAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		Filter<InstanceProperty> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/properties", filter, cancellationToken);

	/// <summary>
	///     Get all ResourceDataSourceInstances given a Resource id and ResourceDataSource id
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceDataSourceInstance>> GetAllResourceDataSourceInstancesAsync(
		int resourceId,
		int resourceDataSourceId,
		CancellationToken cancellationToken)
		=> GetAllResourceDataSourceInstancesAsync(resourceId, resourceDataSourceId, new(), cancellationToken);

	/// <summary>
	///     Get all ResourceDataSourceInstances given a Resource id and ResourceDataSource id
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="resourceDataSourceId">The ResourceDataSource id</param>
	/// <param name="filter">The optional filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<ResourceDataSourceInstance>> GetAllResourceDataSourceInstancesAsync(
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
				return StrictPagingTotalChecking && itemsThisTime.TotalCount != items.Count
					? throw new PagingException($"Mismatch between API declared total: {itemsThisTime.TotalCount} and received count: {items.Count}")
					: items;
			}

			filter.Skip += filter.Take;
		}
	}

	/// <summary>
	///     Get all ResourceDataSourceInstances given a Resource id
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceDataSourceInstance>> GetAllResourceDataSourceInstancesAsync(
		int resourceId,
		CancellationToken cancellationToken)
		=> GetAllResourceDataSourceInstancesAsync(resourceId, new Filter<ResourceDataSourceInstance>(), cancellationToken);

	/// <summary>
	///     Get all ResourceDataSourceInstances given a Resource id
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<ResourceDataSourceInstance>> GetAllResourceDataSourceInstancesAsync(
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
				return StrictPagingTotalChecking && itemsThisTime.TotalCount != items.Count
					? throw new PagingException($"Mismatch between API declared total: {itemsThisTime.TotalCount} and received count: {items.Count}")
					: items;
			}

			filter.Skip += filter.Take;
		}
	}

	/// <summary>
	///     GetAllResourceDataSourceInstanceProperties
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
			var itemsThisTime = await GetResourceDataSourceInstancePropertiesPageAsync(
				resourceId,
				resourceDataSourceId,
				resourceDataSourceInstanceId,
				filter,
				cancellationToken)
				.ConfigureAwait(false);

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
		if (logicModuleType != LogicModuleType.DataSource)
		{
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

		var appliesToResourceIds = await GetAppliesToResourceIdsAsync(logicModuleIds, cancellationToken).ConfigureAwait(false);

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
				.ConvertAll(d => d.Id);

			// Iterate over all devices where the appliesTo matches
			foreach (var logicModuleId in logicModuleIds)
			{
				var resourceIds = appliesToResourceIds[logicModuleId];
				foreach (var deviceId in resourceGroupResourceIds.Where(id => resourceIds.Contains(id)))
				{
					resourceDataSourceInstances.AddRange(await GetMatchingInstancesAsync(
						deviceId,
						logicModuleId,
						instanceProperty,
						instancePropertyValueRegex,
						filter,
						cancellationToken).ConfigureAwait(false));
				}
			}
		}

		return resourceDataSourceInstances;
	}

	private async Task<Dictionary<int, List<int>>> GetAppliesToResourceIdsAsync(List<int> logicModuleIds, CancellationToken cancellationToken)
	{
		var appliesToResourceIds = new Dictionary<int, List<int>>();
		foreach (var logicModuleId in logicModuleIds)
		{
			// TODO - support more than just this LogicModuleType
			var appliesToFunction = (await GetAsync<DataSource>(logicModuleId, cancellationToken).ConfigureAwait(false)).AppliesTo;
			appliesToResourceIds[logicModuleId] = (await GetAppliesToAsync(appliesToFunction, cancellationToken).ConfigureAwait(false)).ConvertAll(atr => atr.Id);
		}

		return appliesToResourceIds;
	}

	private async Task<List<ResourceDataSourceInstance>> GetMatchingInstancesAsync(
		int resourceId,
		int logicModuleId,
		string? instanceProperty,
		Regex? instancePropertyValueRegex,
		Filter<InstanceProperty> filter,
		CancellationToken cancellationToken)
	{
		var instanceFilter = new Filter<ResourceDataSourceInstance>
		{
			Properties = [
				nameof(ResourceDataSourceInstance.Id),
				nameof(ResourceDataSourceInstance.DataSourceId),
				nameof(ResourceDataSourceInstance.ResourceId)
			]
		};

		var resourceDataSource = await GetResourceDataSourceByResourceIdAndDataSourceIdAsync(resourceId, logicModuleId, cancellationToken).ConfigureAwait(false);
		var instances = await GetAllResourceDataSourceInstancesAsync(resourceId, resourceDataSource!.Id, instanceFilter, cancellationToken).ConfigureAwait(false);
		if (instanceProperty is null)
		{
			return instances;
		}

		var matchingInstances = new List<ResourceDataSourceInstance>();
		foreach (var instance in instances)
		{
			var instanceCustomProperties = await GetAllResourceDataSourceInstancePropertiesAsync(resourceId, resourceDataSource.Id, instance.Id, filter, cancellationToken).ConfigureAwait(false);
			if (instancePropertyValueRegex is null || instanceCustomProperties.Any(cp => cp.Name == instanceProperty && instancePropertyValueRegex.IsMatch(cp.Value)))
			{
				matchingInstances.Add(instance);
			}
		}

		return matchingInstances;
	}
}
