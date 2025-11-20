namespace LogicMonitor.Api.Netscans;

/// <summary>
///    The method by which the netscan policy discovers devices
/// </summary>
[DataContract(Name = "method")]
[JsonConverter(typeof(StringEnumConverter))]
public enum NetscanMethod
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    Use ICMP ping to scan the network
	/// </summary>
	[EnumMember(Value = "nmap")]
	Icmp = 1,

	/// <summary>
	///    AWS (EC2)
	/// </summary>
	[EnumMember(Value = "nec2")]
	Ec2 = 2,

	/// <summary>
	///    Upload a script or csv to discover devices
	/// </summary>
	[EnumMember(Value = "nscript")]
	Nscript = 3,

	/// <summary>
	///    Upload a script or csv to discover devices
	/// </summary>
	[EnumMember(Value = "script")]
	Script = 4,

	/// <summary>
	///    Enhanced script discovery method
	/// </summary>
	[EnumMember(Value = "enhancedScript")]
	EnhancedScript = 5
}
