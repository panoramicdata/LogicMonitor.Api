namespace LogicMonitor.Api;

/// <summary>
///     DataSource portal interaction
/// </summary>
public partial class LogicMonitorClient
{

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
}
