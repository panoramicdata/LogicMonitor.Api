namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A RemediationSource LogicModule
/// </summary>
[DataContract]
public class RemediationSource : NamedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	/// The Access Groups Id's
	/// </summary>
	[DataMember(Name = "accessGroupIds")]
	public List<int> AccessGroupIds { get; set; } = [];

	/// <summary>
	/// Access group Details in response
	/// </summary>
	[DataMember(Name = "accessGroups")]
	public List<AccessGroup> AccessGroups { get; set; } = [];

	/// <summary>
	/// The Applies To for the LMModule
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The metadata checksum for the LMModule content
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "checksum")]
	public string Checksum { get; set; } = string.Empty;

	/// <summary>
	/// The data type of remediation source, default is 0.
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "dataType")]
	public int DataType { get; set; }

	/// <summary>
	/// Groovy script
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; } = string.Empty;

	/// <summary>
	/// The group the LMModule is in
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// The local module's IntegrationMetadata, readable for troubleshooting purposes
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "installationMetadata")]
	public IntegrationMetadata? InstallationMetadata { get; set; }

	/// <summary>
	/// The remediation Installation status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "installationStatuses")]
	public List<string> InstallationStatuses { get; set; } = [];

	/// <summary>
	/// Whether the LM module is currently in use
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "isInUse")]
	public bool IsInUse { get; set; }

	/// <summary>
	/// The lineageId the LMModule belongs to
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "lineageId")]
	public string LineageId { get; set; } = string.Empty;

	/// <summary>
	/// The Registry ID of the Exchange Integration this module is based from, including this field will set this as the module's import base and mark the ID's version as audited
	/// </summary>
	[DataMember(Name = "originRegistryId")]
	public string OriginRegistryId { get; set; } = string.Empty;

	/// <summary>
	/// Script type: groovy | powershell
	/// </summary>
	[DataMember(Name = "scriptType")]
	public string ScriptType { get; set; } = string.Empty;

	/// <summary>
	/// The remediation source
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "source")]
	public string Source { get; set; } = string.Empty;

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

	/// <inheritdoc />
	public string Endpoint() => "setting/remediationsources";
}
