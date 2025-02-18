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
	public string Anomaly { get; set; } = string.Empty;

	/// <summary>
	/// Displayed alerts must be associated with Resources that meet this filter criteria. Glob is accepted, and * and an empty string both indicate all Resources
	/// </summary>
	[DataMember(Name = "host")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///     The group display name
	/// </summary>
	[DataMember(Name = "group")]
	public string ResourceGroupDisplayName { get; set; } = string.Empty;

	/// <summary>
	///     The datasource name
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	///     The datasource instance name
	/// </summary>
	[DataMember(Name = "instance")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	///     The DataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; } = string.Empty;

	/// <summary>
	/// The DependencyRole
	/// </summary>
	[DataMember(Name = "dependencyRole")]
	public string DependencyRole { get; set; } = string.Empty;

	/// <summary>
	/// The dependency routing state
	/// </summary>
	[DataMember(Name = "dependencyRoutingState")]
	public string DependencyRoutingState { get; set; } = string.Empty;

	/// <summary>
	///     The minimum alert severity
	/// </summary>
	[DataMember(Name = "severity")]
	public string AlertLevel { get; set; } = string.Empty;

	/// <summary>
	/// Displayed alerts must have an acknowledgement status that satisfies this criteria
	/// </summary>
	[DataMember(Name = "acked")]
	public string Acknowledged { get; set; } = string.Empty;

	/// <summary>
	///     The scheduled down time filter
	/// </summary>
	[DataMember(Name = "sdted")]
	public string ScheduleDownTime { get; set; } = string.Empty;

	/// <summary>
	/// Displayed alerts must match a rule that satisfies this filter. Glob is accepted, and * and an empty string both match all rules
	/// </summary>
	[DataMember(Name = "rule")]
	public string Rule { get; set; } = string.Empty;

	/// <summary>
	///     The scheduled down time filter
	/// </summary>
	[DataMember(Name = "chain")]
	public string EscalationChain { get; set; } = string.Empty;

	/// <summary>
	///     The cleared filter
	/// </summary>
	[DataMember(Name = "cleared")]
	public string Cleared { get; set; } = string.Empty;

	/// <summary>
	///     The keyword filter
	/// </summary>
	[DataMember(Name = "keyword")]
	public string KeyWord { get; set; } = string.Empty;
}
