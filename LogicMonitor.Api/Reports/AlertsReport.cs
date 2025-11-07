namespace LogicMonitor.Api.Reports;

/// <summary>
/// An alert report
/// </summary>
[DataContract]
public class AlertsReport : DateRangeReport
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
	/// The level
	/// </summary>
	[DataMember(Name = "level")]
	public string Level { get; set; } = string.Empty;

	/// <summary>
	/// The columns to sort by
	/// </summary>
	[DataMember(Name = "sortedBy")]
	public string SortedBy { get; set; } = string.Empty;

	/// <summary>
	/// The whether to include pre-existing
	/// </summary>
	[DataMember(Name = "includePreexist")]
	public bool IncludePreexist { get; set; }

	/// <summary>
	/// Whether to show active only
	/// </summary>
	[DataMember(Name = "activeOnly")]
	public bool ActiveOnly { get; set; }

	/// <summary>
	/// Whether to show summary only
	/// </summary>
	[DataMember(Name = "summaryOnly")]
	public bool SummaryOnly { get; set; }

	/// <summary>
	/// The acknowledgement filter value
	/// </summary>
	[DataMember(Name = "ackFilter")]
	public string AckFilter { get; set; } = string.Empty;

	/// <summary>
	/// The SDT filter value
	/// </summary>
	[DataMember(Name = "sdtFilter")]
	public string SdtFilter { get; set; } = string.Empty;

	/// <summary>
	/// The timing
	/// </summary>
	[DataMember(Name = "timing")]
	public string Timing { get; set; } = string.Empty;

	/// <summary>
	/// The datasource
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSource { get; set; } = string.Empty;

	/// <summary>
	/// The rule
	/// </summary>
	[DataMember(Name = "rule")]
	public string Rule { get; set; } = string.Empty;

	/// <summary>
	/// The chain
	/// </summary>
	[DataMember(Name = "chain")]
	public string Chain { get; set; } = string.Empty;

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = [];

	/// <summary>
	/// asc | desc
	/// </summary>
	[DataMember(Name = "sortedDirection")]
	public string SortDirection { get; set; } = string.Empty;

	/// <summary>
	/// all|yes|no|yes,no|no,yes\nall: return all anomaly, non anomaly and unknown anomaly alerts\nyes: only alerts which has anomaly will be displayed\nno: only alerts which has no anomaly will be displayed\nyes,no:  return all anomaly and non anomaly alerts
	/// </summary>
	[DataMember(Name = "anomaly")]
	public string Anomaly { get; set; } = string.Empty;

	/// <summary>
	/// The DependencyRole
	/// </summary>
	[DataMember(Name = "dependencyRole")]
	public string DependencyRole { get; set; } = string.Empty;

	/// <summary>
	/// The dependency routing state
	/// </summary>
	[DataMember(Name = "dependencyRoutingState")]
	public string DependencyRoutingState { get; set; } = string.Empty;

	/// <summary>
	/// The clear filter
	/// </summary>
	[DataMember(Name = "clearFilter")]
	public string ClearFilter { get; set; } = string.Empty;

	/// <summary>
	/// Whether to include historical SDT
	/// </summary>
	[DataMember(Name = "isHistoricalSDT")]
	public string IsHistoricalSDT { get; set; } = string.Empty;
}
