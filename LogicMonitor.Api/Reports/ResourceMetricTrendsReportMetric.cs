namespace LogicMonitor.Api.Reports;

/// <summary>
/// A HostMetricTrendsReportMetric
/// </summary>
[DataContract]
public class ResourceMetricTrendsReportMetric
{
	/// <summary>
	/// The consolidation function
	/// </summary>
	[DataMember(Name = "consolidationFunc")]
	public string ConsolidationFunction { get; set; } = string.Empty;

	/// <summary>
	/// The dataSourceId
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The dataSourceFullName
	/// </summary>
	[DataMember(Name = "DataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	/// The instances
	/// </summary>
	[DataMember(Name = "Instances")]
	public string Instances { get; set; } = string.Empty;

	/// <summary>
	/// The metric
	/// </summary>
	[DataMember(Name = "Metric")]
	public string Metric { get; set; } = string.Empty;
}
