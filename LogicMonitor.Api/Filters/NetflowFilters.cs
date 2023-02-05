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
	[DataMember(Name = "deviceInterfaces", IsRequired = false)]
	public List<InterfacesFilter> DeviceInterfaces { get; set; } = new();

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public string Ports { get; set; } = string.Empty;

	/// <summary>
	/// The protocol
	/// </summary>
	[DataMember(Name = "protocol", IsRequired = false)]
	public string Protocol { get; set; } = string.Empty;

	/// <summary>
	/// The IP version
	/// </summary>
	[DataMember(Name = "ipVersion", IsRequired = false)]
	public string IpVersion { get; set; } = string.Empty;

	/// <summary>
	/// Netflow filter netflowDevices expression
	/// </summary>
	[DataMember(Name = "netflowDevices")]
	public List<NetflowDeviceInfo> NetflowDevices { get; set; } = new();

	/// <summary>
	/// The top
	/// </summary>
	[DataMember(Name = "top", IsRequired = false)]
	public int Top { get; set; }

	/// <summary>
	/// The appType
	/// </summary>
	[DataMember(Name = "appType", IsRequired = false)]
	public string AppType { get; set; } = string.Empty;

	/// <summary>
	/// nbarApplicationNames
	/// </summary>
	[DataMember(Name = "nbarApplicationNames")]
	public List<NbarApplicationNames> NbarApplicationNames { get; set; } = new();

	/// <summary>
	/// Node A
	/// </summary>
	[DataMember(Name = "nodeA", IsRequired = false)]
	public string NodeA { get; set; } = string.Empty;

	/// <summary>
	/// The conversation
	/// </summary>
	[DataMember(Name = "conversation", IsRequired = false)]
	public List<ConversationFilter> Conversations { get; set; } = new();

	/// <summary>
	/// The interface names
	/// </summary>
	[DataMember(Name = "ifNames", IsRequired = false)]
	public List<string> IfNames { get; set; } = new();

	/// <summary>
	/// The direction
	/// </summary>
	[DataMember(Name = "direction", IsRequired = false)]
	public string Direction { get; set; } = "both";

	/// <summary>
	/// Converts to a URL encoded string for the query URL
	/// </summary>
	public string AsUrlEncodedString()
	=> HttpUtility.UrlEncode(JsonConvert.SerializeObject(this));
}
