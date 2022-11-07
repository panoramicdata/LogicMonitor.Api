namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///    Get Flows using a FlowRequest
	/// </summary>
	/// <param name="flowRequest"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<Flow>> GetFlows(FlowRequest flowRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<Flow>>(flowRequest.GetQueryString(), cancellationToken);

	/// <summary>
	///    Get Flows using a FlowPortRequest
	/// </summary>
	/// <param name="flowPortsRequest"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<FlowPort>> GetFlowPortsPageAsync(FlowPortsRequest flowPortsRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowPort>>(flowPortsRequest.GetQueryString(), cancellationToken);

	/// <summary>
	///    Get Flows using a FlowPortRequest
	/// </summary>
	/// <param name="flowEndpointsRequest"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<FlowEndpoint>> GetFlowEndpointsPageAsync(FlowEndpointsRequest flowEndpointsRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowEndpoint>>(flowEndpointsRequest.GetQueryString(), cancellationToken);

	/// <summary>
	///    Get Flows using a FlowPortRequest
	/// </summary>
	/// <param name="flowApplicationsRequest"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<FlowApplication>> GetFlowApplicationsPageAsync(FlowApplicationsRequest flowApplicationsRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowApplication>>(flowApplicationsRequest.GetQueryString(), cancellationToken);

	/// <summary>
	///    Gets a device's flow information
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<FlowInterface>> GetDeviceFlowInterfacesPageAsync(int deviceId, Filter<FlowInterface> filter, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowInterface>>($"device/devices/{deviceId}/interfaces?{filter}", cancellationToken);

	#region Device Group flows

	/// <summary>
	///  Get Flows for a Device Group using a FlowRequest
	/// </summary>
	/// <param name="flowRequest"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<Flow>> GetDeviceGroupFlowsPageAsync(DeviceGroupFlowRequest flowRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<Flow>>(flowRequest.GetQueryString(), cancellationToken);

	/// <summary>
	/// Get a Device Group's application flows. This only ever appears to return 10 items
	/// </summary>
	/// <param name="flowRequest"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<FlowApplication>> GetDeviceGroupFlowApplicationsPageAsync(DeviceGroupFlowApplicationsRequest flowRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowApplication>>(flowRequest.GetQueryString(), cancellationToken);

	/// <summary>
	/// Get a Device Group's bandwidth flows. This only ever appears to return 1 item
	/// </summary>
	/// <param name="flowRequest"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<FlowBandwidth>> GetDeviceGroupFlowBandwidthsPageAsync(DeviceGroupFlowBandwidthsRequest flowRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowBandwidth>>(flowRequest.GetQueryString(), cancellationToken);

	#endregion
}
