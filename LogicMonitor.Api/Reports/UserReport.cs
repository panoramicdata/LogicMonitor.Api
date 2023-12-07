namespace LogicMonitor.Api.Reports;

/// <summary>
/// A user report
/// </summary>
[DataContract]
public class UserReport : ReportBase
{
	/// <summary>
	/// The user filter
	/// </summary>
	[DataMember(Name = "userFilter")]
	public UserReportUserFilter UserFilter { get; set; } = new();

	/// <summary>
	/// The sorted-by column
	/// </summary>
	[DataMember(Name = "sortedBy")]
	public string SortedBy { get; set; } = string.Empty;

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = [];
}
