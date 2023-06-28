using LogicMonitor.Api.Topologies;

namespace LogicMonitor.Api;

/// <summary>
///    Websites Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///    Gets the current portal's version.
	/// </summary>
	/// <param name="dataRequest"></param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public async Task<TopologyData> GetTopologyDataAsync(
		TopologyDataRequest dataRequest,
		CancellationToken cancellationToken)
	{
		var response = await GetBySubUrlAsync<TopologyData>($"topology/data?{dataRequest.GetQueryString()}", cancellationToken)
			.ConfigureAwait(false);

		return response;
	}

	/// <summary>
	/// Get a topology group using its id
	/// </summary>
	public async Task<TopologyGroup> GetTopologyGroupAsync(
		int id,
		CancellationToken cancellationToken)
		=> await GetBySubUrlAsync<TopologyGroup>($"topology/groups/{id}", cancellationToken);

	/// <summary>
	/// Get a topology group using its id
	/// </summary>
	public async Task<Page<Topology>> GetTopologiesFromGroupAsync(
		int id,
		CancellationToken cancellationToken)
		=> await GetBySubUrlAsync<Page<Topology>>($"topology/groups/{id}/topologies", cancellationToken);
}
