namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Netflow filter
/// </summary>
[DataContract]
public class NetflowFilters
{
	/// <summary>
	/// Node B
	/// </summary>
	[DataMember(Name = "nodeB", IsRequired = false)]
	public string NodeB { get; set; } = string.Empty;

	/// <summary>
	/// The qosType
	/// </summary>
	[DataMember(Name = "qosType", IsRequired = false)]
	public string QosType { get; set; } = string.Empty;

	/// <summary>
	/// The device interfaces
	/// </summary>
	[DataMember(Name = "deviceInterfaces")]
	public List<object> DeviceInterfaces { get; set; } = new();

	/// <summary>
	/// The appType
	/// </summary>
	[DataMember(Name = "appType")]
	public string AppType { get; set; } = "all";

	/// <summary>
	/// The direction
	/// </summary>
	[DataMember(Name = "direction")]
	public string Direction { get; set; } = "both";

	/// <summary>
	/// The protocol
	/// </summary>
	[DataMember(Name = "protocol")]
	public string Protocol { get; set; } = "all";

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public string Ports { get; set; } = "";

	/// <summary>
	/// The top
	/// </summary>
	[DataMember(Name = "top")]
	public int Top { get; set; } = 10;

	/// <summary>
	/// The interface names
	/// </summary>
	[DataMember(Name = "ifNames")]
	public List<string> InterfaceNames { get; set; } = new();

	/// <summary>
	/// The IP version
	/// </summary>
	[DataMember(Name = "ipVersion")]
	public string IpVersion { get; set; } = "both";

	/// <summary>
	/// The conversation
	/// </summary>
	[DataMember(Name = "conversation")]
	public List<NetflowFilterConversation> Conversations { get; set; } = new();

	/// <summary>
	/// The conversation
	/// </summary>
	[DataMember(Name = "nbarApplicationNames")]
	public List<string> NbarApplicationNames { get; set; } = new();

	/// <summary>
	/// The netflow devices
	/// </summary>
	[DataMember(Name = "netflowDevices")]
	public List<object> NetflowDevices { get; set; } = new();

	/// <summary>
	/// The interface index
	/// </summary>
	[DataMember(Name = "ifIdx")]
	public int InterfaceIndex { get; set; }

	/// <summary>
	/// The interface index
	/// </summary>
	[DataMember(Name = "ifName")]
	public string InterfaceName { get; set; }

	/// <summary>
	/// Node A
	/// </summary>
	[DataMember(Name = "nodeA")]
	public string NodeA { get; set; }

	/// <summary>
	/// Converts to a URL encoded string for the query URL
	/// </summary>
	internal string AsUrlEncodedString()
	=> HttpUtility.UrlEncode(JsonConvert.SerializeObject(this));
}
