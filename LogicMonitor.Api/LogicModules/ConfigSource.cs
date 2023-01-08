namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A ConfigSource
/// </summary>
[DataContract]
public class ConfigSource : LogicModule, IHasEndpoint
{
	/// <summary>
	/// The publishing information
	/// </summary>
	[DataMember(Name = "configChecks")]
	public List<ConfigCheck> ConfigChecks { get; set; }

	/// <summary>
	/// The publishing information
	/// </summary>
	[DataMember(Name = "published")]
	public string Published { get; set; }

	/// <summary>
	/// What this applies to
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; }

	/// <summary>
	/// The audit version
	/// </summary>
	[DataMember(Name = "auditVersion")]
	public int? AuditVersion { get; set; }

	/// <summary>
	/// The autodiscovery config
	/// </summary>
	[DataMember(Name = "autoDiscoveryConfig")]
	public object AutoDiscoveryConfig { get; set; }

	/// <summary>
	/// The collection interval in seconds
	/// </summary>
	[DataMember(Name = "collectInterval")]
	public int CollectionIntervalSeconds { get; set; }

	/// <summary>
	/// The collection method
	/// </summary>
	[DataMember(Name = "collectMethod")]
	public CollectionMethod CollectionMethod { get; set; }

	/// <summary>
	/// The collector attribute
	/// </summary>
	[DataMember(Name = "collectorAttribute")]
	public CollectorAttribute CollectorAttribute { get; set; }

	/// <summary>
	/// The display name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; }

	/// <summary>
	/// Whether autodiscovery is enabled
	/// </summary>
	[DataMember(Name = "enableAutoDiscovery")]
	public bool EnableAutoDiscovery { get; set; }

	/// <summary>
	/// The file format
	/// </summary>
	[DataMember(Name = "fileFormat")]
	public string FileFormat { get; set; }

	/// <summary>
	/// The Group name
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; }

	/// <summary>
	/// Whether there are multiple DataSourceInstances
	/// </summary>
	[DataMember(Name = "hasMultiInstances")]
	public bool HasMultiInstances { get; set; }

	/// <summary>
	/// Origin id
	/// </summary>
	[DataMember(Name = "originId")]
	public int? OriginId { get; set; }

	/// <summary>
	/// Retain
	/// </summary>
	[DataMember(Name = "retain")]
	public string Retention { get; set; }

	/// <summary>
	/// Tags
	/// </summary>
	[DataMember(Name = "tags")]
	public string Tags { get; set; }

	/// <summary>
	/// Technology
	/// </summary>
	[DataMember(Name = "technology")]
	public string Technology { get; set; }

	/// <summary>
	/// TimestampFormat
	/// </summary>
	[DataMember(Name = "timestampFormat")]
	public string TimestampFormat { get; set; }

	/// <summary>
	/// The version
	/// </summary>
	[DataMember(Name = "version")]
	public int? Version { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/configsources";
}
