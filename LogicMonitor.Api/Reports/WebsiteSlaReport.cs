namespace LogicMonitor.Api.Reports;

/// <summary>
/// An Website SLA report
/// </summary>
[DataContract]
public class WebsiteSlaReport : DateRangeReport
{
	/// <summary>
	/// The dayInOneWeek
	/// </summary>
	[DataMember(Name = "dayInOneWeek")]
	public string DayInOneWeek { get; set; } = string.Empty;

	/// <summary>
	/// The periodInOneDay
	/// </summary>
	[DataMember(Name = "periodInOneDay")]
	public string PeriodInOneDay { get; set; } = string.Empty;

	/// <summary>
	/// The timezone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<WebsiteSlaReportMetric> Metrics { get; set; } = new();
}
