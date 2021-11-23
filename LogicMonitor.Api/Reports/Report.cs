using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports;

/// <summary>
/// A report
/// </summary>
[DebuggerDisplay("{Type}:{Name}")]
[JsonConverter(typeof(ReportConverter))]
[DataContract]
public class Report : NamedItem, IHasEndpoint
{
	/// <summary>
	/// The report type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	/// The type alias
	/// </summary>
	[DataMember(Name = "typeAlias")]
	public string TypeAlias { get; set; }

	/// <summary>
	/// The group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The format
	/// </summary>
	[DataMember(Name = "format")]
	public string Format { get; set; }

	/// <summary>
	/// The delivery
	/// </summary>
	[DataMember(Name = "delivery")]
	public string Delivery { get; set; }

	/// <summary>
	/// The recipients
	/// </summary>
	[DataMember(Name = "recipients")]
	public List<Recipient> Recipients { get; set; }

	/// <summary>
	/// The schedule
	/// </summary>
	[DataMember(Name = "schedule")]
	public string Schedule { get; set; }

	/// <summary>
	/// The schedule time zone
	/// </summary>
	[DataMember(Name = "scheduleTimezone")]
	public string ScheduleTimezone { get; set; }

	/// <summary>
	/// The id of the user to last modify it
	/// </summary>
	[DataMember(Name = "lastmodifyUserId")]
	public int LastmodifyUserId { get; set; }

	/// <summary>
	/// The username of the user to last modify it
	/// </summary>
	[DataMember(Name = "lastmodifyUserName")]
	public string LastmodifyUserName { get; set; }

	/// <summary>
	/// Whether to enable view as another user
	/// </summary>
	[DataMember(Name = "enableViewAsOtherUser")]
	public bool EnableViewAsOtherUser { get; set; }

	/// <summary>
	/// User permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <summary>
	/// The last generation time
	/// </summary>
	[DataMember(Name = "lastGenerateOn")]
	public int LastGenerateOn { get; set; }

	/// <summary>
	/// The size the last time it was generated
	/// </summary>
	[DataMember(Name = "lastGenerateSize")]
	public int LastGenerateSize { get; set; }

	/// <summary>
	/// The number of pages last time it was generated
	/// </summary>
	[DataMember(Name = "lastGeneratePages")]
	public int LastGeneratePages { get; set; }

	/// <summary>
	/// The custom report type id
	/// </summary>
	[DataMember(Name = "customReportTypeId")]
	public int CustomReportTypeId { get; set; }

	/// <summary>
	/// The custom report type name
	/// </summary>
	[DataMember(Name = "customReportTypeName")]
	public string CustomReportTypeName { get; set; }

	/// <summary>
	/// When the report Link expires
	/// </summary>
	[DataMember(Name = "reportLinkExpire")]
	public string ReportLinkExpire { get; set; }

	/// <summary>
	/// The report link number
	/// </summary>
	[DataMember(Name = "reportLinkNum")]
	public int ReportLinkNum { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "report/reports";
}
