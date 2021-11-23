namespace LogicMonitor.Api.LogicModules;

/// <summary>
///     LogicModule Metadata
/// </summary>
[DataContract]
public class LogicModuleMetadata
{
	/// <summary>
	///     The status
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; }


	/// <summary>
	///     The namespace
	/// </summary>
	[DataMember(Name = "namespace")]
	public string Namespace { get; set; }

	/// <summary>
	///     The registryVersion
	/// </summary>
	[DataMember(Name = "registryVersion")]
	public string RegistryVersion { get; set; }

	/// <summary>
	///     The quality
	/// </summary>
	[DataMember(Name = "quality")]
	public string Quality { get; set; }

	/// <summary>
	///     The LM Locator
	/// </summary>
	[DataMember(Name = "lmLocator")]
	public string LmLocator { get; set; }

	/// <summary>
	///     The ID
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; }

	/// <inheritdoc />
	/// <returns>'Id : Name - DisplayedAs'</returns>
	public override string ToString() => $"{Namespace}.{LmLocator}/v{RegistryVersion} ({Quality})";
}
