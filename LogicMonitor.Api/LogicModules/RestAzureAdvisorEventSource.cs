namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// RestAzureAdvisorEventSource
/// </summary>

[DataContract]
public class RestAzureAdvisorEventSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule")]
	public int Schedule { get; set; }
}
