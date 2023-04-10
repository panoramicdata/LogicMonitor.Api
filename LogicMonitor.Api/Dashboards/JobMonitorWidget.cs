namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A job monitor widget
/// </summary>
[DataContract]
public class JobMonitorWidget : Widget, IWidget
{
	/// <summary>
	///     The device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///     The device group display name
	/// </summary>
	[DataMember(Name = "groupDisplayName")]
	public string DeviceGroupDisplayName { get; set; } = string.Empty;

	/// <summary>
	///     The batch job name
	/// </summary>
	[DataMember(Name = "batchJobName")]
	public string BatchJobName { get; set; } = string.Empty;

	/// <summary>
	///     The BatchJob Id
	/// </summary>
	[DataMember(Name = "batchJobId")]
	public string BatchJobId { get; set; } = string.Empty;

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
