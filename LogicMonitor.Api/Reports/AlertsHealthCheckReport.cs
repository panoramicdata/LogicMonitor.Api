namespace LogicMonitor.Api.Reports;

/// <summary>
/// An Alerts Health Check report, which has an alert report's filters under different field names
/// </summary>
[DataContract]
public class AlertsHealthCheckReport : DateRangeReport
{
	/// <summary>
	/// The resource group full path
	/// </summary>
	[DataMember(Name = "hostGroup")]
	public string GroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// The resource display name
	/// </summary>
	[DataMember(Name = "host")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The datasource
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSource { get; set; } = string.Empty;

	/// <summary>
	/// The datasource instance name
	/// </summary>
	[DataMember(Name = "instance")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The datapoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; } = string.Empty;

	/// <summary>
	/// The alert level
	/// </summary>
	[DataMember(Name = "severity")]
	public string Level { get; set; } = string.Empty;

	/// <summary>
	/// The acknowledgement filter value
	/// </summary>
	[DataMember(Name = "ackFilter")]
	public string AckFilter { get; set; } = string.Empty;

	/// <summary>
	/// all|yes|no: which alerts to include by whether they have an anomaly
	/// </summary>
	[DataMember(Name = "anomaly")]
	public string Anomaly { get; set; } = string.Empty;

	/// <summary>
	/// The SDT filter value
	/// </summary>
	[DataMember(Name = "sdtFilter")]
	public string SdtFilter { get; set; } = string.Empty;

	/// <summary>
	/// Whether to show active only
	/// </summary>
	[DataMember(Name = "activeOnly")]
	public bool ActiveOnly { get; set; }

	/// <summary>
	/// The clear filter
	/// </summary>
	[DataMember(Name = "clearFilter")]
	public string ClearFilter { get; set; } = string.Empty;

	/// <summary>
	/// The rule
	/// </summary>
	[DataMember(Name = "rule")]
	public string Rule { get; set; } = string.Empty;

	/// <summary>
	/// The chain
	/// </summary>
	[DataMember(Name = "chain")]
	public string Chain { get; set; } = string.Empty;

	/// <summary>
	/// Whether to include escalated alerts. all|yes|no
	/// </summary>
	[DataMember(Name = "isEscalation")]
	public string IsEscalation { get; set; } = string.Empty;

	/// <summary>
	/// Whether to include alerts from deleted resources or websites. yes|no
	/// </summary>
	[DataMember(Name = "isIncludeDeletedResourceOrWebsite")]
	public string IsIncludeDeletedResourceOrWebsite { get; set; } = string.Empty;

	/// <summary>
	/// The monitored object groups
	/// </summary>
	[DataMember(Name = "monitoredObjectGroups")]
	public object? MonitoredObjectGroups { get; set; }

	/// <summary>
	/// The dependency role
	/// </summary>
	[DataMember(Name = "dependencyRole")]
	public string DependencyRole { get; set; } = string.Empty;

	/// <summary>
	/// The dependency routing state
	/// </summary>
	[DataMember(Name = "dependencyRoutingState")]
	public string DependencyRoutingState { get; set; } = string.Empty;

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = [];
}
