namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Website overview widget
/// </summary>
[DataContract]
public class WebsiteOverviewWidget : Widget, IWidget
{
	/// <summary>
	///     The Website Id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int WebsiteId { get; set; }

	/// <summary>
	///     The graph name
	/// </summary>
	[DataMember(Name = "graph")]
	public string GraphName { get; set; } = string.Empty;

	/// <summary>
	///     The geographic information
	/// </summary>
	[DataMember(Name = "geoInfo")]
	public string GeographInformation { get; set; } = string.Empty;

	/// <summary>
	///     The Website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; } = string.Empty;

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
