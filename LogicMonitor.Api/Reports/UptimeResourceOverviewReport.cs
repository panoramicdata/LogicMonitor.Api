namespace LogicMonitor.Api.Reports;

/// <summary>
/// An Uptime Resource Overview report.
///
/// Carries the same type-specific fields as <see cref="WebsiteOverviewReport"/> - confirmed
/// against a live portal rather than assumed - but reports on resources rather than websites,
/// so it is a sibling rather than a subclass.
/// </summary>
[DataContract]
public class UptimeResourceOverviewReport : DateRangeReport
{
	/// <summary>
	/// The items
	/// </summary>
	[DataMember(Name = "items")]
	public string Items { get; set; } = string.Empty;

	/// <summary>
	/// The items type
	/// </summary>
	[DataMember(Name = "itemsType")]
	public string ItemsType { get; set; } = string.Empty;

	/// <summary>
	/// The displayType
	/// </summary>
	[DataMember(Name = "displayType")]
	public int DisplayType { get; set; }

	/// <summary>
	/// The filter
	/// </summary>
	[DataMember(Name = "includeTypes")]
	public List<object> IncludeTypes { get; set; } = [];

	/// <summary>
	/// Whether to exclude SDT
	/// </summary>
	[DataMember(Name = "excludeSDT")]
	public bool ExcludeSdt { get; set; }

	/// <summary>
	/// Whether to exclude resources with 100% availability
	/// </summary>
	[DataMember(Name = "exclude100Availability")]
	public bool Exclude100Availability { get; set; }
}
