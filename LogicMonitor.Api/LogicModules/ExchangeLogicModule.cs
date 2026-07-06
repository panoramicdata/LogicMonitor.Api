namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A LogicModule as reported by the LM Exchange metadata endpoint (setting/logicmodules/metadata).
/// A single record per module, across all <see cref="LogicModuleType"/>s, annotated with installation
/// and upgrade state. This replaces the retired listcore "update" mechanism: a module has an update
/// available when <see cref="HasUpdateAvailable"/> is true.
/// </summary>
/// <remarks>
/// The endpoint returns a heterogeneous set of fields that vary by module type and installation state,
/// so only the identity, lifecycle and upgrade-detection fields are modelled here; unmodelled fields
/// are ignored during deserialization.
/// </remarks>
[DataContract]
public class ExchangeLogicModule
{
	/// <summary>
	/// The id. Numeric for installed modules; a GUID for not-installed catalog entries.
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The LogicModule type
	/// </summary>
	[DataMember(Name = "type")]
	public LogicModuleType Type { get; set; }

	/// <summary>
	/// The exchange model (e.g. exchangeDataSources)
	/// </summary>
	[DataMember(Name = "model")]
	public string Model { get; set; } = string.Empty;

	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The tags
	/// </summary>
	[DataMember(Name = "tags")]
	public List<string> Tags { get; set; } = [];

	/// <summary>
	/// When the module was last updated, in epoch milliseconds
	/// </summary>
	[DataMember(Name = "updatedAtMS")]
	public long UpdatedAtMs { get; set; }

	/// <summary>
	/// The installation statuses (e.g. IS_INSTALLED, IS_CUSTOMIZED). Empty for not-installed catalog entries.
	/// </summary>
	[DataMember(Name = "installationStatuses")]
	public List<string> InstallationStatuses { get; set; } = [];

	/// <summary>
	/// The installed origin version (semver, e.g. "1.8.0")
	/// </summary>
	[DataMember(Name = "originVersion")]
	public string OriginVersion { get; set; } = string.Empty;

	/// <summary>
	/// The origin lifecycle status (e.g. CORE, COMMUNITY, DEPRECATED)
	/// </summary>
	[DataMember(Name = "originStatus")]
	public string OriginStatus { get; set; } = string.Empty;

	/// <summary>
	/// The registry id of the currently-installed origin version
	/// </summary>
	[DataMember(Name = "originRegistryId")]
	public string OriginRegistryId { get; set; } = string.Empty;

	/// <summary>
	/// The registry id of the version this module can be upgraded to. When present and different from
	/// <see cref="OriginRegistryId"/>, an update is available (see <see cref="HasUpdateAvailable"/>).
	/// </summary>
	[DataMember(Name = "upgradeableRegistryId")]
	public string UpgradeableRegistryId { get; set; } = string.Empty;

	/// <summary>
	/// The origin locator
	/// </summary>
	[DataMember(Name = "originLocator")]
	public string OriginLocator { get; set; } = string.Empty;

	/// <summary>
	/// The origin name
	/// </summary>
	[DataMember(Name = "originName")]
	public string OriginName { get; set; } = string.Empty;

	/// <summary>
	/// The origin author namespace (e.g. core)
	/// </summary>
	[DataMember(Name = "originAuthorNamespace")]
	public string OriginAuthorNamespace { get; set; } = string.Empty;

	/// <summary>
	/// When the origin version was published, in epoch milliseconds
	/// </summary>
	[DataMember(Name = "originPublishedAtMS")]
	public long OriginPublishedAtMs { get; set; }

	/// <summary>
	/// Whether the module is in use
	/// </summary>
	[DataMember(Name = "isInUse")]
	public bool IsInUse { get; set; }

	/// <summary>
	/// Whether the local module has been changed from the last version published to the exchange (local drift)
	/// </summary>
	[DataMember(Name = "isChangedFromTargetLastPublished")]
	public bool IsChangedFromTargetLastPublished { get; set; }

	/// <summary>
	/// The group
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// The display name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The collection method
	/// </summary>
	[DataMember(Name = "collectionMethod")]
	public string CollectionMethod { get; set; } = string.Empty;

	/// <summary>
	/// The source (e.g. LOCAL for installed modules)
	/// </summary>
	[DataMember(Name = "source")]
	public string Source { get; set; } = string.Empty;

	/// <summary>
	/// The catalog version (present on not-installed catalog entries)
	/// </summary>
	[DataMember(Name = "version")]
	public string Version { get; set; } = string.Empty;

	/// <summary>
	/// The catalog status (present on not-installed catalog entries)
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;

	/// <summary>
	/// The catalog locator (present on not-installed catalog entries)
	/// </summary>
	[DataMember(Name = "locator")]
	public string Locator { get; set; } = string.Empty;

	/// <summary>
	/// The author portal name (present on not-installed catalog entries)
	/// </summary>
	[DataMember(Name = "authorPortalName")]
	public string AuthorPortalName { get; set; } = string.Empty;

	/// <summary>
	/// True when an update is available: the upgradeable registry id is present and differs from the
	/// installed origin registry id.
	/// </summary>
	public bool HasUpdateAvailable =>
		!string.IsNullOrEmpty(UpgradeableRegistryId)
		&& !string.IsNullOrEmpty(OriginRegistryId)
		&& !string.Equals(UpgradeableRegistryId, OriginRegistryId, StringComparison.Ordinal);

	/// <summary>
	/// True when the module is installed locally
	/// </summary>
	public bool IsInstalled => InstallationStatuses.Contains("IS_INSTALLED");

	/// <summary>
	/// True when the installed module has been customized
	/// </summary>
	public bool IsCustomized => InstallationStatuses.Contains("IS_CUSTOMIZED");

	/// <summary>
	/// True when the origin lifecycle status is DEPRECATED
	/// </summary>
	public bool IsDeprecated => string.Equals(OriginStatus, "DEPRECATED", StringComparison.OrdinalIgnoreCase);

	/// <inheritdoc />
	public override string ToString() => $"{Name} ({Type}) v{OriginVersion}{(HasUpdateAvailable ? " [update available]" : string.Empty)}";
}
