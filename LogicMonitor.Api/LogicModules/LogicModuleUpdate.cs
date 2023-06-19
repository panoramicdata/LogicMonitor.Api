namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A LogicModule Update
/// </summary>
[DataContract]
public class LogicModuleUpdate : IHasEndpoint
{
	/// <summary>
	/// The local ID
	/// </summary>
	[DataMember(Name = "localId")]
	public int LocalId { get; set; }

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The appliesTo
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The permission
	/// </summary>
	[DataMember(Name = "category")]
	public LogicModuleUpdateCategory Category { get; set; }

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public LogicModuleType Type { get; set; }

	/// <summary>
	/// The collection method
	/// </summary>
	[DataMember(Name = "collectionMethod")]
	public string CollectionMethod { get; set; } = string.Empty;

	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The group
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// The version (installed). This is an epoch timestamp
	/// </summary>
	[DataMember(Name = "version")]
	public long Version { get; set; }

	/// <summary>
	/// The local version. This is an epoch timestamp
	/// </summary>
	[DataMember(Name = "localVersion")]
	public long LocalVersion { get; set; }

	/// <summary>
	/// The audit version. This is an epoch timestamp
	/// </summary>
	[DataMember(Name = "auditVersion")]
	public long AuditVersion { get; set; }

	/// <summary>
	/// The rest LM (?)
	/// </summary>
	[DataMember(Name = "restLm")]
	public string RestLm { get; set; } = string.Empty;

	/// <summary>
	/// The registryVersion
	/// </summary>
	[DataMember(Name = "registryVersion")]
	public string RegistryVersion { get; set; } = string.Empty;

	/// <summary>
	/// The publish time
	/// </summary>
	[DataMember(Name = "publishedAt")]
	public long PublishedAtMilliseconds { get; set; }

	/// <summary>
	/// The quality
	/// </summary>
	[DataMember(Name = "quality")]
	public string Quality { get; set; } = string.Empty;

	/// <summary>
	/// The locator
	/// </summary>
	[DataMember(Name = "locator")]
	public string Locator { get; set; } = string.Empty;

	/// <summary>
	/// The currentUuid
	/// </summary>
	[DataMember(Name = "currentUuid")]
	public string CurrentUuid { get; set; } = string.Empty;

	/// <summary>
	/// The namespace
	/// </summary>
	[DataMember(Name = "namespace")]
	public string Namespace { get; set; } = string.Empty;

	/// <summary>
	/// The local version
	/// </summary>
	[DataMember(Name = "local")]
	public string Local { get; set; } = string.Empty;

	/// <summary>
	/// The remote version
	/// </summary>
	[DataMember(Name = "remote")]
	public string Remote { get; set; } = string.Empty;

	/// <summary>
	/// The endpoint
	/// </summary>
	public string Endpoint() => "setting/logicmodules/listcore";
}
