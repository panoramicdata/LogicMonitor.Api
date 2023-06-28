namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    Map Point
/// </summary>
[JsonConverter(typeof(MapPointConverter))]
public class MapPoint
{
	/// <summary>
	///    The type
	/// </summary>
	public string Type { get; set; } = string.Empty;
}
