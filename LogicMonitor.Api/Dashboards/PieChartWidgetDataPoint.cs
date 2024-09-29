namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Pie chart widget data point
/// </summary>
[DataContract]
public class PieChartWidgetDataPoint
{
	/// <summary>
	///     The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupFullPath instead", true)]
	public string DeviceGroupFullPath => ResourceGroupFullPath;

	/// <summary>
	/// The Resource Display Name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDisplayName instead", true)]
	public string DeviceDisplayName => ResourceDisplayName;

	/// <summary>
	/// The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	/// The dataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The instanceName
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The dataPoint Name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// The dataPoint Id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// The entity's name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Whether Top 10 is enabled
	/// </summary>
	[DataMember(Name = "top10")]
	public bool Top10 { get; set; }

	/// <summary>
	/// Whether to aggregate
	/// </summary>
	[DataMember(Name = "aggregate")]
	public bool Aggregate { get; set; }

	/// <summary>
	/// The aggregateFunction
	/// </summary>
	[DataMember(Name = "aggregateFunction")]
	public string AggregateFunction { get; set; } = string.Empty;
}
