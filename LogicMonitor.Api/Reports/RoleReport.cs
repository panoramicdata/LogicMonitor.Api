namespace LogicMonitor.Api.Reports;

/// <summary>
/// A role report
/// </summary>
[DataContract]
public class RoleReport : ReportBase
{
	/// <summary>
	/// The display format
	/// </summary>
	[DataMember(Name = "displayFormat")]
	public string DisplayFormat { get; set; } = string.Empty;

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = new();
}
