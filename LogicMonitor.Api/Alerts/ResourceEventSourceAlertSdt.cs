namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Resource EventSource SDT
/// </summary>
[DataContract]
public class ResourceEventSourceAlertSdt : AlertSdt
{
	/// <summary>
	/// The Resource display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource EventSource ID
	/// </summary>
	[DataMember(Name = "deviceEventSourceId")]
	public int ResourceEventSourceId { get; set; }

	/// <summary>
	/// The Resource ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The EventSource name
	/// </summary>
	[DataMember(Name = "eventSourceName")]
	public string EventSourceName { get; set; } = string.Empty;
}