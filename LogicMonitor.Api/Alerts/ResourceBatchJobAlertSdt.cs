namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Device Batch Job SDT
/// </summary>
[DataContract]
public class ResourceBatchJobAlertSdt : AlertSdt
{
	/// <summary>
	/// The Batch Job name
	/// </summary>
	[DataMember(Name = "batchJobName")]
	public string BatchJobName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource Batch Job ID
	/// </summary>
	[DataMember(Name = "deviceBatchJobId")]
	public int ResourceBatchJobId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceBatchJobId instead", true)]
	public int DeviceBatchJobId => ResourceBatchJobId;

	/// <summary>
	/// The Resource display name
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
	/// The Device ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceId instead", true)]
	public int DeviceId => ResourceId;
}
