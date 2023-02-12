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
	[DataMember(Name = "reportsCount", IsRequired = false)]
	public int ReportCount { get; set; }

	/// <summary>
	/// The matched reports count of this group
	/// </summary>
	[DataMember(Name = "matchedReportCount", IsRequired = false)]
	public int MatchedReportCount { get; set; }

	/// <summary>
	/// The user permission on the report group
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "report/groups";
}
