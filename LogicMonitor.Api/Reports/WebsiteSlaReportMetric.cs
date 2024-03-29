namespace LogicMonitor.Api.Reports;

/// <summary>
/// An Website SLA report metric
/// </summary>
[DataContract]
public class WebsiteSlaReportMetric
{
	/// <summary>
	/// The website group name (or * for all)
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	/// The Website Name (or * for all)
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; } = string.Empty;

	/// <summary>
	/// Whether to exclude alerts occurring in SDT
	/// </summary>
	[DataMember(Name = "excludeSDT")]
	public bool ExcludeSdt { get; set; }
}
