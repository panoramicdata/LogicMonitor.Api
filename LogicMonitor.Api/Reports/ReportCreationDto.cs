namespace LogicMonitor.Api.Reports;

/// <summary>
///    A ReportGroup creation DTO. This is incomplete i.e. does not yet include properties to set a schedule or recipients
/// </summary>
[DataContract]
public class ReportCreationDto : CreationDto<ReportBase>
{
	/// <summary>
	///    The type, e.g Dashboard
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///    The Group ID
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///    The format - HTML or PDF
	/// </summary>
	[DataMember(Name = "format")]
	public string Format { get; set; } = "HTML";

	/// <summary>
	///    The expiry type - High Flexibiilty or High Security
	/// </summary>
	[DataMember(Name = "reportLinkExpire")]
	public string ReportLinkExpire { get; set; } = "High Flexibility";

	/// <summary>
	///    The date range
	/// </summary>
	[DataMember(Name = "dateRange")]
	public string DateRange { get; set; } = "Default";

	/// <summary>
	///    The Dashboard ID
	/// </summary>
	[DataMember(Name = "dashboardId")]
	public int DashboardId { get; set; }

	/// <summary>
	///    The display name used in the generated report (when run)
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	///    Whether to display the generated report's URL on the output
	/// </summary>
	[DataMember(Name = "displayLink")]
	public bool DisplayLink { get; set; }

	/// <summary>
	///    Whether to display the generated report's URL on the output
	/// </summary>
	[DataMember(Name = "delivery")]
	public string Delivery { get; set; } = "none";

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>Name</returns>
	public override string ToString() => Name;
}
