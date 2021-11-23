namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An EventSource match
/// </summary>
public class EventSourceMatch
{
	/// <summary>
	/// Pattern
	/// </summary>
	[DataMember(Name = "pattern")]
	public string Pattern { get; set; }

	/// <summary>
	/// AlertLevel
	/// </summary>
	[DataMember(Name = "alertLevel")]
	public AlertLevel AlertLevel { get; set; }
}
