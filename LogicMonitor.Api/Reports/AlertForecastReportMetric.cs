namespace LogicMonitor.Api.Reports;

/// <summary>
/// An alert forecast report metric
/// </summary>
[DataContract]
public class AlertForecastReportMetric
{
	/// <summary>
	/// The dataSourceId
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	/// The instances
	/// </summary>
	[DataMember(Name = "instances")]
	public string Instances { get; set; } = string.Empty;

	/// <summary>
	/// The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// The dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }
}
