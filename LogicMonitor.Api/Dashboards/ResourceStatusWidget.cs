namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Device Status Widget
/// </summary>
[DataContract]
public class ResourceStatusWidget : Widget, IWidget
{
	/// <summary>
	/// The Device ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public string DeviceId { get; set; } = string.Empty;

	/// <summary>
	/// The Device Display Name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Display Settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}