namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The LogicModule type
/// </summary>
[DataContract]
[JsonConverter(typeof(TolerantStringEnumConverter))]
public enum LogicModuleType
{
	/// <summary>
	/// A type this client does not model.  TolerantStringEnumConverter resolves an
	/// unrecognised value here, so an unknown type is never mistaken for All, which
	/// means "every type" when querying.
	/// </summary>
	[EnumMember(Value = "Unknown")]
	Unknown = -1,

	/// <summary>
	/// All types.  Used as the no-filter value when querying.
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
	/// SnmpSysOIDMap (or SNMP_SYSOID_MAP)
	/// </summary>
	[EnumMember(Value = "SNMP SysOID Map")]
	[EnumMemberAlias("SNMP_SYSOID_MAP")]
	SnmpSysOIDMap,

	/// <summary>
	/// TopologySource
	/// </summary>
	[EnumMember(Value = "TOPOLOGYSOURCE")]
	TopologySource,

	/// <summary>
	/// DiagnosticSource
	/// </summary>
	[EnumMember(Value = "DIAGNOSTICSOURCE")]
	DiagnosticSource,

	/// <summary>
	/// LogSource
	/// </summary>
	[EnumMember(Value = "LOGSOURCE")]
	LogSource,

	/// <summary>
	/// RemediationSource - the remote action modules (Kill Windows Process, Restart Linux
	/// Device and similar).  LogicMonitor sends REMEDIATIONSOURCE, not ACTIONSOURCE.
	/// </summary>
	[EnumMember(Value = "REMEDIATIONSOURCE")]
	RemediationSource
}