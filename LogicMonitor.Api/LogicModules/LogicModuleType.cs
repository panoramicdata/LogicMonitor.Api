namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The LogicModule type
/// </summary>
[DataContract]
public enum LogicModuleType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "All")]
	All = 0,

	/// <summary>
	/// DataSource
	/// </summary>
	[EnumMember(Value = "DataSource")]
	DataSource,

	/// <summary>
	/// EventSource
	/// </summary>
	[EnumMember(Value = "EventSource")]
	EventSource,

	/// <summary>
	/// ConfigSource
	/// </summary>
	[EnumMember(Value = "ConfigSource")]
	ConfigSource,

	/// <summary>
	/// PropertySource
	/// </summary>
	[EnumMember(Value = "PropertySource")]
	PropertySource,

	/// <summary>
	/// JobMonitor
	/// </summary>
	[EnumMember(Value = "JobMonitor")]
	JobMonitor,

	/// <summary>
	/// AppliesTo
	/// </summary>
	[EnumMember(Value = "AppliesToFunction")]
	AppliesToFunction,

	/// <summary>
	/// AppliesTo
	/// </summary>
	[EnumMember(Value = "SNMP SysOID Map")]
	SnmpSysOIDMap,

	/// <summary>
	/// AppliesTo
	/// </summary>
	[EnumMember(Value = "TopologySource")]
	TopologySource
}
