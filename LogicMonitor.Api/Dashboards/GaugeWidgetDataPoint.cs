namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A gauge widget data point
/// </summary>
[DataContract]
public class GaugeWidgetDataPoint
{
	/// <summary>
	///     The deviceGroupFullPath
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string DeviceGroupFullPath { get; set; }

	/// <summary>
	///     The deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	///     The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; }

	/// <summary>
	///     The dataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	///     The dataSource instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string DataSourceInstanceName { get; set; }

	/// <summary>
	///     The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }

	/// <summary>
	///     The dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///     The aggregateFunction
	/// </summary>
	[DataMember(Name = "aggregateFunction")]
	public string AggregateFunction { get; set; }

	/// <summary>
	///     The data series
	/// </summary>
	[DataMember(Name = "dataSeries")]
	public string DataSeries { get; set; }

	/// <summary>
	///     The RPN
	/// </summary>
	[DataMember(Name = "rpn")]
	public string Rpn { get; set; }
}
