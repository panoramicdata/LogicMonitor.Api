namespace LogicMonitor.Api.Reports;

/// <summary>
/// A report metric
/// </summary>
public class Metric
{
	/// <summary>
	/// The DataSource id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The DataSource full name
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	/// The Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The DataSource instances
	/// </summary>
	[DataMember(Name = "instances")]
	public string Instances { get; set; } = string.Empty;
}
