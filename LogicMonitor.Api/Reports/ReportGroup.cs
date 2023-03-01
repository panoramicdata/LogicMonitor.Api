namespace LogicMonitor.Api.Reports;

/// <summary>
/// A report group
/// </summary>
[DataContract]
public class ReportGroup : NamedItem, IHasEndpoint
{
	/// <summary>
	/// The reports count of this group
	/// </summary>
	[DataMember(Name = "reportsCount")]
	public int ReportCount { get; set; }

	/// <summary>
	/// The matched reports count of this group
	/// </summary>
	[DataMember(Name = "matchedReportCount")]
	public int MatchedReportCount { get; set; }

	/// <summary>
	/// The user permission on the report group
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermissionValues UserPermission { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "report/groups";
}
