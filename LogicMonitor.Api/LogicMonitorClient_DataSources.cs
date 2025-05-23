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
						resourceDataSourceInstances.AddRange(await GetAllResourceDataSourceInstancesAsync(deviceId, resourceDataSource.Id, instanceFilter, cancellationToken).ConfigureAwait(false));
					}
					else
					{
						var thisResourceDataSourceInstances = await GetAllResourceDataSourceInstancesAsync(deviceId, resourceDataSource.Id, instanceFilter, cancellationToken).ConfigureAwait(false);
						foreach (var resourceDataSourceInstance in thisResourceDataSourceInstances)
						{
							var instanceCustomProperties = await GetAllResourceDataSourceInstancePropertiesAsync(deviceId, resourceDataSource.Id, resourceDataSourceInstance.Id, filter, cancellationToken).ConfigureAwait(false);
							if (instancePropertyValueRegex is not null)
							{
								if (!instanceCustomProperties.Any(cp => cp.Name == instanceProperty && instancePropertyValueRegex.IsMatch(cp.Value)))
								{
									continue;
								}
							}

							resourceDataSourceInstances.Add(resourceDataSourceInstance);
						}
					}
				}
			}
		}

		return resourceDataSourceInstances;
	}

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
	public async Task<ResourceDataSourceInstanceGroup> GetResourceDataSourceInstanceGroupByNameAsync(
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
	public async Task<ResourceDataSource> GetResourceDataSourceByResourceIdAndDataSourceIdAsync(
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
}
