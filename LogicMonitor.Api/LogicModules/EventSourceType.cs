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

	/// <summary>
	///    AWS Health
	/// </summary>
	[EnumMember(Value = "awshealth")]
	AwsHealth = 11,

	/// <summary>
	///    AWS RDSPI event
	/// </summary>
	[EnumMember(Value = "awsrdspievent")]
	AwsRdspiEvent = 12,

	/// <summary>
	///    AWS trusted advisor
	/// </summary>
	[EnumMember(Value = "awstrustedadvisor")]
	AwsTrustedAdvisor = 13,

	/// <summary>
	///    Azure advisor
	/// </summary>
	[EnumMember(Value = "azureadvisor")]
	AzureAdvisor = 14,

	/// <summary>
	///    Azure emerging issue
	/// </summary>
	[EnumMember(Value = "azureemergingissue")]
	AzureEmergingIssue = 15,

	/// <summary>
	///    Azure resource health event
	/// </summary>
	[EnumMember(Value = "azureresourcehealthevent")]
	AzureResourceHealthEvent = 16,

	/// <summary>
	///    Azure log analytics workspaces event
	/// </summary>
	[EnumMember(Value = "azureloganalyticsworkspacesevent")]
	AzureLogAnalyticsWorkspacesEvent = 17,

	/// <summary>
	///    Aws Organizational Health
	/// </summary>
	[EnumMember(Value = "awsorganizationalhealth")]
	AwsOrganizationalHealth = 18
}
