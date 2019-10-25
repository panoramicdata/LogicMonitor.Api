namespace LogicMonitor.Api.Flows
{
	/// <summary>
	/// A sorted flow request
	/// </summary>
	public abstract class DeviceGroupSortedFlowRequest : DeviceGroupFlowRequest
	{
		/// <summary>
		/// The flow field to sort by
		/// </summary>
		public FlowField SortFlowField { get; set; }
	}
}