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
	public string Checksum { get; set; }

	/// <summary>
	/// The lineage Id of the LMModule
	/// </summary>
	[DataMember(Name = "lineageId")]
	public string LineageId { get; set; }

	/// <summary>
	/// The local module\u0027s IntegrationMetadata, readable for troubleshooting purposes
	/// </summary>
	[DataMember(Name = "installationMetadata", IsRequired =	false)]
	public IntegrationMetadata? InstallationMetadata { get; set; }
}