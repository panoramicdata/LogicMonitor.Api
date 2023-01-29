namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Google Map widget creation DTO
/// </summary>
[DataContract]
public class GoogleMapWidgetCreationDto : WidgetCreationDto<GoogleMapWidget>
{
	/// <summary>
	///     The name
	/// </summary>
	public override string Type { get; } = "gmap";

	/// <summary>
	///     The name
	/// </summary>
	[DataMember(Name = "widgetTokens")]
	public List<EntityProperty> CustomProperties { get; set; }

	/// <summary>
	///     The name
	/// </summary>
	[DataMember(Name = "mapPoints")]
	public List<MapPoint> MapPoints { get; set; }
}
