namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// IPMIEventSource
/// </summary>

[DataContract]
public class IPMIEventSource : EventSource
{
	/// <summary>
	/// impmi check interval
	/// </summary>
	[DataMember(Name = "checkInterval")]
	public int CheckInterval { get; set; }
}
