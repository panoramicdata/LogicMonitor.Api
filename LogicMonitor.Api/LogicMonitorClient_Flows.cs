namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///    Get Flows using a FlowRequest
	/// </summary>
	/// <param name="flowRequest"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<Flow>> GetFlowsPageAsync(FlowRequest flowRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<Flow>>(flowRequest.GetQueryString(), cancellationToken);

	/// <summary>
	///    Get Flows using a FlowPortRequest
	/// </summary>
	/// <param name="flowPortsRequest"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<NetflowPort>> GetFlowPortsPageAsync(FlowPortsRequest flowPortsRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<NetflowPort>>(flowPortsRequest.GetQueryString(), cancellationToken);

	/// <summary>
	///    Get Flows using a FlowPortRequest
	/// </summary>
	/// <param name="flowApplicationsRequest"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<FlowApplication>> GetFlowApplicationsPageAsync(FlowApplicationsRequest flowApplicationsRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowApplication>>(flowApplicationsRequest.GetQueryString(), cancellationToken);

	/// <summary>
	///    Gets a Resource's flow information
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<FlowInterface>> GetResourceFlowInterfacesPageAsync(
		int resourceId,
		Filter<FlowInterface> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowInterface>>($"device/devices/{resourceId}/interfaces?{filter}", cancellationToken);

	/// <summary>
	/// get netflow flows
	/// </summary>
	/// <param name="id"></param>
	public Task<Page<NetFlowRecord>> GetNetflowFlowPageAsync(
		int id) => GetNetflowFlowPageAsync(id, CancellationToken.None);

	/// <summary>
	/// get netflow flows
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<NetFlowRecord>> GetNetflowFlowPageAsync(
		int id,
		CancellationToken cancellationToken
		) => GetBySubUrlAsync<Page<NetFlowRecord>>($"device/devices/{id}/flows", cancellationToken);

	#region ResourceGroup flows

	/// <summary>
	///  Get Flows for a ResourceGroup using a FlowRequest
	/// </summary>
	/// <param name="flowRequest"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<Flow>> GetResourceGroupFlowsPageAsync(
		ResourceGroupFlowRequest flowRequest,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<Flow>>(flowRequest.GetQueryString(), cancellationToken);

	/// <summary>
	/// Get a ResourceGroup's application flows. This only ever appears to return 10 items
	/// </summary>
	/// <param name="flowRequest"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<FlowApplication>> GetResourceGroupFlowApplicationsPageAsync(
		ResourceGroupFlowApplicationsRequest flowRequest,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowApplication>>(flowRequest.GetQueryString(), cancellationToken);

	/// <summary>
	/// Get a ResourceGroup's bandwidth flows. This only ever appears to return 1 item
	/// </summary>
	/// <param name="flowRequest"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<NetflowBandwidth>> GetResourceGroupFlowBandwidthsPageAsync(
		ResourceGroupFlowBandwidthsRequest flowRequest,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<NetflowBandwidth>>(flowRequest.GetQueryString(), cancellationToken);

	#endregion
}
