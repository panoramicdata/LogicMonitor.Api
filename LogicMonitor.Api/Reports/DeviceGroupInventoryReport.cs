namespace LogicMonitor.Api.Reports;

/// <summary>
/// A host group inventory report
/// </summary>
[DataContract]
public class DeviceGroupInventoryReport : ReportBase
{
	/// <summary>
	/// The comma-separated device groups
	/// </summary>
	[DataMember(Name = "hostGroups")]
	public string DeviceGroups { get; set; } = string.Empty;

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
