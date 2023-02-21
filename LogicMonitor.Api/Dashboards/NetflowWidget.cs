namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A netflow widget
/// </summary>
[DataContract]
public class NetflowWidget : Widget
{
	/// <summary>
	/// The function
	/// </summary>
	[DataMember(Name = "function")]
	public string Function { get; set; }

	/// <summary>
	/// The interface index
	/// </summary>
	[DataMember(Name = "ifIdx")]
	public int InterfaceIndex { get; set; }

	/// <summary>
	/// The device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The device id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The data type
	/// </summary>
	[DataMember(Name = "dataType")]
	public string DataType { get; set; }

	/// <summary>
	/// The row filters
	/// </summary>
	[DataMember(Name = "rowFilters")]
	public string RowFilters { get; set; }

	/// <summary>
	/// The direction
	/// </summary>
	[DataMember(Name = "direction")]
	public string Direction { get; set; }

	/// <summary>
	/// The QoS type
	/// </summary>
	[DataMember(Name = "qosType")]
	public string QosType { get; set; }

	/// <summary>
	/// The device name
	/// </summary>
	[DataMember(Name = "hostName")]
	public string DeviceName { get; set; }

	/// <summary>
	/// The interface name
	/// </summary>
	[DataMember(Name = "ifName")]
	public string InterfaceName { get; set; }

	/// <summary>
	/// The filter
	/// </summary>
	[DataMember(Name = "filter")]
	public string Filter { get; set; }

	/// <summary>
	/// The netflow filter
	/// </summary>
	[DataMember(Name = "netflowFilter")]
	public NetflowFilters NetflowFilter { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; }
}
