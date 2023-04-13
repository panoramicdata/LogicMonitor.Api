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
}
