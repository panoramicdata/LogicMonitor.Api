namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A NOC widget
/// </summary>
[DataContract]
public class NocWidget : Widget, IWidget
{
	/// <summary>
	/// The Alert Level
	/// </summary>
	[DataMember(Name = "level")]
	public AlertLevel AlertLevel { get; set; }

	/// <summary>
	/// The device type
	/// </summary>
	[DataMember(Name = "hostsType")]
	public string DeviceType { get; set; } = string.Empty;

	/// <summary>
	/// The device type
	/// </summary>
	[DataMember(Name = "hosts")]
	public string Devices { get; set; } = string.Empty;

	/// <summary>
	/// The Items
	/// </summary>
	[DataMember(Name = "items")]
	public List<NocWidgetItem> Items { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
