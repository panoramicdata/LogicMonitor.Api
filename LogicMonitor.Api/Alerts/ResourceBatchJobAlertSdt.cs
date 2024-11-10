namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Resource Batch Job SDT
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
	/// The Resource display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }
}
