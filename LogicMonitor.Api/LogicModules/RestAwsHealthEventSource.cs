namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// RestAwsHealthEventSource
/// </summary>

[DataContract]
public class RestAwsHealthEventSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule", IsRequired = false)]
	public int Schedule { get; set; }
}
