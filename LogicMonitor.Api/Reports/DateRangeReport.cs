namespace LogicMonitor.Api.Reports;

/// <summary>
/// A date range report
/// </summary>
public abstract class DateRangeReport : ReportBase
{
	/// <summary>
	/// The date range
	/// </summary>
	[DataMember(Name = "dateRange")]
	public string DateRange { get; set; }
}
