namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The list of Snmp SysOID Map names and credentials to import
/// </summary>
[DataContract]
public class SnmpSysOidMapImportObject
{
	/// <summary>
	/// The server address
	/// </summary>
	[DataMember(Name = "coreServer")]
	public string CoreServer { get; set; }

	/// <summary>
	/// The username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; } = "anonymouse";

	/// <summary>
	/// The password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = "logicmonitor";

	/// <summary>
	/// The LogicModule names. This is the same for all Module types except SNMP SysOID Maps
	/// </summary>
	[DataMember(Name = "importOids")]
	public List<SnmpSysOidMapImportItem> ImportOids { get; set; }
}
