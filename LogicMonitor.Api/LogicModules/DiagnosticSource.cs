namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DiagnosticSource module.
/// </summary>
[DataContract]
public class DiagnosticSource : LogicModule, IHasEndpoint
{
	/// <summary>
	/// The script to execute.
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; } = string.Empty;

	/// <summary>
	/// The applies-to expression for target resources.
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// Technical notes.
	/// </summary>
	[DataMember(Name = "technology")]
	public string Technology { get; set; } = string.Empty;

	/// <summary>
	/// Module tags.
	/// </summary>
	[DataMember(Name = "tags")]
	public string Tags { get; set; } = string.Empty;

	/// <summary>
	/// The module group path.
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// Optional origin registry id for Exchange-backed modules.
	/// </summary>
	[DataMember(Name = "originRegistryId")]
	public string OriginRegistryId { get; set; } = string.Empty;

	/// <summary>
	/// Script type, typically groovy or powershell.
	/// </summary>
	[DataMember(Name = "scriptType")]
	public string ScriptType { get; set; } = string.Empty;

	/// <summary>
	/// Whether the module is currently in use.
	/// </summary>
	[DataMember(Name = "inUse")]
	public string InUse { get; set; } = string.Empty;

	/// <summary>
	/// Diagnostics source value returned by the API.
	/// </summary>
	[DataMember(Name = "source")]
	public string Source { get; set; } = string.Empty;

	/// <summary>
	/// Data type reported by the API.
	/// </summary>
	[DataMember(Name = "dataType")]
	public int? DataType { get; set; }

	/// <summary>
	/// Installation status values.
	/// </summary>
	[DataMember(Name = "installationStatuses")]
	public List<string> InstallationStatuses { get; set; } = [];

	/// <inheritdoc />
	public string Endpoint() => "setting/diagnosticsources";
}