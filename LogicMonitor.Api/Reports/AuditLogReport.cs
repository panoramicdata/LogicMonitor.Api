namespace LogicMonitor.Api.Reports;

/// <summary>
/// An audit log report
/// </summary>
[DataContract]
public class AuditLogReport : DateRangeReport
{
	/// <summary>
	/// The columns to sort by
	/// </summary>
	[DataMember(Name = "sortedBy")]
	public string SortedBy { get; set; } = string.Empty;

	/// <summary>
	/// The filter
	/// </summary>
	[DataMember(Name = "filter")]
	public string Filter { get; set; } = string.Empty;

	/// <summary>
	/// The username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; } = string.Empty;

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = new();
}
