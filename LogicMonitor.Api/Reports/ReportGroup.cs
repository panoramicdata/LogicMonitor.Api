namespace LogicMonitor.Api.Reports;

/// <summary>
/// A report group
/// </summary>
[DataContract]
public class ReportGroup : NamedItem, IHasEndpoint
{
	/// <summary>
	/// Child ReportGroups
	/// </summary>
	[DataMember(Name = "subGroups")]
	public List<ReportGroup> SubGroups { get; set; }

	/// <summary>
	/// Reports
	/// </summary>
	[DataMember(Name = "reports")]
	public List<ReportBase> Reports { get; set; }

	/// <summary>
	/// Report count
	/// </summary>
	[DataMember(Name = "reportsCount")]
	public int ReportCount { get; set; }

	/// <summary>
	/// Matched report count
	/// </summary>
	[DataMember(Name = "matchedReportCount")]
	public int MatchedReportCount { get; set; }

	/// <summary>
	/// User permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "report/groups";
}
