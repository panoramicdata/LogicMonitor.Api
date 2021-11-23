namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Device Batch Job SDT
/// </summary>
[DataContract]
public class DeviceBatchJobAlertSdt : AlertSdt
{
	/// <summary>
	/// The Batch Job name
	/// </summary>
	[DataMember(Name = "batchJobName")]
	public string BatchJobName { get; set; }

	/// <summary>
	/// The Device Batch Job ID
	/// </summary>
	[DataMember(Name = "deviceBatchJobId")]
	public int DeviceBatchJobId { get; set; }

	/// <summary>
	/// The Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The Device ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }
}
