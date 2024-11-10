namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A big number data point
/// </summary>
[DataContract]
public class BigNumberDataPoint
{
	/// <summary>
	///     The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///     The Resource DisplayName
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
	///     The instanceName
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; } = string.Empty;

	/// <summary>
	///     The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }

	/// <summary>
	///     The dataSource Id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///     The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///     The aggregateFunction
	/// </summary>
	[DataMember(Name = "aggregateFunction")]
	public string AggregateFunction { get; set; } = string.Empty;

	/// <summary>
	///     The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///     The globMode
	/// </summary>
	[DataMember(Name = "globMode")]
	public bool GlobMode { get; set; }
}
