namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Website overall status widget
/// </summary>
[DataContract]
public class WebsiteOverallStatusWidget : Widget
{
	/// <summary>
	/// The Website select mode
	/// </summary>
	[DataMember(Name = "websiteSelectMode")]
	public string WebsiteSelectMode { get; set; }

	/// <summary>
	/// The selected Websites
	/// </summary>
	[DataMember(Name = "selectedWebsites")]
	public List<WebsiteOverallWidgetWebsite> SelectedWebsites { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }

	/// <summary>
	///     The items
	/// </summary>
	[DataMember(Name = "items")]
	public List<object> Items { get; set; }
}
