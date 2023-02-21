namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AzureEmergingIssueEventSource
/// </summary>

[DataContract]
public class AzureEmergingIssueEventSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule", IsRequired = false)]
	public int Schedule { get; set; }
}
