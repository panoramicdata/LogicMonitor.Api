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
	/// The lineageId the LMModule belongs to
	/// </summary>
	[DataMember(Name = "lineageId")]
	public string LineageId { get; set; }

	/// <summary>
	/// The installation metadata
	/// </summary>
	[DataMember(Name = "installationMetadata")]
	public IntegrationMetadata? InstallationMetadata { get; set; }
}