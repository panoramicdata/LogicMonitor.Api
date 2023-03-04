namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A custom graph widget data point
/// </summary>
[DataContract]
public class CustomGraphWidgetDataPoint : IdentifiedItem
{
	/// <summary>
	/// The entity's name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The consolidate function
	/// </summary>
	[DataMember(Name = "consolidateFunction")]
	public ConsolidateFunction ConsolidateFunction { get; set; }

	/// <summary>
	/// The custom Graph Id
	/// </summary>
	[DataMember(Name = "customGraphId")]
	public int CustomGraphId { get; set; }

	/// <summary>
	/// The dataPoint Id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// The dataPoint Name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }

	/// <summary>
	/// The aggregateFunction
	/// </summary>
	[DataMember(Name = "aggregateFunction")]
	public string AggregateFunction { get; set; } = string.Empty;

	/// <summary>
	/// The dataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; } = string.Empty;

	/// <summary>
	/// The deviceDisplay Name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public StringSpecification DeviceDisplayName { get; set; } = new();

	/// <summary>
	/// The deviceGroupFullPath
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public StringSpecification DeviceGroupFullPath { get; set; } = new();

	/// <summary>
	/// The instanceName
	/// </summary>
	[DataMember(Name = "instanceName")]
	public StringSpecification DataSourceInstanceName { get; set; } = new();

	/// <summary>
	/// The display
	/// </summary>
	[DataMember(Name = "display")]
	public Display Display { get; set; } = new();
}
