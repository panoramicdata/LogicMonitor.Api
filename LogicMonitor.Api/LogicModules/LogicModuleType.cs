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
	[EnumMember(Value = "DATASOURCE")]
	DataSource,

	/// <summary>
	/// EventSource
	/// </summary>
	[EnumMember(Value = "EVENTSOURCE")]
	EventSource,

	/// <summary>
	/// ConfigSource
	/// </summary>
	[EnumMember(Value = "CONFIGSOURCE")]
	ConfigSource,

	/// <summary>
	/// PropertySource
	/// </summary>
	[EnumMember(Value = "PROPERTYSOURCE")]
	PropertySource,

	/// <summary>
	/// JobMonitor
	/// </summary>
	[EnumMember(Value = "JOBMONITOR")]
	JobMonitor,

	/// <summary>
	/// AppliesTo
	/// </summary>
	[EnumMember(Value = "APPLIESTO_FUNCTION")]
	AppliesToFunction,

	/// <summary>
	/// AppliesTo
	/// </summary>
	[EnumMember(Value = "SNMP_SYSOID_MAP")]
	SnmpSysOIDMap,

	/// <summary>
	/// AppliesTo
	/// </summary>
	[EnumMember(Value = "TOPOLOGYSOURCE")]
	TopologySource,

	/// <summary>
	/// LogSource
	/// </summary>
	[EnumMember(Value = "LOGSOURCE")]
	LogSource
}
