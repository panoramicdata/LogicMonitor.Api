namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A gauge widget data point
/// </summary>
[DataContract]
public class GaugeWidgetDataPoint
{
	/// <summary>
	///     The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///     The deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///     The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	///     The dataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	///     The dataSource instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	///     The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	///     The dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///     The aggregateFunction
	/// </summary>
	[DataMember(Name = "aggregateFunction")]
	public string AggregateFunction { get; set; } = string.Empty;

	/// <summary>
	///     The data series
	/// </summary>
	[DataMember(Name = "dataSeries")]
	public string DataSeries { get; set; } = string.Empty;

	/// <summary>
	///     The RPN
	/// </summary>
	[DataMember(Name = "rpn")]
	public string Rpn { get; set; } = string.Empty;
}
