namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// An overview graph widget
/// </summary>
[DataContract]
public class OverviewGraphWidget : GraphWidget
{
	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "hId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The datasource id
	/// </summary>
	[DataMember(Name = "dsId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The datasource name
	/// </summary>
	[DataMember(Name = "dsName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The datasource instance group id
	/// </summary>
	[DataMember(Name = "dsigId")]
	public int DataSourceInstanceGroupId { get; set; }

	/// <summary>
	/// The datasource instance group name
	/// </summary>
	[DataMember(Name = "dsigName")]
	public string DataSourceInstanceGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource name
	/// </summary>
	[DataMember(Name = "hostName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The graph id
	/// </summary>
	[DataMember(Name = "graphId")]
	public int GraphId { get; set; }

	/// <summary>
	/// The graph name
	/// </summary>
	[DataMember(Name = "graphName")]
	public string GraphName { get; set; } = string.Empty;

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
