namespace LogicMonitor.Api.Logs;

/// <summary>
/// How a log alert clears
/// </summary>
[DataContract]
public class LogPipelineProcessorClearConfig
{
	/// <summary>
	/// The auto clear after
	/// </summary>
	[DataMember(Name = "autoClearAfter")]
	public string AutoClearAfter { get; set; } = string.Empty;

	/// <summary>
	/// The auto clear on ack
	/// </summary>
	[DataMember(Name = "autoClearOnAck")]
	public bool AutoClearOnAck { get; set; }

	/// <summary>
	/// The clear query
	/// </summary>
	[DataMember(Name = "clearQuery")]
	public string ClearQuery { get; set; } = string.Empty;

	/// <summary>
	/// The grouping entities
	/// </summary>
	[DataMember(Name = "groupingEntities")]
	public List<string> GroupingEntities { get; set; } = [];
}
