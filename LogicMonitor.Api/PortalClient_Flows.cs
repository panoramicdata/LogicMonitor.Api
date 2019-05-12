using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Flows;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	public partial class PortalClient
	{
		/// <summary>
		///    Get Flows using a FlowRequest
		/// </summary>
		/// <param name="flowRequest"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<Flow>> GetFlows(FlowRequest flowRequest, CancellationToken cancellationToken = default)
			=> GetBySubUrlAsync<Page<Flow>>(flowRequest.GetQueryString(), cancellationToken);

		/// <summary>
		///    Get Flows using a FlowPortRequest
		/// </summary>
		/// <param name="flowPortsRequest"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<FlowPort>> GetFlowPortsPageAsync(FlowPortsRequest flowPortsRequest, CancellationToken cancellationToken = default)
			=> GetBySubUrlAsync<Page<FlowPort>>(flowPortsRequest.GetQueryString(), cancellationToken);

		/// <summary>
		///    Get Flows using a FlowPortRequest
		/// </summary>
		/// <param name="flowEndpointsRequest"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<FlowEndpoint>> GetFlowEndpointsPageAsync(FlowEndpointsRequest flowEndpointsRequest, CancellationToken cancellationToken = default)
			=> GetBySubUrlAsync<Page<FlowEndpoint>>(flowEndpointsRequest.GetQueryString(), cancellationToken);

		/// <summary>
		///    Get Flows using a FlowPortRequest
		/// </summary>
		/// <param name="flowApplicationsRequest"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<FlowApplication>> GetFlowApplicationsPageAsync(FlowApplicationsRequest flowApplicationsRequest, CancellationToken cancellationToken = default)
			=> GetBySubUrlAsync<Page<FlowApplication>>(flowApplicationsRequest.GetQueryString(), cancellationToken);

		/// <summary>
		///    Gets a device's flow information
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="filter">The filter</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns></returns>
		public Task<Page<FlowInterface>> GetDeviceFlowInterfacesPageAsync(int deviceId, Filter<FlowInterface> filter, CancellationToken cancellationToken = default)
		{
			ValidateFilter(filter);
			return GetBySubUrlAsync<Page<FlowInterface>>($"device/devices/{deviceId}/interfaces?{filter}", cancellationToken);
		}
	}
}