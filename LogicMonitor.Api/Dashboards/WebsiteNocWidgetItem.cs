namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Website NOC widget item
/// </summary>
[DataContract]
public class WebsiteNocWidgetItem
{
	/// <summary>
	/// Website group name
	/// </summary>
	[DataMember(Name = "websiteGroupName")]
	public string WebsiteGroupName { get; set; }

	/// <summary>
	/// Website group name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; }

	/// <summary>
	/// Website group name
	/// </summary>
	[DataMember(Name = "groupBy")]
	public string GroupBy { get; set; }

	/// <summary>
	/// What to display
	/// </summary>
	[DataMember(Name = "name")]
	public string DisplayText { get; set; }
}
