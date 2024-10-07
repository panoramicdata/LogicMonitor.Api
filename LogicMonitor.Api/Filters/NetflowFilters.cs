namespace LogicMonitor.Api.Filters;

/// <summary>
/// A Netflow filter
/// </summary>
[DataContract]
public class NetflowFilters
{
	/// <summary>
	/// Node B
	/// </summary>
	[DataMember(Name = "nodeB")]
	public string NodeB { get; set; } = string.Empty;

	/// <summary>
	/// The qosType
	/// </summary>
	[DataMember(Name = "qosType")]
	public string QosType { get; set; } = string.Empty;

	/// <summary>
	/// The device interfaces
	/// </summary>
	[DataMember(Name = "deviceInterfaces")]
	public List<InterfacesFilter> DeviceInterfaces { get; set; } = [];

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public string Ports { get; set; } = string.Empty;

	/// <summary>
	/// The protocol
	/// </summary>
	[DataMember(Name = "protocol")]
	public string Protocol { get; set; } = string.Empty;

	/// <summary>
	/// The IP version
	/// </summary>
	[DataMember(Name = "ipVersion")]
	public string IpVersion { get; set; } = string.Empty;

	/// <summary>
	/// Netflow filter netflowDevices expression
	/// </summary>
	[DataMember(Name = "netflowDevices")]
	public List<NetflowDeviceInfo> NetflowDevices { get; set; } = [];

	/// <summary>
	/// The top
	/// </summary>
	[DataMember(Name = "top")]
	public int Top { get; set; }

	/// <summary>
	/// The appType
	/// </summary>
	[DataMember(Name = "appType")]
	public string AppType { get; set; } = string.Empty;

	/// <summary>
	/// nbarApplicationNames
	/// </summary>
	[DataMember(Name = "nbarApplicationNames")]
	public List<NbarApplicationNames> NbarApplicationNames { get; set; } = [];

	/// <summary>
	/// Node A
	/// </summary>
	[DataMember(Name = "nodeA")]
	public string NodeA { get; set; } = string.Empty;

	/// <summary>
	/// The conversation
	/// </summary>
	[DataMember(Name = "conversation")]
	public List<ConversationFilter> Conversations { get; set; } = [];

	/// <summary>
	/// The interface names
	/// </summary>
	[DataMember(Name = "ifNames")]
	public List<string> IfNames { get; set; } = [];

	/// <summary>
	/// The direction
	/// </summary>
	[DataMember(Name = "direction")]
	public string Direction { get; set; } = "both";

	/// <summary>
	/// Converts to a URL encoded string for the query URL
	/// </summary>
	public string AsUrlEncodedString()
	=> HttpUtility.UrlEncode(JsonConvert.SerializeObject(this));
}
