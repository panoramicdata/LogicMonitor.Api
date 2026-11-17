namespace LogicMonitor.Api;

/// <summary>
///     DataSource portal interaction
/// </summary>
public partial class LogicMonitorClient
{

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
