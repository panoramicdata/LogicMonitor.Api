namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Push Metric response's resource ids
/// </summary>
[DataContract]
public class PushMetricResponseResourceIds
{
	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "system.deviceId")]
	public string ResourceId { get; set; } = string.Empty;
}
