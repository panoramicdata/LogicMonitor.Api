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
	///    Gets a device's flow information
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<FlowInterface>> GetDeviceFlowInterfacesPageAsync(int deviceId, Filter<FlowInterface> filter, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<FlowInterface>>($"device/devices/{deviceId}/interfaces?{filter}", cancellationToken);

	/// <summary>
	/// get netflow flows
	/// </summary>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <param name="netflowFilter"></param>
	/// <param name="id"></param>
	/// <param name="fields"></param>
	/// <param name="filter"></param>
	/// <param name="size"></param>
	/// <param name="offset"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<NetFlowRecord>> GetNetflowFlowPageAsync(
		long start,
		long end,
		string netflowFilter,
		int id,
		string fields,
		string filter,
		int size = 50,
		int offset = 0,
		CancellationToken cancellationToken = default
		) => GetBySubUrlAsync<Page<NetFlowRecord>>($"device/devices/{id}/flows", cancellationToken);

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
	public Task<Page<NetflowBandwidth>> GetDeviceGroupFlowBandwidthsPageAsync(DeviceGroupFlowBandwidthsRequest flowRequest, CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<NetflowBandwidth>>(flowRequest.GetQueryString(), cancellationToken);

	#endregion
}
