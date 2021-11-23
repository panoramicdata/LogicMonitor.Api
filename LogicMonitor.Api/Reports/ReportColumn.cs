namespace LogicMonitor.Api.Reports;

/// <summary>
/// Report column
/// </summary>
[DataContract]
public class ReportColumn
{
	/// <summary>
	/// The column name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// Whether the column is hidden
	/// </summary>
	[DataMember(Name = "isHidden")]
	public bool IsHidden { get; set; }
}
