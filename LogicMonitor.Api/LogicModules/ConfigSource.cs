namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A ConfigSource
/// </summary>
[DataContract]
public class ConfigSource : LogicModule, IHasEndpoint
{
	/// <summary>
	/// Collector attribute
	/// </summary>
	[DataMember(Name = "collectorAttribute")]
	public AttributeCollector? CollectorAttribute { get; set; }

	/// <summary>
	/// Auto discovery configuration
	/// </summary>
	[DataMember(Name = "autoDiscoveryConfig")]
	public AutoDiscoveryConfiguration? AutoDiscoveryConfig { get; set; }

	/// <summary>
	/// The ConfigSource display name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string? DisplayName { get; set; }

	/// <summary>
	/// The List of ConfigChecks
	/// </summary>
	[DataMember(Name = "configChecks")]
	public List<ConfigCheck>? ConfigChecks { get; set; }

	/// <summary>
	/// The Applies To for the LMModule
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string? AppliesTo { get; set; }

	/// <summary>
	/// Enable active discovery if ConfigSource has multiple instances. true|false
	/// </summary>
	[DataMember(Name = "enableAutoDiscovery")]
	public bool EnableAutoDiscovery { get; set; }

	/// <summary>
	/// The Technical Notes for the LMModule
	/// </summary>
	[DataMember(Name = "technology")]
	public string? Technology { get; set; }

	/// <summary>
	/// The ConfigSource version
	/// </summary>
	[DataMember(Name = "version")]
	public long Version { get; set; }

	/// <summary>
	/// The Tags for the LMModule
	/// </summary>
	[DataMember(Name = "tags")]
	public string? Tags { get; set; }

	/// <summary>
	/// The ConfigSource audit version
	/// </summary>
	[DataMember(Name = "auditVersion")]
	public long AuditVersion { get; set; }

	/// <summary>
	/// The method to collect data
	/// </summary>
	[DataMember(Name = "collectMethod")]
	public CollectionMethod CollectionMethod { get; set; }

	/// <summary>
	/// Whether the ConfigSource has multiple instances. true|false
	/// </summary>
	[DataMember(Name = "hasMultiInstances")]
	public bool HasMultiInstances { get; set; }

	/// <summary>
	/// The ConfigSource data collect interval
	/// </summary>
	[DataMember(Name = "collectInterval")]
	public int CollectionIntervalSeconds { get; set; }

	/// <summary>
	/// Timestamp format. ex. yyyy-MM-dd hh:mm:ss
	/// </summary>
	[DataMember(Name = "timestampFormat")]
	public string? TimestampFormat { get; set; }

	/// <summary>
	/// Configuration file format. arbitrary|unix|java-properties|JSON|XML
	/// </summary>
	[DataMember(Name = "fileFormat")]
	public string? FileFormat { get; set; }

	/// <summary>
	/// The group the LMModule is in
	/// </summary>
	[DataMember(Name = "group")]
	public string? Group { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/configsources";
}
