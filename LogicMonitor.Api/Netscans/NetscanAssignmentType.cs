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
	///    Cisco devices
	/// </summary>
	Cisco = 2,

	/// <summary>
	///    Linux devices
	/// </summary>
	Linux = 3,

	/// <summary>
	///    NetApp devices
	/// </summary>
	NetApp = 4,

	/// <summary>
	///    Windows devices
	/// </summary>
	Windows = 5,

	/// <summary>
	///    Devices specified using a query string
	/// </summary>
	Custom = 6
}
