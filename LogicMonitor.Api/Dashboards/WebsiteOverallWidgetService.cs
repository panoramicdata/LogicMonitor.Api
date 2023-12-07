namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Website specification for the WebsiteOverallWidget
/// </summary>
[DataContract]
public class WebsiteOverallWidgetWebsite
{
	/// <summary>
	/// The Website group id
	/// </summary>
	[DataMember(Name = "websiteGroupId")]
	public int WebsiteGroupId { get; set; }

	/// <summary>
	/// The Website group name
	/// </summary>
	[DataMember(Name = "websiteGroupName")]
	public string WebsiteGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The Website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; } = string.Empty;

	/// <summary>
	/// Whether all are chosen
	/// </summary>
	[DataMember(Name = "chooseAll")]
	public bool AreAllChosen { get; set; }

	/// <summary>
	/// The Website locations
	/// </summary>
	[DataMember(Name = "websites")]
	public List<WebsiteOverallWidgetWebsiteDetail> Websites { get; set; } = [];
}
