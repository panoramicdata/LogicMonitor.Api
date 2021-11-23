using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
///    EventSourceType
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum EventSourceType : byte
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    Script Event
	/// </summary>
	[EnumMember(Value = "scriptevent")]
	ScriptEvent = 1,

	/// <summary>
	///    LogFile
	/// </summary>
	[EnumMember(Value = "logfile")]
	LogFile = 2,

	/// <summary>
	///    Windows Event Log
	/// </summary>
	[EnumMember(Value = "wineventlog")]
	WindowsEventLog = 3,

	/// <summary>
	///    IPMI
	/// </summary>
	[EnumMember(Value = "ipmievent")]
	IpmiEvent = 4,

	/// <summary>
	///    Syslog
	/// </summary>
	[EnumMember(Value = "syslog")]
	Syslog = 5,

	/// <summary>
	///    SNMP Trap
	/// </summary>
	[EnumMember(Value = "snmptrap")]
	SnmpTrap = 6,

	/// <summary>
	///    Echo
	/// </summary>
	[EnumMember(Value = "echo")]
	Echo = 7,

	/// <summary>
	///    Azure RSS
	/// </summary>
	[EnumMember(Value = "azurerss")]
	AzureRss = 8,

	/// <summary>
	///    AWS RSS
	/// </summary>
	[EnumMember(Value = "awsrss")]
	AwsRss = 9,

	/// <summary>
	///    GCP Atom
	/// </summary>
	[EnumMember(Value = "gcpatom")]
	GcpAtom = 10,
}
