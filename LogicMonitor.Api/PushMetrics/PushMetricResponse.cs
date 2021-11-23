namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Push Metric response
/// </summary>
[DataContract]
public class PushMetricResponse
{
	/// <summary>
	/// The message
	/// </summary>
	[DataMember(Name = "message")]
	public string Message { get; set; }

	/// <summary>
	/// The resource ids
	/// </summary>
	[DataMember(Name = "resourceIds")]
	public PushMetricResponseResourceIds ResourceIds { get; set; }

	/// <summary>
	/// The timestamp
	/// </summary>
	[DataMember(Name = "timestamp")]
	public long Timestamp { get; set; }
}
