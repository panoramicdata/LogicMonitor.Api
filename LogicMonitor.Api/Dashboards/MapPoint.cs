namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    Map Point
/// </summary>
[DataContract]
[JsonConverter(typeof(MapPointConverter))]
public class MapPoint
{
	/// <summary>
	///    The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;
}
