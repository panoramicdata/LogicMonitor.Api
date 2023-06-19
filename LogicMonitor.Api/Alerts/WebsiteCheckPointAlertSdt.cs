namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Website Checkpoint SDT
/// </summary>
[DataContract]
public class WebsiteCheckpointAlertSdt : AlertSdt
{
	/// <summary>
	/// The Checkpoint ID
	/// </summary>
	[DataMember(Name = "checkpointId")]
	public int CheckpointId { get; set; }

	/// <summary>
	/// The Checkpoint name
	/// </summary>
	[DataMember(Name = "checkpointName")]
	public string CheckpointName { get; set; } = string.Empty;

	/// <summary>
	/// The Checkpoint name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; } = string.Empty;
}
