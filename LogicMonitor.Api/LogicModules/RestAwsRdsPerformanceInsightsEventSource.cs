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
	[DataMember(Name = "schedule", IsRequired = false)]
	public int Schedule { get; set; }

	/// <summary>
	/// Maximum number of items to return
	/// </summary>
	[DataMember(Name = "num", IsRequired = false)]
	public int Num { get; set; }

	/// <summary>
	/// The dimension to query
	/// </summary>
	[DataMember(Name = "dimension", IsRequired = false)]
	public string Dimension { get; set; } = string.Empty;
}
