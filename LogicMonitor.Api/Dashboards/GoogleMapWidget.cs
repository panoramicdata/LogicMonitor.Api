namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Google Map widget
/// </summary>
[DataContract]
public class GoogleMapWidget : Widget, IWidget
{
	/// <summary>
	/// The map points
	/// </summary>
	[DataMember(Name = "mapPoints")]
	public List<MapPoint> MapPoints { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
