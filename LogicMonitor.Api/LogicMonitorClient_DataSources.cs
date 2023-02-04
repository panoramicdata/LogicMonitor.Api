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
		=> (await GetAsync(filter, cancellationToken).ConfigureAwait(false)).Items ?? new List<DataSource>();

	/// <summary>
	///     Gets a list of all graphs for a DataSource.
	/// </summary>
	/// <param name="dataSourceId"></param>
	/// <param name="cancellationToken"></param>
	public async Task<List<DataSourceOverviewGraph>> GetDataSourceGraphsAsync(
		int dataSourceId,
		CancellationToken cancellationToken)
	{
		var page = await GetBySubUrlAsync<Page<DataSourceOverviewGraph>>($"setting/datasources/{dataSourceId}/graphs", cancellationToken).ConfigureAwait(false);
		if (page.Items is null)
		{
			return new List<DataSourceOverviewGraph>();
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
	public async Task<DataSourceOverviewGraph> GetDataSourceGraphByNameAsync(
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
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/datasources/{dataSourceId}?format=xml", cancellationToken).ConfigureAwait(false))?.Content;

	/// <summary>
	///     Gets a list of all overview graphs for a DataSource.
	/// </summary>
	/// <param name="dataSourceId">The datasource id</param>
	/// <param name="graphName">The graph name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<DataSourceOverviewGraph> GetDataSourceOverviewGraphByNameAsync(
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
			Properties = new List<string>
				{
					nameof(DataSource.Id),
					nameof(DataSource.Name)
				},
			FilterItems = new List<FilterItem<DataSource>>
				{
					new FilterItem<DataSource>
					{
						Property = nameof(DataSource.Name),
						Operation = ":",
						Value = dataSourceName
					}
				}
		};

		var dataSources = await GetAllAsync(filter, cancellationToken).ConfigureAwait(false);

		return dataSources.Count switch
		{
			1 => await GetAsync<DataSource>(dataSources.Single().Id, cancellationToken).ConfigureAwait(false),
			0 => null,
			_ => throw new Exception($"Unexpected result count {dataSources.Count}"),
		};
	}

	/// <summary>
	///     Gets a list of DataSource graphs given its dataSourceId
	/// </summary>
	/// <param name="dataSourceId">The datasource Id</param>
	/// <param name="filter">Filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DataSourceOverviewGraph>> GetDataSourceGraphsPageAsync(int dataSourceId,
		Filter<DataSourceOverviewGraph> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DataSourceOverviewGraph>>($"setting/datasources/{dataSourceId}/graphs?{filter}", cancellationToken);

	/// <summary>
	///     Gets a DataSource graph given dataSourceId and graphId
	/// </summary>
	/// <param name="dataSourceId">The DataSource Id</param>
	/// <param name="graphId">The Graph Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DataSourceOverviewGraph> GetDataSourceGraphAsync(int dataSourceId, int graphId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DataSourceOverviewGraph>($"setting/datasources/{dataSourceId}/graphs/{graphId}", cancellationToken);

	/// <summary>
	///     Gets a list of DataSource graphs given its dataSourceId
	/// </summary>
	/// <param name="dataSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken"></param>
	public async Task<Page<DataSourceOverviewGraph>> GetDataSourceOverviewGraphsPageAsync(
		int dataSourceId,
		Filter<DataSourceOverviewGraph>? filter = null,
		CancellationToken cancellationToken = default)
	{
		filter ??= new Filter<DataSourceOverviewGraph> { Skip = 0, Take = 300 };

		var dataSourceOverviewGraphsPageAsync = await GetBySubUrlAsync<Page<DataSourceOverviewGraph>>($"setting/datasources/{dataSourceId}/ographs?{filter}", cancellationToken).ConfigureAwait(false);

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
	public Task<DataSourceOverviewGraph> GetDataSourceOverviewGraphAsync(
		int dataSourceId,
		int overviewGraphId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DataSourceOverviewGraph>($"setting/datasources/{dataSourceId}/ographs/{overviewGraphId}", cancellationToken);

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
	/// <param name="deviceId">The Device Id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetAllDeviceDataSourcesAsync instead", false)]
	public Task<Page<DeviceDataSource>> GetDeviceDataSourcesPageAsync(
		int deviceId,
		Filter<DeviceDataSource> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DeviceDataSource>>($"device/devices/{deviceId}/devicedatasources?{filter}", cancellationToken);

	/// <summary>
	/// Gets all DeviceDataSources that match a filter
	/// </summary>
	/// <param name="deviceId">The Device Id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<DeviceDataSource>> GetAllDeviceDataSourcesAsync(
		int deviceId,
		Filter<DeviceDataSource> filter,
		CancellationToken cancellationToken)
		=> GetAllAsync(filter, $"device/devices/{deviceId}/devicedatasources", cancellationToken);

	/// <summary>
	///     Gets the deviceDataSource
	/// </summary>
	/// <param name="deviceId">The Device Id</param>
	/// <param name="deviceDataSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DeviceDataSource> GetDeviceDataSourceAsync(
		int deviceId,
		int deviceDataSourceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DeviceDataSource>($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}", cancellationToken);

	/// <summary>
	///     Gets a page of device DataSource groups
	/// </summary>
	/// <param name="deviceId">The Device Id</param>
	/// <param name="deviceDataSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DeviceDataSourceGroup>> GetDeviceDataSourceGroupsPageAsync(
		int deviceId,
		int deviceDataSourceId,
		Filter<DeviceDataSourceGroup> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DeviceDataSourceGroup>>($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/groups?{filter}", cancellationToken);

	/// <summary>
	///     Gets a page of DataSourceInstances
	/// </summary>
	/// <param name="deviceId">The device Id</param>
	/// <param name="deviceDataSourceId">The device data source id</param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetAllDeviceDataSourceInstancesAsync() instead")]
	public Task<Page<DeviceDataSourceInstance>> GetDeviceDataSourceInstancesPageAsync(
		int deviceId,
		int deviceDataSourceId,
		Filter<DeviceDataSourceInstance> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances", filter, cancellationToken);

	/// <summary>
	///     Gets a list of DataSourceInstances
	/// </summary>
	/// <param name="deviceId">The device Id</param>
	/// <param name="deviceDataSourceId">The device data source id</param>
	/// <param name="deviceDataSourceInstanceId"></param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<InstanceProperty>> GetDeviceDataSourceInstancePropertiesPageAsync(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceId,
		Filter<InstanceProperty> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/properties", filter, cancellationToken);

	/// <summary>
	///     Get all DeviceDataSourceInstances given a Device id and DeviceDataSource id
	/// </summary>
	/// <param name="deviceId">The device Id</param>
	/// <param name="deviceDataSourceId">The device data source id</param>
	/// <param name="filter">The optional filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<DeviceDataSourceInstance>> GetAllDeviceDataSourceInstancesAsync(
		int deviceId,
		int deviceDataSourceId,
		Filter<DeviceDataSourceInstance>? filter = null,
		CancellationToken cancellationToken = default)
	{
		filter ??= new Filter<DeviceDataSourceInstance>();

		filter.Take = 1000; // LogicMonitor limitation as of 2020-02-12
		filter.Skip = 0;

		var items = new List<DeviceDataSourceInstance>();
		while (true)
		{
			var itemsThisTime = await FilteredGetAsync($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances", filter, cancellationToken)
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
	/// <param name="deviceId">The device Id</param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<DeviceDataSourceInstance>> GetAllDeviceDataSourceInstancesAsync(
		int deviceId,
		Filter<DeviceDataSourceInstance>? filter = null,
		CancellationToken cancellationToken = default)
	{
		filter ??= new Filter<DeviceDataSourceInstance>();

		filter.Take = 1000; // LogicMonitor limitation as of 2020-02-12
		filter.Skip = 0;

		var items = new List<DeviceDataSourceInstance>();
		while (true)
		{
			var itemsThisTime = await FilteredGetAsync($"device/devices/{deviceId}/instances", filter, cancellationToken)
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
	///     GetAllDeviceDataSourceInstanceProperties
	/// </summary>
	/// <param name="deviceId">The device Id</param>
	/// <param name="deviceDataSourceId">The device data source id</param>
	/// <param name="deviceDataSourceInstanceId"></param>
	/// <param name="filter">The filter to apply</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<InstanceProperty>> GetAllDeviceDataSourceInstanceProperties(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceId,
		Filter<InstanceProperty>? filter = null,
		CancellationToken cancellationToken = default)
	{
		filter ??= new Filter<InstanceProperty>();

		filter.Take = 50; // LogicMonitor hardcoded value
		filter.Skip = 0;

		var items = new List<InstanceProperty>();
		while (true)
		{
			var itemsThisTime = await GetDeviceDataSourceInstancePropertiesPageAsync(deviceId, deviceDataSourceId, deviceDataSourceInstanceId, filter, cancellationToken).ConfigureAwait(false);
			if (itemsThisTime.Items.Count == 0)
			{
				return items;
			}

			items.AddRange(itemsThisTime.Items);
			filter.Skip += filter.Take;
		}
	}

	/// <summary>
	/// Gets a minimal amount of information about all LogicModule instances within a device group
	/// </summary>
	public async Task<List<DeviceDataSourceInstance>> GetInstancesAsync(
		LogicModuleType logicModuleType,
		int rootDeviceGroupId,
		List<int> logicModuleIds,
		string instanceProperty = null,
		Regex? instancePropertyValueRegex = null,
		Filter<InstanceProperty>? filter = null,
		CancellationToken cancellationToken = default)
	{
		// TODO - Move this inline and support other LogicModule types
		switch (logicModuleType)
		{
			case LogicModuleType.DataSource:
				break;
			default:
				throw new NotSupportedException($"LogicModuleType {logicModuleType} not yet supported.");
		}

		var deviceGroupFilter = new Filter<DeviceGroup> { Properties = new List<string> { nameof(DeviceGroup.Id), nameof(DeviceGroup.SubGroups), nameof(DeviceGroup.Devices) } };
		var instanceFilter = new Filter<DeviceDataSourceInstance> { Properties = new List<string> { nameof(DeviceDataSourceInstance.Id), nameof(DeviceDataSourceInstance.DataSourceId), nameof(DeviceDataSourceInstance.DeviceId) } };

		var appliesToDeviceIds = new Dictionary<int, List<int>>();
		foreach (var logicModuleId in logicModuleIds)
		{
			// TODO - support more than just this LogicModuleType
			var appliesToFunction = (await GetAsync<DataSource>(logicModuleId, cancellationToken).ConfigureAwait(false)).AppliesTo;
			appliesToDeviceIds[logicModuleId] = (await GetAppliesToAsync(appliesToFunction, cancellationToken).ConfigureAwait(false)).ConvertAll(atr => atr.Id);
		}

		// Get a list of deviceGroupIds
		var deviceGroupIds = new List<int> { rootDeviceGroupId };
		var deviceDataSourceInstances = new List<DeviceDataSourceInstance>();

		for (var listIndex = 0; listIndex < deviceGroupIds.Count; listIndex++)
		{
			// Add the child deviceGroupIds to the list
			var deviceGroup = await GetAsync(deviceGroupIds[listIndex], deviceGroupFilter, cancellationToken).ConfigureAwait(false);
			deviceGroupIds.AddRange(deviceGroup.SubGroups.Select(subGroup => subGroup.Id));

			var deviceGroupDeviceIds = (await GetDevicesByDeviceGroupIdAsync(deviceGroup.Id, new Filter<Device>
			{
				Properties = new List<string> { nameof(Device.Id) }
			}, cancellationToken).ConfigureAwait(false))
				.ConvertAll(d => d.Id)
;

			// Iterate over all devices where the appliesTo matches
			foreach (var logicModuleId in logicModuleIds)
			{
				var deviceIds = appliesToDeviceIds[logicModuleId];
				foreach (var deviceId in deviceGroupDeviceIds.Where(id => deviceIds.Contains(id)))
				{
					var deviceDataSource = await GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(deviceId, logicModuleId, cancellationToken).ConfigureAwait(false);

					if (instanceProperty is null)
					{
						deviceDataSourceInstances.AddRange(await GetAllDeviceDataSourceInstancesAsync(deviceId, deviceDataSource.Id, instanceFilter, cancellationToken).ConfigureAwait(false));
					}
					else
					{
						var thisDeviceDataSourceInstances = await GetAllDeviceDataSourceInstancesAsync(deviceId, deviceDataSource.Id, instanceFilter, cancellationToken).ConfigureAwait(false);
						foreach (var deviceDataSourceInstance in thisDeviceDataSourceInstances)
						{
							var instanceCustomProperties = await GetAllDeviceDataSourceInstanceProperties(deviceId, deviceDataSource.Id, deviceDataSourceInstance.Id, filter, cancellationToken).ConfigureAwait(false);
							if (!instanceCustomProperties.Any(cp => cp.Name == instanceProperty && instancePropertyValueRegex.IsMatch(cp.Value)))
							{
								continue;
							}

							deviceDataSourceInstances.Add(deviceDataSourceInstance);
						}
					}
				}
			}
		}

		return deviceDataSourceInstances;
	}

	/// <summary>
	///     Gets a list of DataSourceInstances
	/// </summary>
	/// <param name="deviceId">The device Id</param>
	/// <param name="deviceDataSourceId">The device data source id</param>
	/// <param name="deviceDataSourceInstanceId">The device data source instance id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DeviceDataSourceInstance> GetDeviceDataSourceInstanceAsync(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DeviceDataSourceInstance>($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}", cancellationToken);

	/// <summary>
	///     Get DataSource Instance Groups
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="deviceDataSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<DeviceDataSourceInstanceGroup>> GetDeviceDataSourceInstanceGroupsAsync(
		int deviceId,
		int deviceDataSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<Page<DeviceDataSourceInstanceGroup>>(
			$"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/groups", cancellationToken).ConfigureAwait(false)).Items;

	/// <summary>
	///     Get DataSource Instance Groups
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="deviceDataSourceId"></param>
	/// <param name="name">The device dataSource instanceGroup name</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<DeviceDataSourceInstanceGroup> GetDeviceDataSourceInstanceGroupByNameAsync(
		int deviceId,
		int deviceDataSourceId,
		string name,
		CancellationToken cancellationToken)
		=> (await GetDeviceDataSourceInstanceGroupsAsync(deviceId, deviceDataSourceId, cancellationToken).ConfigureAwait(false)).SingleOrDefault(ig => ig.Name == name);

	/// <summary>
	///     Get DataSource Instance Group details
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="deviceDataSourceId"></param>
	/// <param name="deviceDataSourceInstanceGroupId"></param>
	/// <param name="sendNullIfError"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<DeviceDataSourceInstanceGroup?> GetDeviceDataSourceInstanceGroupAsync(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceGroupId,
		bool sendNullIfError = false,
		CancellationToken cancellationToken = default)
	{
		try
		{
			return await GetBySubUrlAsync<DeviceDataSourceInstanceGroup>(
				$"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/groups/{deviceDataSourceInstanceGroupId}",
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
	/// <param name="deviceId"></param>
	/// <param name="deviceDataSourceId"></param>
	/// <param name="deviceDataSourceInstanceGroupId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DeviceDataSourceInstance>> GetDeviceDataSourceInstanceGroupInstancesPageAsync(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceGroupId,
		Filter<DeviceDataSourceInstance> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DeviceDataSourceInstance>>(
			$"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/groups/{deviceDataSourceInstanceGroupId}/instances?{filter}",
			cancellationToken);

	/// <summary>
	///     Gets a list of devices that a datasource applies to
	/// </summary>
	/// <param name="deviceGroupId">The device group id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DataSourceAppliesToCollection>> GetDataSourceAppliesToCollectionsPageAsync(
		int deviceGroupId,
		Filter<DataSourceAppliesToCollection> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DataSourceAppliesToCollection>>(
			$"device/groups/{deviceGroupId}/datasources?{filter}",
			cancellationToken);

	/// <summary>
	///     Gets a list of DataSources that apply to a device group
	/// </summary>
	/// <param name="deviceGroupId">The device group Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<DeviceGroupDataSource>> GetAllDeviceGroupDataSourcesAsync(
		int deviceGroupId,
		CancellationToken cancellationToken)
		=> GetAllAsync<DeviceGroupDataSource>($"device/groups/{deviceGroupId}/datasources", cancellationToken);

	/// <summary>
	///     Gets a list of DataPointConfiguration for a specific device, device data source, and data source instance
	/// </summary>
	/// <param name="deviceId">The device Id</param>
	/// <param name="deviceDataSourceId">The deviceDataSource Id</param>
	/// <param name="deviceDataSourceInstanceId">The deviceDataSourceInstance Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DataPointConfiguration>> GetDeviceDataSourceInstanceDataPointConfigurationAsync(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DataPointConfiguration>>(
			$"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/alertsettings",
			cancellationToken);

	/// <summary>
	///     Gets a device data source
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="dataSourceId"></param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public async Task<DeviceDataSource> GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(int deviceId, int dataSourceId, CancellationToken cancellationToken)
	{
		var deviceDataSources = await GetAllDeviceDataSourcesAsync(deviceId, new Filter<DeviceDataSource>
		{
			FilterItems = new List<FilterItem<DeviceDataSource>>
				{
					new Eq<DeviceDataSource>(nameof(DeviceDataSource.DataSourceId), dataSourceId.ToString(CultureInfo.InvariantCulture))
				}
		}, cancellationToken).ConfigureAwait(false);
		return deviceDataSources.SingleOrDefault();
	}

	/// <summary>
	///     Adds a device data source
	/// </summary>
	/// <param name="deviceId">The device Id</param>
	/// <param name="deviceDataSourceId"></param>
	/// <param name="deviceDataSourceInstanceCreationDto"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DeviceDataSourceInstance> AddDeviceDataSourceInstanceAsync(
		int deviceId,
		int deviceDataSourceId,
		DeviceDataSourceInstanceCreationDto deviceDataSourceInstanceCreationDto,
		CancellationToken cancellationToken) => PostAsync<DeviceDataSourceInstanceCreationDto, DeviceDataSourceInstance>(
			deviceDataSourceInstanceCreationDto,
			$"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances",
			cancellationToken);

	/// <summary>
	/// add audit version
	/// </summary>
	/// <param name="id">The datasource id</param>
	/// <param name="body">The audit to be added</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DataSource> AddDatasourceAuditVersionAsync(
		int id,
		Audit body,
		CancellationToken cancellationToken) => PostAsync<Audit, DataSource>(body,
			$"setting/datasources/{id}/audit",
			cancellationToken);
}
