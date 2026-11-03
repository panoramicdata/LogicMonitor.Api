namespace LogicMonitor.Api.Logs;

/// <summary>
/// How a log alert groups alerts
/// </summary>
[DataContract]
public class LogPipelineProcessorGroupingConfig
{
	/// <summary>
	/// The alert entities
	/// </summary>
	[DataMember(Name = "alertEntities")]
	public List<string> AlertEntities { get; set; } = [];
}
