namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A netscan policy assignment type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum NetscanAssignmentType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    All devices
	/// </summary>
	[EnumMember(Value = "1")]
	All = 1,

	/// <summary>
	///    Cisco devices
	/// </summary>
	[EnumMember(Value = "2")]
	Cisco = 2,

	/// <summary>
	///    Linux devices
	/// </summary>
	[EnumMember(Value = "3")]
	Linux = 3,

	/// <summary>
	///    NetApp devices
	/// </summary>
	[EnumMember(Value = "4")]
	NetApp = 4,

	/// <summary>
	///    Windows devices
	/// </summary>
	[EnumMember(Value = "5")]
	Windows = 5,

	/// <summary>
	///    Devices specified using a query string
	/// </summary>
	[EnumMember(Value = "6")]
	Custom = 6
}
