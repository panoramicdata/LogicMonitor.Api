namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Resource EventSource SDT
/// </summary>
[DataContract]
public class ResourceEventSourceAlertSdt : AlertSdt
{
	/// <summary>
	/// The Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDisplayName instead", true)]
	public string DeviceDisplayName => ResourceDisplayName;

	/// <summary>
	/// The Resource EventSource ID
	/// </summary>
	[DataMember(Name = "deviceEventSourceId")]
	public int ResourceEventSourceId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceEventSourceId instead", true)]
	public int DeviceEventSourceId => ResourceEventSourceId;

	/// <summary>
	/// The Resource ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceId instead", true)]
	public int DeviceId => ResourceId;

	/// <summary>
	/// The EventSource name
	/// </summary>
	[DataMember(Name = "eventSourceName")]
	public string EventSourceName { get; set; } = string.Empty;
}