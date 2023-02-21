namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A NOC widget
/// </summary>
[DataContract]
public class NocWidget : Widget
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
	public string DeviceType { get; set; }

	/// <summary>
	/// The device type
	/// </summary>
	[DataMember(Name = "hosts")]
	public string Devices { get; set; }

	/// <summary>
	/// The Items
	/// </summary>
	[DataMember(Name = "items")]
	public List<NocWidgetItem> Items { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; }
}
