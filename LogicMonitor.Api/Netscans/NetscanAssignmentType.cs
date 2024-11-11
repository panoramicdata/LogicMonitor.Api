namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A netscan policy assignment type
/// </summary>
[DataContract]
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
	All = 1,

	/// <summary>
	///    Cisco Resource
	/// </summary>
	Cisco = 2,

	/// <summary>
	///    Linux Resource
	/// </summary>
	Linux = 3,

	/// <summary>
	///    NetApp Resource
	/// </summary>
	NetApp = 4,

	/// <summary>
	///    Windows Resource
	/// </summary>
	Windows = 5,

	/// <summary>
	///    Resources specified using a query string
	/// </summary>
	Custom = 6
}
