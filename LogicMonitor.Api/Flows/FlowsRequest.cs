namespace LogicMonitor.Api.Flows;

/// <summary>
/// A request for flow data
/// </summary>
public class FlowsRequest : FlowRequest
{
	/// <summary>
	/// The flow field to sort by
	/// </summary>
	public FlowField SortFlowField { get; set; }

	/// <summary>
	/// IRequest Gets Query String
	/// </summary>
	public override string GetQueryString() => $"device/devices/{DeviceId}/flows?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}&netflowFilter={NetflowFilter.AsUrlEncodedString()}&size={Take}&offset={Skip}";
}
