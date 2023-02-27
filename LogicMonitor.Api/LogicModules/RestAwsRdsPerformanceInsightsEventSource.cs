namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// RestAwsRdsPerformanceInsightsEventSource
/// </summary>

[DataContract]
public class RestAwsRdsPerformanceInsightsEventSource : EventSource
{
	/// <summary>
	/// The polling interval for the EventSource
	/// </summary>
	[DataMember(Name = "schedule")]
	public int Schedule { get; set; }

	/// <summary>
	/// Maximum number of items to return
	/// </summary>
	[DataMember(Name = "num")]
	public int Num { get; set; }
}
