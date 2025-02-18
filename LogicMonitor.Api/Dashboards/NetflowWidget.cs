namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A netflow widget
/// </summary>
[DataContract]
public class NetflowWidget : Widget, IWidget
{
	/// <summary>
	/// The function
	/// </summary>
	[DataMember(Name = "function")]
	public string Function { get; set; } = string.Empty;

	/// <summary>
	/// The interface index
	/// </summary>
	[DataMember(Name = "ifIdx")]
	public int InterfaceIndex { get; set; }

	/// <summary>
	/// The Resource display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The data type
	/// </summary>
	[DataMember(Name = "dataType")]
	public string DataType { get; set; } = string.Empty;

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
	/// The device name
	/// </summary>
	[DataMember(Name = "hostName")]
	public string ResourceHostName { get; set; } = string.Empty;

	/// <summary>
	/// The interface name
	/// </summary>
	[DataMember(Name = "ifName")]
	public string InterfaceName { get; set; } = string.Empty;

	/// <summary>
	/// The filter
	/// </summary>
	[DataMember(Name = "filter")]
	public string Filter { get; set; } = string.Empty;

	/// <summary>
	/// The netflow filter
	/// </summary>
	[DataMember(Name = "netflowFilter")]
	public NetflowFilters NetflowFilter { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
