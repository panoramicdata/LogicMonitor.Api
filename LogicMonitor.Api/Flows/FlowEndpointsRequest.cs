namespace LogicMonitor.Api.Flows;

/// <summary>
///    A request for flow endpoints.
/// </summary>
public class FlowEndpointsRequest : SortedFlowRequest
{
	/// <summary>
	///    Gets the query string
	/// </summary>
	public override string GetQueryString() => $"device/devices/{ResourceId}/endpoints?filter=type:\"destination\"&sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}&netflowFilter={NetflowFilter.AsUrlEncodedString()}&size={Take}&offset={Skip}";
}
