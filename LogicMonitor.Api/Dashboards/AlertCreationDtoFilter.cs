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
	public string Acked { get; set; }

	/// <summary>
	/// The device group
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; }

	/// <summary>
	/// The device display name
	/// </summary>
	[DataMember(Name = "host")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The data point
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; }

	/// <summary>
	/// How to include SDTed alerts
	/// </summary>
	[DataMember(Name = "sdted")]
	public string Sdted { get; set; }

	/// <summary>
	/// The Alert Rule
	/// </summary>
	[DataMember(Name = "rule")]
	public string Rule { get; set; }

	/// <summary>
	/// The escalation chain
	/// </summary>
	[DataMember(Name = "chain")]
	public string Chain { get; set; }

	/// <summary>
	/// The cleared filter
	/// </summary>
	[DataMember(Name = "cleared")]
	public string Cleared { get; set; }

	/// <summary>
	/// The severity
	/// </summary>
	[DataMember(Name = "severity")]
	public string Severity { get; set; }

	/// <summary>
	/// The datasource
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSourceName { get; set; }

	/// <summary>
	/// The instance
	/// </summary>
	[DataMember(Name = "instance")]
	public string Instance { get; set; }

	/// <summary>
	/// The key word
	/// </summary>
	[DataMember(Name = "keyword")]
	public string KeyWord { get; set; }
}
