namespace LogicMonitor.Api.Reports;

/// <summary>
/// An alert threshold report
/// </summary>
[DataContract]
public class AlertsThresholdsReport : ReportBase
{
	/// <summary>
	/// The group full path
	/// </summary>
	[DataMember(Name = "groupFullPath")]
	public string GroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// The Resource display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The dataSourceInstanceName
	/// </summary>
	[DataMember(Name = "dataSourceInstanceName")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The dataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; } = string.Empty;

	/// <summary>
	/// The columns to sort by
	/// </summary>
	[DataMember(Name = "sortedBy")]
	public string SortedBy { get; set; } = string.Empty;

	/// <summary>
	/// Whether to exclude global
	/// </summary>
	[DataMember(Name = "excludeGlobal")]
	public bool IncludePreexist { get; set; }

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = [];
}
