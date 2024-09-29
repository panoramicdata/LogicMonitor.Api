namespace LogicMonitor.Api.Flows;

/// <summary>
/// A flow request (abstract)
/// </summary>
public abstract class ResourceGroupFlowRequest : TimeBasedRequest
{
	/// <summary>
	/// Constructor
	/// </summary>
	protected ResourceGroupFlowRequest()
	{
		SortDirection = SortDirection.Descending;
		Take = 100;
		Skip = 0;
	}

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupId", true)]
	public int DeviceGroupId => ResourceGroupId;

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
