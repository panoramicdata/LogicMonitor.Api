namespace LogicMonitor.Api.Logs;

/// <summary>
/// The window over which a log alert counts matches
/// </summary>
[DataContract]
public class LogPipelineProcessorWindowConfig
{
	/// <summary>
	/// The count
	/// </summary>
	[DataMember(Name = "count")]
	public int Count { get; set; }

	/// <summary>
	/// The duration
	/// </summary>
	[DataMember(Name = "duration")]
	public string Duration { get; set; } = string.Empty;
}
