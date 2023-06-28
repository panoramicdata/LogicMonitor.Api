namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A set of parsed alert filters
/// </summary>
public class ParsedAlertFilters
{
	/// <summary>
	///     The acked
	/// </summary>
	[DataMember(Name = "acked")]
	public string Acked { get; set; } = string.Empty;

	/// <summary>
	///     The anomaly
	/// </summary>
	[DataMember(Name = "anomaly")]
	public string Anomaly { get; set; } = string.Empty;

	/// <summary>
	///     The chain
	/// </summary>
	[DataMember(Name = "chain")]
	public string Chain { get; set; } = string.Empty;

	/// <summary>
	///     The cleared
	/// </summary>
	[DataMember(Name = "cleared")]
	public string Cleared { get; set; } = string.Empty;

	/// <summary>
	///     The dataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; } = string.Empty;

	/// <summary>
	///     The dataSource
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSource { get; set; } = string.Empty;

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
	///     The group
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	///     The host
	/// </summary>
	[DataMember(Name = "host")]
	public string Host { get; set; } = string.Empty;

	/// <summary>
	///     The instance
	/// </summary>
	[DataMember(Name = "instance")]
	public string Instance { get; set; } = string.Empty;

	/// <summary>
	///     The keyword
	/// </summary>
	[DataMember(Name = "keyword")]
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	///     The rule
	/// </summary>
	[DataMember(Name = "rule")]
	public string Rule { get; set; } = string.Empty;

	/// <summary>
	///     The sdted
	/// </summary>
	[DataMember(Name = "sdted")]
	public string Sdted { get; set; } = string.Empty;

	/// <summary>
	///     The severity
	/// </summary>
	[DataMember(Name = "severity")]
	public string Severity { get; set; } = string.Empty;
}
