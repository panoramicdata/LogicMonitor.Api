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
	public string DataSourceFullName { get; set; }

	/// <summary>
	/// The DataSource instances
	/// </summary>
	[DataMember(Name = "instances")]
	public string Instances { get; set; }

	/// <summary>
	/// The DataPoint name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }


	/// <summary>
	/// The DataPoint ID
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }
}
