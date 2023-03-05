namespace LogicMonitor.Api.Flows;

/// <summary>
/// A flow request (abstract)
/// </summary>
public abstract class DeviceGroupFlowRequest : TimeBasedRequest
{
	/// <summary>
	/// Constructor
	/// </summary>
	protected DeviceGroupFlowRequest()
	{
		SortDirection = SortDirection.Descending;
		Take = 100;
		Skip = 0;
	}

	/// <summary>
	/// The id of the flow Device Group
	/// </summary>
	public int DeviceGroupId { get; set; }

	/// <summary>
	/// The QoS type
	/// </summary>
	public string QosType { get; set; } = string.Empty;

	/// <summary>
	/// The number of items to skip
	/// </summary>
	public int Skip { get; set; }

	/// <summary>
	/// The flow direction
	/// </summary>
	public FlowDirection FlowDirection { get; set; }

	/// <summary>
	/// The sort direction
	/// </summary>
	public SortDirection SortDirection { get; set; }

	/// <summary>
	/// The number of items to take
	/// </summary>
	public int Take { get; set; }
}
