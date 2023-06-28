namespace LogicMonitor.Api.Settings;

/// <summary>
/// An external alert destination
/// </summary>
[DataContract]
public class ExternalAlertDestination : IdentifiedItem, IHasEndpoint
{
	/// <summary>
	/// The groups
	/// </summary>
	[DataMember(Name = "groups")]
	public string Groups { get; set; } = string.Empty;

	/// <summary>
	/// The Collector Id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int CollectorId { get; set; }

	/// <summary>
	/// The collectorDescription
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; } = string.Empty;

	/// <summary>
	/// The mechanism
	/// </summary>
	[DataMember(Name = "mechanism")]
	public string Mechanism { get; set; } = string.Empty;

	/// <summary>
	/// The script path
	/// </summary>
	[DataMember(Name = "scriptPath")]
	public string ScriptPath { get; set; } = string.Empty;

	/// <summary>
	/// The script command line
	/// </summary>
	[DataMember(Name = "scriptCmdline")]
	public string ScriptCommandLine { get; set; } = string.Empty;

	/// <summary>
	/// The snmpTrapVersion
	/// </summary>
	[DataMember(Name = "snmpTrapVersion")]
	public string SnmpTrapVersion { get; set; } = string.Empty;

	/// <summary>
	/// The SNMP Trap server
	/// </summary>
	[DataMember(Name = "snmpTrapServer")]
	public string SnmpTrapServer { get; set; } = string.Empty;

	/// <summary>
	/// The SNMP community string
	/// </summary>
	[DataMember(Name = "snmpCommunity")]
	public string SnmpCommunityString { get; set; } = string.Empty;

	/// <summary>
	/// The Syslog server
	/// </summary>
	[DataMember(Name = "syslogServer")]
	public string SyslogServer { get; set; } = string.Empty;

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/alert/internalalerts";
}
