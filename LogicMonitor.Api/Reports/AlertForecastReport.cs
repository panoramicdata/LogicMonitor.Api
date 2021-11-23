namespace LogicMonitor.Api.Reports;

/// <summary>
/// An alert forecast report
/// </summary>
[DataContract]
public class AlertForecastReport : DateRangeReport
{
	/// <summary>
	/// The confidence level
	/// </summary>
	[DataMember(Name = "confidenceLevel")]
	public int ConfidenceLevelPercent { get; set; }

	/// <summary>
	/// The hostsVal
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string DevicesValue { get; set; }

	/// <summary>
	/// The hostsValType
	/// </summary>
	[DataMember(Name = "hostsValType")]
	public string DevicesValueType { get; set; }

	/// <summary>
	/// The algorithm
	/// </summary>
	[DataMember(Name = "algorithm")]
	public string Algorithm { get; set; }

	/// <summary>
	/// The columns to sort by
	/// </summary>
	[DataMember(Name = "sortedBy")]
	public string SortedBy { get; set; }

	/// <summary>
	/// Whether to exclude global
	/// </summary>
	[DataMember(Name = "top10only")]
	public bool TopTenOnly { get; set; }

	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<AlertForecastReportMetric> Metrics { get; set; }

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; }
}
