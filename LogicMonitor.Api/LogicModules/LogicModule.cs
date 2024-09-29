namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A LogicModule
/// </summary>
[DataContract]
public abstract class LogicModule : NamedItem
{

	/// <summary>
	/// The metadata checksum for the LMModule content
	/// </summary>
	[DataMember(Name = "checksum")]
	public string Checksum { get; set; } = string.Empty;

	/// <summary>
	///		The access group Ids
	/// </summary>
	[DataMember(Name = "accessGroupIds")]
	public List<int> AccessGroupIds { get; set; } = [];

	/// <summary>
	///		The access groups
	/// </summary>
	[DataMember(Name = "accessGroups")]
	public List<AccessGroup> AccessGroups { get; set; } = [];

	/// <summary>
	/// The lineage Id of the LMModule
	/// </summary>
	[DataMember(Name = "lineageId")]
	public string LineageId { get; set; } = string.Empty;

	/// <summary>
	/// The local module\u0027s IntegrationMetadata, readable for troubleshooting purposes
	/// </summary>
	[DataMember(Name = "installationMetadata", IsRequired = false)]
	public IntegrationMetadata InstallationMetadata { get; set; } = new();
}