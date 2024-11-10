namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// An AlertCreationDtoFilter
/// </summary>
[DataContract]
public class AlertCreationDtoFilter
{
	/// <summary>
	/// How to include ACKed alerts
	/// </summary>
	[DataMember(Name = "acked")]
	public string Acked { get; set; } = string.Empty;

	/// <summary>
	/// The Group
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// The device display name
	/// </summary>
	[DataMember(Name = "host")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The data point
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; } = string.Empty;

	/// <summary>
	/// How to include SDTed alerts
	/// </summary>
	[DataMember(Name = "sdted")]
	public string Sdted { get; set; } = string.Empty;

	/// <summary>
	/// The Alert Rule
	/// </summary>
	[DataMember(Name = "rule")]
	public string Rule { get; set; } = string.Empty;

	/// <summary>
	/// The escalation chain
	/// </summary>
	[DataMember(Name = "chain")]
	public string Chain { get; set; } = string.Empty;

	/// <summary>
	/// The cleared filter
	/// </summary>
	[DataMember(Name = "cleared")]
	public string Cleared { get; set; } = string.Empty;

	/// <summary>
	/// The severity
	/// </summary>
	[DataMember(Name = "severity")]
	public string Severity { get; set; } = string.Empty;

	/// <summary>
	/// The datasource
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The instance
	/// </summary>
	[DataMember(Name = "instance")]
	public string Instance { get; set; } = string.Empty;

	/// <summary>
	/// The key word
	/// </summary>
	[DataMember(Name = "keyword")]
	public string KeyWord { get; set; } = string.Empty;
}
