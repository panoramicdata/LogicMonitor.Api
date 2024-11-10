namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Resource Status Widget
/// </summary>
[DataContract]
public class ResourceStatusWidget : Widget, IWidget
{
	/// <summary>
	/// The Resource ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public string ResourceId { get; set; } = string.Empty;

	/// <summary>
	/// The Resource Display Name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Display Settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}