namespace LogicMonitor.Api.LogicModules;

/// <summary>
///     A DataSource base
/// </summary>
[DataContract]
public class DataSource : NamedItem, IHasEndpoint
{
	/// <summary>
	///     The AGD Method
	/// </summary>
	[DataMember(Name = "agdMethod")]
	public string AgdMethod { get; set; }

	/// <summary>
	///     The AGD parameters
	/// </summary>
	[DataMember(Name = "agdParams")]
	public string AgdParams { get; set; }

	/// <summary>
	///     What this applies to
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; }

	/// <summary>
	///     The audit version
	/// </summary>
	[DataMember(Name = "auditVersion")]
	public int? AuditVersion { get; set; }

	/// <summary>
	///     Whether to delete inactive DataSourceInstances
	/// </summary>
	[DataMember(Name = "wildcard_deleteinactive")]
	public bool AutoDiscoveryDeleteInactive { get; set; }

	/// <summary>
	///     The AutoDiscovery Groovyscript
	/// </summary>
	[DataMember(Name = "wildcard_groovyscript")]
	public string AutoDiscoveryGroovyscript { get; set; }

	/// <summary>
	///     The AutoDiscovery schedule interval in minutes
	/// </summary>
	[DataMember(Name = "wildcard_schedule")]
	public int? AutoDiscoveryIntervalMinutes { get; set; }

	/// <summary>
	///     The AutoDiscovery Linux commnd line
	/// </summary>
	[DataMember(Name = "wildcard_linuxcmdline")]
	public string AutoDiscoveryLinuxCmdLine { get; set; }

	/// <summary>
	///     Wildcard linux command line
	/// </summary>
	[DataMember(Name = "wildcard_linuxscript")]
	public string AutoDiscoveryLinuxScript { get; set; }

	/// <summary>
	///     The AutoDiscovery Windows command line
	/// </summary>
	[DataMember(Name = "wildcard_wincmdline")]
	public string AutoDiscoveryWindowsCommandLine { get; set; }

	/// <summary>
	///     The AutoDiscovery Windows script
	/// </summary>
	[DataMember(Name = "wildcard_winscript")]
	public string AutoDiscoveryWindowsScript { get; set; }

	/// <summary>
	///     The DataSourceType
	/// </summary>
	[DataMember(Name = "collector")]
	public string Collector { get; set; }

	/// <summary>
	///     The collection method
	/// </summary>
	[DataMember(Name = "collectMethod")]
	public CollectionMethod CollectionMethod { get; set; }

	/// <summary>
	///     The CollectorAttribute
	/// </summary>
	[DataMember(Name = "collectorAttribute")]
	public CollectorAttribute CollectorAttribute { get; set; }

	/// <summary>
	///     The display name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; }

	/// <summary>
	///     Whether ERI discovery is enabled
	/// </summary>
	[DataMember(Name = "enableEriDiscovery")]
	public string EnableEriDiscovery { get; set; }

	/// <summary>
	///     The ERI discovery interval in (unknown time unit)
	/// </summary>
	[DataMember(Name = "eriDiscoveryInterval")]
	public string EriDiscoveryInterval { get; set; }

	/// <summary>
	///     The ERI discovery config
	/// </summary>
	[DataMember(Name = "eriDiscoveryConfig")]
	public object EriDiscoveryConfig { get; set; }

	/// <summary>
	///     The Group name
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; }

	/// <summary>
	///     Whether there are multiple DataSourceInstances
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
	///     Whether the AutoDiscovery is persistent
	/// </summary>
	[DataMember(Name = "autoDiscoveryConfig")]
	public AutoDiscoveryConfiguration AutoDiscoveryConfiguration { get; set; }

	/// <summary>
	///     The polling interval in seconds
	/// </summary>
	[DataMember(Name = "collectInterval")]
	public int? PollingIntervalSeconds { get; set; }

	/// <summary>
	///     The publish information
	/// </summary>
	[DataMember(Name = "published")]
	public int? Published { get; set; }

	/// <summary>
	///     Tags
	/// </summary>
	[DataMember(Name = "tags")]
	public string Tags { get; set; }

	/// <summary>
	///     Technology
	/// </summary>
	[DataMember(Name = "technology")]
	public string Technology { get; set; }

	/// <summary>
	///     Whether use use the WildValue as the UUID
	/// </summary>
	[DataMember(Name = "useWildValueAsUUID")]
	public bool UseWildValueAsUuid { get; set; }

	/// <summary>
	///     The version
	/// </summary>
	[DataMember(Name = "version")]
	public int? Version { get; set; }

	/// <summary>
	///     The dataPoints
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<DataSourceDataPoint> DataSourceDataPoints { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/datasources";

	/// <inheritdoc />
	/// <returns>'Id : Name - DisplayedAs'</returns>
	public override string ToString() => $"{Id} : {Name} - {DisplayName}";
}
