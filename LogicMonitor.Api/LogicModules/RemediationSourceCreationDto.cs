namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A request to create a remediation source
/// </summary>
[DataContract]
public class RemediationSourceCreationDto : CreationDto<RemediationSource>
{
	/// <summary>
	/// The Access Groups Id's
	/// </summary>
	[DataMember(Name = "accessGroupIds")]
	public List<int> AccessGroupIds { get; set; } = [];

	/// <summary>
	/// The Applies To for the LMModule
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The description for the LMModule
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

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
	/// The remediation source name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

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
	/// The Tags for the LMModule
	/// </summary>
	[DataMember(Name = "tags")]
	public string Tags { get; set; } = string.Empty;

	/// <summary>
	/// The Technical Notes for the LMModule
	/// </summary>
	[DataMember(Name = "technology")]
	public string Technology { get; set; } = string.Empty;
}
