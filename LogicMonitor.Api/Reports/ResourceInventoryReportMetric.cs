namespace LogicMonitor.Api.Reports;

/// <summary>
/// A metric on a resource inventory report: a DataSource and the instances it covers
/// </summary>
[DataContract]
public class ResourceInventoryReportMetric
{
	/// <summary>
	/// The DataSource id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The DataSource's full name, for example Network Interfaces
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	/// The instances, for example * for all of them
	/// </summary>
	[DataMember(Name = "instances")]
	public string Instances { get; set; } = string.Empty;
}
