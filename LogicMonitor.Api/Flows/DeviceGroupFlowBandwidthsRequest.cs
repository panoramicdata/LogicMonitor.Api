namespace LogicMonitor.Api.Flows;

/// <summary>
///    A request for flow bandwidths (for a Device Group).
/// </summary>
public class DeviceGroupFlowBandwidthsRequest : DeviceGroupSortedFlowRequest
{
	/// <summary>
	///    Gets the query string
	/// </summary>
	public override string GetQueryString()
		=> $"device/groups/{DeviceGroupId}/netflow/bandwidths?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}&direction={FlowDirection.ToString().ToLowerInvariant()}{GetTimePartialQueryString()}&size={Take}&offset={Skip}&qosType={QosType}";
}
