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
}
