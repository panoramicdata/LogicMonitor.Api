namespace LogicMonitor.Api.Flows
{
	/// <summary>
	///    A request for flow applications (for a Device Group).
	///    This is basically the Top Talkers as shown on the Network tab of a device group
	/// </summary>
	public class DeviceGroupFlowApplicationsRequest : DeviceGroupSortedFlowRequest
	{
		/// <summary>
		///    Gets the query string
		/// </summary>
		/// <returns></returns>
		public override string GetQueryString()
			=> $"device/groups/{DeviceGroupId}/netflow/applications?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}&direction={FlowDirection.ToString().ToLowerInvariant()}{GetTimePartialQueryStringNew()}&size={Take}&offset={Skip}&qosType={QosType}";
	}
}