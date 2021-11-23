namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A set of parsed alert filters
/// </summary>
public class ParsedAlertFilters
{
	/// <summary>
	///     The group
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; }

	/// <summary>
	///     The host
	/// </summary>
	[DataMember(Name = "host")]
	public string Host { get; set; }

	/// <summary>
	///     The dataSource
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSource { get; set; }

	/// <summary>
	///     The instance
	/// </summary>
	[DataMember(Name = "instance")]
	public string Instance { get; set; }

	/// <summary>
	///     The dataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public string DataPoint { get; set; }

	/// <summary>
	///     The severity
	/// </summary>
	[DataMember(Name = "severity")]
	public string Severity { get; set; }

	/// <summary>
	///     The acked
	/// </summary>
	[DataMember(Name = "acked")]
	public string Acked { get; set; }

	/// <summary>
	///     The sdted
	/// </summary>
	[DataMember(Name = "sdted")]
	public string Sdted { get; set; }

	/// <summary>
	///     The rule
	/// </summary>
	[DataMember(Name = "rule")]
	public string Rule { get; set; }

	/// <summary>
	///     The chain
	/// </summary>
	[DataMember(Name = "chain")]
	public string Chain { get; set; }

	/// <summary>
	///     The cleared
	/// </summary>
	[DataMember(Name = "cleared")]
	public string Cleared { get; set; }

	/// <summary>
	///     The keyword
	/// </summary>
	[DataMember(Name = "keyword")]
	public string Keyword { get; set; }
}
