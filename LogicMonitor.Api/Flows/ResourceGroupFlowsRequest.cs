namespace LogicMonitor.Api.Flows;

/// <summary>
/// A request for flow data for a ResourceGroup
/// </summary>
public class ResourceGroupFlowsRequest : ResourceGroupFlowRequest
{
	/// <summary>
	/// The flow field to sort by
	/// </summary>
	public FlowField SortFlowField { get; set; }

	/// <summary>
	/// IRequest Gets Query String
	/// </summary>
	public override string GetQueryString()
		=> $"device/groups/{ResourceGroupId}/netflow/flows?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}{GetTimePartialQueryString()}&size={Take}&offset={Skip}&qosType={QosType}";
}
