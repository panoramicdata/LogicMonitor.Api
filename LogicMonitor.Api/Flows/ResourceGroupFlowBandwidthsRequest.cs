namespace LogicMonitor.Api.Flows;

/// <summary>
///    A request for flow bandwidths (for a ResourceGroup).
/// </summary>
public class ResourceGroupFlowBandwidthsRequest : ResourceGroupSortedFlowRequest
{
	/// <summary>
	///    Gets the query string
	/// </summary>
	public override string GetQueryString()
		=> $"device/groups/{ResourceGroupId}/netflow/bandwidths?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}&direction={FlowDirection.ToString().ToLowerInvariant()}{GetTimePartialQueryString()}&size={Take}&offset={Skip}&qosType={QosType}";
}
