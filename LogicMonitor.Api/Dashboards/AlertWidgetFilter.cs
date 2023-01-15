namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     An alert widget filter
/// </summary>
[DataContract]
public class AlertWidgetFilter
{
	/// <summary>
	///     The anomaly
	/// </summary>
	[DataMember(Name = "anomaly")]
	public string Anomaly { get; set; }

	/// <summary>
	/// Displayed alerts must be associated with devices that meet this filter criteria. Glob is accepted, and * and an empty string both indicate all devices
	/// </summary>
	[DataMember(Name = "host")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	///     The device group display name
	/// </summary>
	[DataMember(Name = "group")]
	public string DeviceGroupDisplayName { get; set; }

	/// <summary>
	///     The datasource name
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSourceName { get; set; }

	/// <summary>
	///     The datasource instance name
	/// </summary>
	[DataMember(Name = "instance")]
	public string DataSourceInstanceName { get; set; }

	/// <summary>
	///     The DataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; }

	/// <summary>
	/// The DependencyRole
	/// </summary>
	[DataMember(Name = "dependencyRole")]
	public string DependencyRole { get; set; }

	/// <summary>
	/// The dependency routing state
	/// </summary>
	[DataMember(Name = "dependencyRoutingState")]
	public string DependencyRoutingState { get; set; }

	/// <summary>
	///     The minimum alert severity
	/// </summary>
	[DataMember(Name = "severity")]
	public string AlertLevel { get; set; }

	/// <summary>
	/// Displayed alerts must have an acknowledgement status that satisfies this criteria
	/// </summary>
	[DataMember(Name = "acked")]
	public string Acknowledged { get; set; }

	/// <summary>
	///     The scheduled down time filter
	/// </summary>
	[DataMember(Name = "sdted")]
	public string ScheduleDownTime { get; set; }

	/// <summary>
	/// Displayed alerts must match a rule that satisfies this filter. Glob is accepted, and * and an empty string both match all rules
	/// </summary>
	[DataMember(Name = "rule")]
	public string Rule { get; set; }

	/// <summary>
	///     The scheduled down time filter
	/// </summary>
	[DataMember(Name = "chain")]
	public string EscalationChain { get; set; }

	/// <summary>
	///     The cleared filter
	/// </summary>
	[DataMember(Name = "cleared")]
	public string Cleared { get; set; }

	/// <summary>
	///     The keyword filter
	/// </summary>
	[DataMember(Name = "keyword")]
	public string KeyWord { get; set; }
}
