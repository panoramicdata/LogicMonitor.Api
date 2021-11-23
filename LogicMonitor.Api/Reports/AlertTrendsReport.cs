namespace LogicMonitor.Api.Reports;

/// <summary>
/// An alert trends report
/// </summary>
[DataContract]
public class AlertTrendsReport : DateRangeReport
{
	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<AlertTrendsReportMetric> Metrics { get; set; }
}
