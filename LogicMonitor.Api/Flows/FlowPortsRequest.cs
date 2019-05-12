namespace LogicMonitor.Api.Flows
{
	/// <summary>
	///    A request for flow ports.
	/// </summary>
	public class FlowPortsRequest : SortedFlowRequest
	{
		/// <summary>
		///    Gets the query string
		/// </summary>
		/// <returns></returns>
		public override string GetQueryString()
			=>
			$"device/devices/{DeviceId}/ports?sort={(SortDirection == SortDirection.Ascending ? string.Empty : "-") + SortFlowField.ToString().ToLowerInvariant()}&direction={FlowDirection.ToString().ToLowerInvariant()}{GetTimePartialQueryStringNew()}&netflowFilter={NetflowFilter.AsUrlEncodedString()}&size={Take}&offset={Skip}";
	}
}