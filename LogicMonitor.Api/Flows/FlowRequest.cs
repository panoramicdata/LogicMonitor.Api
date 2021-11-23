using LogicMonitor.Api.Dashboards;

namespace LogicMonitor.Api.Flows;

/// <summary>
/// A flow request (abstract)
/// </summary>
public abstract class FlowRequest : TimeBasedRequest
{
	/// <summary>
	/// Constructor
	/// </summary>
	protected FlowRequest()
	{
		FlowId = -1;
		SortDirection = SortDirection.Descending;
		Take = 100;
		Skip = 0;
	}

	/// <summary>
	/// The id of the flow device
	/// </summary>
	public int DeviceId { get; set; }

	/// <summary>
	/// The netflow filter
	/// </summary>
	public NetflowFilter NetflowFilter { get; set; } = new NetflowFilter();

	/// <summary>
	/// The specific flow id (defaults to -1)
	/// </summary>
	public int FlowId { get; set; }

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
