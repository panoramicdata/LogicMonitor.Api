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
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[Obsolete("Use ResourceDisplayName", true)]
	public string DeviceDisplayName => ResourceDisplayName;

	/// <summary>
	///     The ResourceGroup display name
	/// </summary>
	[DataMember(Name = "groupDisplayName")]
	public string ResourceGroupDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[Obsolete("Use ResourceGroupDisplayName", true)]
	public string DeviceGroupDisplayName => ResourceGroupDisplayName;

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
