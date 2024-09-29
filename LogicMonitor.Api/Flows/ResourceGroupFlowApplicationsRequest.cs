namespace LogicMonitor.Api.Flows;

/// <summary>
///    A request for flow applications (for a ResourceGroup).
///    This is basically the Top Talkers as shown on the Network tab of a ResourceGroup
/// </summary>
public class ResourceGroupFlowApplicationsRequest : ResourceGroupSortedFlowRequest
{
	/// <summary>
	///    Gets the query string
	/// </summary>
	public override string GetQueryString()
		=> $"device/groups/{ResourceGroupId}/netflow/applications?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}&direction={FlowDirection.ToString().ToLowerInvariant()}{GetTimePartialQueryString()}&size={Take}&offset={Skip}&qosType={QosType}";
}
