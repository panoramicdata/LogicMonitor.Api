namespace LogicMonitor.Api.Reports;

/// <summary>
/// NetflowReport
/// </summary>
[DataContract]
public class NetflowReport : ReportBase
{
	/// <summary>
	/// The resource type for the report, host or group
	/// </summary>
	[DataMember(Name = "hostsValType", IsRequired = true)]
	public string HostsValType { get; set; } = null!;

	/// <summary>
	/// The Time Range configured for the report: Last 2 hours | Last 24 hours | Last calendar day | Last 7 days | Last 14 days | Last 30 days | Last calendar month | Last 365 days | Any custom date range in this format: YYYY-MM-dd hh:mm TO YYYY-MM-dd hh:mm
	/// </summary>
	[DataMember(Name = "dateRange", IsRequired = false)]
	public string DateRange { get; set; }

	/// <summary>
	/// The devices OR groups (full path) selected for the report, where multiple entities are separated by commas. Glob is accepted
	/// </summary>
	[DataMember(Name = "hostsVal", IsRequired = true)]
	public string HostsVal { get; set; } = null!;

	/// <summary>
	/// Whether include DNS mappings or not
	/// </summary>
	[DataMember(Name = "includeDNSMappings", IsRequired = false)]
	public bool IncludeDNSMappings { get; set; }
}
