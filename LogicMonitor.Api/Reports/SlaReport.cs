namespace LogicMonitor.Api.Reports;

/// <summary>
/// A service level agreement report
/// </summary>
[DataContract]
public class SlaReport : DateRangeReport
{
	/// <summary>
	/// Calculation method: 0 \u003d percent all resources available, 1 \u003d average of all SLA metrics
	/// </summary>
	[DataMember(Name = "calculationMethod", IsRequired = false)]
	public CalculationMethod CalculationMethod { get; set; }

	/// <summary>
	/// The days of the week that the SLA report should take into account, where multiple values are separated by commas and * refers to all days of the week
	/// </summary>
	[DataMember(Name = "dayInOneWeek", IsRequired = false)]
	public string? DayInOneWeek { get; set; }

	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "metrics", IsRequired = true)]
	public List<SlaReportMetric> Metrics { get; set; } = null!;

	/// <summary>
	/// The hours of each selected day that the SLA report should take into account, where * refers to all hours
	/// </summary>
	[DataMember(Name = "periodInOneDay", IsRequired = false)]
	public string? PeriodInOneDay { get; set; }

	/// <summary>
	/// The specific timezone for the report
	/// </summary>
	[DataMember(Name = "timezone", IsRequired = false)]
	public string? TimeZone { get; set; }

	/// <summary>
	/// If true, the SLA summary (total %) will be displayed
	/// </summary>
	[DataMember(Name = "displaySummary", IsRequired = false)]
	public bool DisplaySummary { get; set; }

	/// <summary>
	/// 0|1|2 - How the time we have no data for the device should be counted, where 1 \u003d ignore no data (subtract from total time), 2 \u003d count as violation (subtract from uptime), 3 \u003d count as available (add to uptime)
	/// </summary>
	[DataMember(Name = "unmonitoredTime", IsRequired = false)]
	public int UnmonitoredTime { get; set; }

	/// <summary>
	/// If true, only devices with less than 100% availability will be displayed in the report
	/// </summary>
	[DataMember(Name = "displayWithAvailability", IsRequired = false)]
	public bool DisplayWithAvailability { get; set; }

	/// <summary>
	/// The columns displayed in the report
	/// </summary>
	[DataMember(Name = "columns", IsRequired = false)]
	public List<ReportColumn>? Columns { get; set; }
}
