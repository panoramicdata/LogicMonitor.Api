namespace LogicMonitor.Api.Dashboards;

/// <summary>
///  Table widget column data point
/// </summary>
public class TableWidgetColumnDataPoint
{
	/// <summary>
	///     The dataSourceId
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	///     Whether isMultiple
	/// </summary>
	[DataMember(Name = "isMultiple")]
	public bool IsMultiple { get; set; }

	/// <summary>
	///     The dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///     The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; }

	/// <summary>
	///     The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }
}
