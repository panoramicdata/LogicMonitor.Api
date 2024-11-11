namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// RestAzureResourceLogAnalyticsWorkspacesSource
/// </summary>

[DataContract]
public class RestAzureResourceLogAnalyticsWorkspacesSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule")]
	public int Schedule { get; set; }
}
