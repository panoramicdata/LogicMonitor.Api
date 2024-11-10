namespace LogicMonitor.Api.Reports;

/// <summary>
/// A ResourceGroup inventory report
/// </summary>
[DataContract]
public class ResourceGroupInventoryReport : ReportBase
{
	/// <summary>
	/// The comma-separated ResourceGroups
	/// </summary>
	[DataMember(Name = "hostGroups")]
	public string ResourceGroups { get; set; } = string.Empty;

	/// <summary>
	/// Whether to include sub groups
	/// </summary>
	[DataMember(Name = "includeSubGroups")]
	public bool IncludeSubGroups { get; set; }

	/// <summary>
	/// The columns to sort by
	/// </summary>
	[DataMember(Name = "sortedBy")]
	public string SortedBy { get; set; } = string.Empty;

	/// <summary>
	/// The properties
	/// </summary>
	[DataMember(Name = "properties")]
	public List<string> Properties { get; set; } = [];

	/// <summary>
	/// The metrics
	/// </summary>
	public List<DeviceInventoryReportMetric> Metrics { get; set; } = [];
}
