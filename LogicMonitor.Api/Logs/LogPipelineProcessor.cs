namespace LogicMonitor.Api.Logs;

/// <summary>
/// A log pipeline processor (LogAlert)
/// </summary>
[DataContract]
public class LogPipelineProcessor : IdentifiedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	/// The alert query
	/// </summary>
	[DataMember(Name = "alertQuery")]
	public string AlertQuery { get; set; } = string.Empty;

	/// <summary>
	/// The alert type
	/// </summary>
	[DataMember(Name = "alertType")]
	public string AlertType { get; set; } = string.Empty;

	/// <summary>
	/// The clear
	/// </summary>
	[DataMember(Name = "clear")]
	public LogPipelineProcessorClearConfig? Clear { get; set; }

	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The disabled
	/// </summary>
	[DataMember(Name = "disabled")]
	public bool Disabled { get; set; }

	/// <summary>
	/// The grouping
	/// </summary>
	[DataMember(Name = "grouping")]
	public LogPipelineProcessorGroupingConfig? Grouping { get; set; }

	/// <summary>
	/// The log metadata fields
	/// </summary>
	[DataMember(Name = "logMetadataFields")]
	public List<string> LogMetadataFields { get; set; } = [];

	/// <summary>
	/// The log notes
	/// </summary>
	[DataMember(Name = "logNotes")]
	public string LogNotes { get; set; } = string.Empty;

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The pipeline id
	/// </summary>
	[DataMember(Name = "pipelineId")]
	public int PipelineId { get; set; }

	/// <summary>
	/// The severity
	/// </summary>
	[DataMember(Name = "severity")]
	public string Severity { get; set; } = string.Empty;

	/// <summary>
	/// The window
	/// </summary>
	[DataMember(Name = "window")]
	public LogPipelineProcessorWindowConfig? Window { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "logpipelines/processors";
}
