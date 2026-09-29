namespace LogicMonitor.Api.Reports;

/// <summary>
/// An Advanced Metrics report, whose rows come from a metric query such as LMQL
/// </summary>
[DataContract]
public class AdvancedMetricsReport : DateRangeReport
{
	/// <summary>
	/// The query that selects the report's metrics
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// Where the metrics come from, for example lmql
	/// </summary>
	[DataMember(Name = "metricSource")]
	public string MetricSource { get; set; } = string.Empty;

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = [];
}
