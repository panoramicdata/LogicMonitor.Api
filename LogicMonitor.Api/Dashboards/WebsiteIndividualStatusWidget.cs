namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Website individual status widget
/// </summary>
[DataContract]
public class WebsiteIndividualStatusWidget : Widget, IWidget
{
	/// <summary>
	/// The Website group id
	/// </summary>
	[DataMember(Name = "websiteGroupId")]
	public int WebsiteGroupId { get; set; }

	/// <summary>
	/// The website id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int WebsiteId { get; set; }

	/// <summary>
	/// The graph name
	/// </summary>
	[DataMember(Name = "graph")]
	public string GraphName { get; set; } = string.Empty;

	/// <summary>
	/// The Website group name
	/// </summary>
	[DataMember(Name = "websiteGroupName")]
	public string WebsiteGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; } = string.Empty;

	/// <summary>
	/// The Website locations
	/// </summary>
	[DataMember(Name = "locations")]
	public List<WebsiteCheckpointSelection> Locations { get; set; } = [];

	/// <summary>
	/// Whether the Website is internal
	/// </summary>
	[DataMember(Name = "isInternal")]
	public bool IsInternal { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
