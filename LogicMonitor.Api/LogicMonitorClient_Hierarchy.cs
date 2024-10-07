using LogicMonitor.Api.Hierarchy;

namespace LogicMonitor.Api;

/// <summary>
///     Device Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	/// Get unmonitored device list
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="dataSourceId"></param>
	/// <param name="cancellationToken"></param>
	public Task<HierarchyResponse> GetResourceDataSourceInstanceSummaryAsync(
		int resourceId,
		int dataSourceId,
		CancellationToken cancellationToken)
		=> PostAsync<HierarchyRequest, HierarchyResponse>(
			new HierarchyRequest
			{
				Meta = new HierarchyRequestMeta
				{
					ResourceId = new HierarchyTypeAndId
					{
						Type = "resources",
						Id = resourceId.ToString(CultureInfo.InvariantCulture)
					},
					DataSourceId = new HierarchyTypeAndId
					{
						Type = "dataSources",
						Id = dataSourceId.ToString(CultureInfo.InvariantCulture)
					},
					Sort = "displayName"
				},
			},
			"hierarchy/dataSourceInstances",
			cancellationToken);
}
