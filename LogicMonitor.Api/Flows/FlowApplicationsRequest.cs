namespace LogicMonitor.Api.Flows;

/// <summary>
///    A request for flow applications.
/// </summary>
public class FlowApplicationsRequest : SortedFlowRequest
{
	/// <summary>
	///    Gets the query string
	/// </summary>
	public override string GetQueryString()
		=>
		$"device/devices/{DeviceId}/topTalkers" +
		$"?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}" +
		$"&netflowFilter={NetflowFilter.AsUrlEncodedString()}" +
		$"&size={Take}&offset={Skip}" +
		GetTimePartialQueryString();
}
