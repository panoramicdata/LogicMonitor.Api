namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A normal graph widget
/// </summary>
[DataContract]
public class NGraphWidget : GraphWidget
{
	/// <summary>
	/// The datasource instance Id
	/// </summary>
	[DataMember(Name = "dsiId")]
	public int DataSourceInstanceId { get; set; }

	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "hId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The graph Id
	/// </summary>
	[DataMember(Name = "graphId")]
	public int GraphId { get; set; }

	/// <summary>
	/// The graph name
	/// </summary>
	[DataMember(Name = "graphName")]
	public string GraphName { get; set; } = string.Empty;

	/// <summary>
	/// The device name
	/// </summary>
	[DataMember(Name = "hostName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The datasource name
	/// </summary>
	[DataMember(Name = "dsName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The datasource instance name
	/// </summary>
	[DataMember(Name = "dsiName")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
