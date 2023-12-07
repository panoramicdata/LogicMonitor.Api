namespace LogicMonitor.Api.Reports;

/// <summary>
/// A host metric trends report
/// </summary>
[DataContract]
public class DeviceMetricTrendsReport : DateRangeReport
{
	/// <summary>
	/// The hostsVal
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string HostsVal { get; set; } = string.Empty;

	/// <summary>
	/// The hostsValType
	/// </summary>
	[DataMember(Name = "hostsValType")]
	public string HostsValType { get; set; } = string.Empty;

	/// <summary>
	/// Whether this is Base 1024
	/// </summary>
	[DataMember(Name = "isBase1024")]
	public bool IsBase1024 { get; set; }

	/// <summary>
	/// The columns to sort by
	/// </summary>
	[DataMember(Name = "sortedBy")]
	public string SortedBy { get; set; } = string.Empty;

	/// <summary>
	/// The rowFormat
	/// </summary>
	[DataMember(Name = "rowFormat")]
	public int RowFormat { get; set; }

	/// <summary>
	/// Whether to only show the top 10
	/// </summary>
	[DataMember(Name = "top10Only")]
	public bool Top10Only { get; set; }

	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<DeviceMetricTrendsReportMetric> Metrics { get; set; } = [];

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = [];
}
