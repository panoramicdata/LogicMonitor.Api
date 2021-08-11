namespace LogicMonitor.Api.Flows
{
	/// <summary>
	/// A request for flow data for a Device Group
	/// </summary>
	public class DeviceGroupFlowsRequest : DeviceGroupFlowRequest
	{
		/// <summary>
		/// The flow field to sort by
		/// </summary>
		public FlowField SortFlowField { get; set; }

		/// <summary>
		/// IRequest Gets Query String
		/// </summary>
		public override string GetQueryString()
			=> $"device/groups/{DeviceGroupId}/netflow/flows?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}{GetTimePartialQueryStringNew()}&size={Take}&offset={Skip}&qosType={QosType}";
	}
}