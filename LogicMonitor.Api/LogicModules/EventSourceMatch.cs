namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An EventSource match
/// </summary>
public class EventSourceMatch
{
	/// <summary>
	/// The regex or plain text to look for in the file and trigger alert if found
	/// </summary>
	[DataMember(Name = "pattern", IsRequired = false)]
	public string Pattern { get; set; } = string.Empty;

	/// <summary>
	/// The level of alert to trigger: warn | error | critical
	/// </summary>
	[DataMember(Name = "alertLevel", IsRequired = false)]
	public string AlertLevel { get; set; } = string.Empty;
}
