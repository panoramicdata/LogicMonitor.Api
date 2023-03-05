namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// An Website Graph widget
/// </summary>
[DataContract]
public class WebsiteGraphWidget : GraphWidget
{
	/// <summary>
	/// The Website checkpoint id
	/// </summary>
	[DataMember(Name = "checkpointId")]
	public int WebsiteCheckPointId { get; set; }

	/// <summary>
	/// The graph name
	/// </summary>
	[DataMember(Name = "graph")]
	public string GraphName { get; set; } = string.Empty;

	/// <summary>
	/// The website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; } = string.Empty;

	/// <summary>
	/// The geographic information
	/// </summary>
	[DataMember(Name = "geoInfo")]
	public string GeographicInformation { get; set; } = string.Empty;

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
