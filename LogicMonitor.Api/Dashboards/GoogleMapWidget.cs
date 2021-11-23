using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Google Map widget
/// </summary>
[DataContract]
public class GoogleMapWidget : Widget
{
	/// <summary>
	/// The map points
	/// </summary>
	[DataMember(Name = "mapPoints")]
	public List<MapPoint> MapPoints { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }
}
