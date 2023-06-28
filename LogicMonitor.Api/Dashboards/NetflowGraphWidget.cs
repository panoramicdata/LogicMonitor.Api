namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A netflow graph widget
/// </summary>
public class NetflowGraphWidget : Widget, IWidget
{
	/// <summary>
	/// The graph type
	/// </summary>
	[DataMember(Name = "graph")]
	public string GraphType { get; set; } = string.Empty;

	/// <summary>
	/// The interface index
	/// </summary>
	[DataMember(Name = "interfaceIndex")]
	public int InterfaceIndex { get; set; }

	/// <summary>
	/// The device id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The device name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceName { get; set; } = string.Empty;

	/// <summary>
	/// The row filters
	/// </summary>
	[DataMember(Name = "rowFilters")]
	public string RowFilters { get; set; } = string.Empty;

	/// <summary>
	/// The direction
	/// </summary>
	[DataMember(Name = "direction")]
	public string Direction { get; set; } = string.Empty;

	/// <summary>
	/// The QoS type
	/// </summary>
	[DataMember(Name = "qosType")]
	public string QosType { get; set; } = string.Empty;

	/// <summary>
	/// The interface name
	/// </summary>
	[DataMember(Name = "interfaceName")]
	public string InterfaceName { get; set; } = string.Empty;

	/// <summary>
	/// The netflow filter
	/// </summary>
	[DataMember(Name = "netflowFilter")]
	public object NetflowFilter { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
