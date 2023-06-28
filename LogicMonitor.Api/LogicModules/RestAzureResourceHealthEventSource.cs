namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// RestAzureResourceHealthEventSource
/// </summary>

[DataContract]
public class RestAzureResourceHealthEventSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule")]
	public int Schedule { get; set; }
}
