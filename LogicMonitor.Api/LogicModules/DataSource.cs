namespace LogicMonitor.Api.LogicModules;

/// <summary>
///     A DataSource base
/// </summary>
[DataContract]
public class DataSource : LogicModule, IHasEndpoint
{
	/// <summary>
	///     The AGD Method
	/// </summary>
	[DataMember(Name = "agdMethod")]
	public string AgdMethod { get; set; } = string.Empty;

	/// <summary>
	///     The AGD parameters
	/// </summary>
	[DataMember(Name = "agdParams")]
	public string AgdParams { get; set; } = string.Empty;

	/// <summary>
	///     What this applies to
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The data source audit version
	/// </summary>
	[DataMember(Name = "auditVersion")]
	public int AuditVersion { get; set; }

	/// <summary>
	///     Whether to delete inactive DataSourceInstances
	/// </summary>
	[DataMember(Name = "wildcard_deleteinactive")]
	public bool AutoDiscoveryDeleteInactive { get; set; }

	/// <summary>
	///     The AutoDiscovery Groovyscript
	/// </summary>
	[DataMember(Name = "wildcard_groovyscript")]
	public string AutoDiscoveryGroovyscript { get; set; } = string.Empty;

	/// <summary>
	///     The AutoDiscovery schedule interval in minutes
	/// </summary>
	[DataMember(Name = "wildcard_schedule")]
	public int? AutoDiscoveryIntervalMinutes { get; set; }

	/// <summary>
	///     The AutoDiscovery Linux commnd line
	/// </summary>
	[DataMember(Name = "wildcard_linuxcmdline")]
	public string AutoDiscoveryLinuxCmdLine { get; set; } = string.Empty;

	/// <summary>
	///     Wildcard linux command line
	/// </summary>
	[DataMember(Name = "wildcard_linuxscript")]
	public string AutoDiscoveryLinuxScript { get; set; } = string.Empty;

	/// <summary>
	///     The AutoDiscovery Windows command line
	/// </summary>
	[DataMember(Name = "wildcard_wincmdline")]
	public string AutoDiscoveryWindowsCommandLine { get; set; } = string.Empty;

	/// <summary>
	///     The AutoDiscovery Windows script
	/// </summary>
	[DataMember(Name = "wildcard_winscript")]
	public string AutoDiscoveryWindowsScript { get; set; } = string.Empty;

	/// <summary>
	/// The  method to collect data: snmp|ping|exs|webpage|wmi|cim|datadump|dns|ipmi|jdbb|script|udp|tcp|xen
	/// </summary>
	[DataMember(Name = "collectMethod")]
	public string CollectionMethod { get; set; } = string.Empty;

	/// <summary>
	/// data collector\u0027s attributes to collector data. e.g. a ping data source has a ping collector attribute. \n PingCollectorAttributeV1 has two fields. the ip to ping, the data size send to ping
	/// </summary>
	[DataMember(Name = "collectorAttribute")]
	public CollectorAttribute CollectorAttribute { get; set; } = new();

	/// <summary>
	/// The data source display name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Enable ERI Discovery or not: false|true
	/// </summary>
	[DataMember(Name = "enableEriDiscovery")]
	public bool EnableEriDiscovery { get; set; }

	/// <summary>
	/// The DataSource data collect interval
	/// </summary>
	[DataMember(Name = "eriDiscoveryInterval")]
	public string EriDiscoveryInterval { get; set; } = string.Empty;

	/// <summary>
	/// Enable ERI Discovery or not
	/// </summary>
	[DataMember(Name = "eriDiscoveryConfig")]
	public ScriptERIDiscoveryAttributeV3 EriDiscoveryConfig { get; set; } = new();

	/// <summary>
	/// The group the LMModule is in
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// If the DataSource has multi instance: true|false
	/// </summary>
	[DataMember(Name = "hasMultiInstances")]
	public bool HasMultiInstances { get; set; }

	/// <summary>
	///     Whether it has an unconfirmed Alert
	/// </summary>
	[DataMember(Name = "hasUnConfirmedAlert")]
	public bool HasUnacknowledgedAlert { get; set; }

	/// <summary>
	///     Whether AutoDiscovery is disabled
	/// </summary>
	[DataMember(Name = "wildcard_disable")]
	public bool IsAutoDiscoveryDisabled { get; set; }

	/// <summary>
	///     Whether autodiscovery is enabled
	/// </summary>
	[DataMember(Name = "enableAutoDiscovery")]
	public bool IsAutoDiscoveryEnabled { get; set; }

	/// <summary>
	/// Auto discovery configuration
	/// </summary>
	[DataMember(Name = "autoDiscoveryConfig")]
	public AutoDiscoveryConfiguration AutoDiscoveryConfiguration { get; set; } = new();

	/// <summary>
	/// The dataSource payload Version for custom metrics
	/// </summary>
	[DataMember(Name = "payloadVersion")]
	public int PayloadVersion { get; set; }

	/// <summary>
	/// The DataSource data collect interval
	/// </summary>
	[DataMember(Name = "collectInterval")]
	public int PollingIntervalSeconds { get; set; }

	/// <summary>
	///     The publish information
	/// </summary>
	[DataMember(Name = "published")]
	public int? Published { get; set; }

	/// <summary>
	/// The Tags for the LMModule
	/// </summary>
	[DataMember(Name = "tags")]
	public string Tags { get; set; } = string.Empty;

	/// <summary>
	/// The Technical Notes for the LMModule
	/// </summary>
	[DataMember(Name = "technology")]
	public string Technology { get; set; } = string.Empty;

	/// <summary>
	/// Use Wildvalue as Unique identifier in case of multiinstance datasource: true|false
	/// </summary>
	[DataMember(Name = "useWildValueAsUUID")]
	public bool UseWildValueAsUuid { get; set; }

	/// <summary>
	/// The data source version
	/// </summary>
	[DataMember(Name = "version")]
	public int Version { get; set; }

	/// <summary>
	/// The data point list
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<DataPoint> DataSourceDataPoints { get; set; } = new();

	/// <inheritdoc />
	public string Endpoint() => "setting/datasources";

	/// <inheritdoc />
	/// <returns>'Id : Name - DisplayedAs'</returns>
	public override string ToString() => $"{Id} : {Name} - {DisplayName}";
}
