namespace LogicMonitor.Api.Reports;

/// <summary>
/// A report
/// </summary>
[DebuggerDisplay("{Type}:{Name}")]
[JsonConverter(typeof(ReportConverter))]
[DataContract]
public class ReportBase : NamedItem, IHasEndpoint
{
	/// <summary>
	/// The report type. Acceptable values are: Alert,Alert SLA,Alert threshold,Alert trends,Host CPU,Host group inventory,Host inventory,Host metric trends,Interfaces Bandwidth,Netflow device metric,Service Level Agreement,Website Service Overview,Word template,Audit Log,Alert Forecasting,Dashboard,Website SLA,User,Role
	/// </summary>
	[DataMember(Name = "type", IsRequired = true)]
	public string Type { get; set; } = null!;

	/// <summary>
	/// The type alias
	/// </summary>
	[DataMember(Name = "typeAlias", IsRequired = false)]
	public string? TypeAlias { get; set; }

	/// <summary>
	/// The Id of the group the report is in, where Id\u003d0 is the root report group
	/// </summary>
	[DataMember(Name = "groupId", IsRequired = false)]
	public int GroupId { get; set; }

	/// <summary>
	/// The format of the report. Allowable values are: HTML, PDF, CSV, WORD
	/// </summary>
	[DataMember(Name = "format", IsRequired = false)]
	public string? Format { get; set; }

	/// <summary>
	/// Whether or not the report is configured to be delivered via email. Acceptable values are: none, email
	/// </summary>
	[DataMember(Name = "delivery", IsRequired = false)]
	public string? Delivery { get; set; }

	/// <summary>
	/// If the report is configured to be delivered via email, this object provides the recipients that the report will be delivered to
	/// </summary>
	[DataMember(Name = "recipients", IsRequired = false)]
	public List<ReportRecipient>? Recipients { get; set; }

	/// <summary>
	/// A cron schedule that indicates when the report will be delivered via email
	/// </summary>
	[DataMember(Name = "schedule", IsRequired = false)]
	public string? Schedule { get; set; }

	/// <summary>
	/// The specific timezone for the scheduled report
	/// </summary>
	[DataMember(Name = "scheduleTimezone", IsRequired = false)]
	public string? ScheduleTimezone { get; set; }

	/// <summary>
	/// The Id of the user that last modified the report
	/// </summary>
	[DataMember(Name = "lastmodifyUserId", IsRequired = false)]
	public int LastmodifyUserId { get; set; }

	/// <summary>
	/// The username of the user that last modified the report
	/// </summary>
	[DataMember(Name = "lastmodifyUserName", IsRequired = false)]
	public string? LastmodifyUserName { get; set; }

	/// <summary>
	/// Whether or not other users are allowed to view the report as the user who last modified the report
	/// </summary>
	[DataMember(Name = "enableViewAsOtherUser", IsRequired = false)]
	public bool EnableViewAsOtherUser { get; set; }

	/// <summary>
	/// The permissions associated with the user who made the API call
	/// </summary>
	[DataMember(Name = "userPermission", IsRequired = false)]
	public UserPermission UserPermission { get; set; }

	/// <summary>
	/// The time, in epoch format, that the report was last generated
	/// </summary>
	[DataMember(Name = "lastGenerateOn", IsRequired = false)]
	public long LastGenerateOn { get; set; }

	/// <summary>
	/// The size of the report, in Bytes, the last time it was generated
	/// </summary>
	[DataMember(Name = "lastGenerateSize", IsRequired = false)]
	public long LastGenerateSize { get; set; }

	/// <summary>
	/// The number of pages in the report, the last time it was generated
	/// </summary>
	[DataMember(Name = "lastGeneratePages", IsRequired = false)]
	public int LastGeneratePages { get; set; }

	/// <summary>
	/// The id of the custom report template, if the report is a custom report. An id of 0 indicates that the report is not a custom report
	/// </summary>
	[DataMember(Name = "customReportTypeId", IsRequired = false)]
	public int CustomReportTypeId { get; set; }

	/// <summary>
	/// The name of the custom report template
	/// </summary>
	[DataMember(Name = "customReportTypeName", IsRequired = false)]
	public string? CustomReportTypeName { get; set; }

	/// <summary>
	/// The report link Expire. Allowable values are:High Flexibility,High Security
	/// </summary>
	[DataMember(Name = "reportLinkExpire", IsRequired = false)]
	public string? ReportLinkExpire { get; set; }

	/// <summary>
	/// The number of links associated with the report, where each link corresponds to a generated report
	/// </summary>
	[DataMember(Name = "reportLinkNum", IsRequired = false)]
	public int ReportLinkNum { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "report/reports";
}
