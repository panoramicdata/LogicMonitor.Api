namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The list of LogicModule names and credentials to import
/// </summary>
[DataContract]
public class LogicModuleImportObject : LogicModuleImportCredentials
{
	/// <summary>
	/// The LogicModule names. This is the same for all Module types except SNMP SysOID Maps
	/// </summary>
	[DataMember(Name = "importDataSources")]
	public List<string> ImportDataSources { get; set; } = new();
}
