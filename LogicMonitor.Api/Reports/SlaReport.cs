using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports;

/// <summary>
/// A service level agreement report
/// </summary>
[DataContract]
public class SlaReport : DateRangeReport
{
	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<SlaReportMetric> Metrics { get; set; }

	/// <summary>
	/// The dayInOneWeek
	/// </summary>
	[DataMember(Name = "dayInOneWeek")]
	public string DayInOneWeek { get; set; }

	/// <summary>
	/// The periodInOneDay
	/// </summary>
	[DataMember(Name = "periodInOneDay")]
	public string PeriodInOneDay { get; set; }

	/// <summary>
	/// The timezone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; }

	/// <summary>
	/// The displaySummary
	/// </summary>
	[DataMember(Name = "displaySummary")]
	public bool DisplaySummary { get; set; }

	/// <summary>
	/// The unmonitoredTime
	/// </summary>
	[DataMember(Name = "unmonitoredTime")]
	public int UnmonitoredTime { get; set; }

	/// <summary>
	/// The displayWithAvailability
	/// </summary>
	[DataMember(Name = "displayWithAvailability")]
	public bool DisplayWithAvailability { get; set; }

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; }
}
