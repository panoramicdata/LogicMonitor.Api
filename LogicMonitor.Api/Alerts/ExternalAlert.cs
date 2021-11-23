namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An external alert
/// </summary>
public class ExternalAlert : IdentifiedItem, IHasEndpoint
{
	/// <summary>
	/// Groups
	/// </summary>
	[DataMember(Name = "groups")]
	public string Groups { get; set; }

	/// <summary>
	/// The collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int CollectorId { get; set; }

	/// <summary>
	/// The collector description
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; }

	/// <summary>
	/// The mechanism
	/// </summary>
	[DataMember(Name = "mechanism")]
	public string Mechanism { get; set; }

	/// <summary>
	/// The scriptPath
	/// </summary>
	[DataMember(Name = "scriptPath")]
	public string ScriptPath { get; set; }

	/// <summary>
	/// The scriptCmdline
	/// </summary>
	[DataMember(Name = "scriptCmdline")]
	public string ScriptCmdline { get; set; }

	/// <summary>
	/// The snmp trap version
	/// </summary>
	[DataMember(Name = "snmpTrapVersion")]
	public string SnmpTrapVersion { get; set; }

	/// <summary>
	/// The snmp trap server
	/// </summary>
	[DataMember(Name = "snmpTrapServer")]
	public string SnmpTrapServer { get; set; }

	/// <summary>
	/// The snmp community
	/// </summary>
	[DataMember(Name = "snmpCommunity")]
	public string SnmpCommunity { get; set; }

	/// <summary>
	/// The syslog server
	/// </summary>
	[DataMember(Name = "syslogServer")]
	public string SyslogServer { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/alert/internalalerts";
}
