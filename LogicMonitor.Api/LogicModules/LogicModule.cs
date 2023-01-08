namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A LogicModule
/// </summary>
[DataContract]
public abstract class LogicModule : NamedItem
{

	/// <summary>
	/// The checksum
	/// </summary>
	[DataMember(Name = "checksum")]
	public string Checksum { get; set; }

	/// <summary>
	/// The lineage id
	/// </summary>
	[DataMember(Name = "lineageId")]
	public string LineageId { get; set; } = string.Empty;

	/// <summary>
	/// The installation metadata
	/// </summary>
	[DataMember(Name = "installationMetadata")]
	public object? InstallationMetadata { get; set; }
}