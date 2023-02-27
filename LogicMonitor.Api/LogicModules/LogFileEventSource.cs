namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// LogFileEventSource
/// </summary>

[DataContract]
public class LogFileEventSource : EventSource
{
	/// <summary>
	/// log files
	/// </summary>
	[DataMember(Name = "logFiles")]
	public List<LogFile> LogFiles { get; set; } = new();
}
