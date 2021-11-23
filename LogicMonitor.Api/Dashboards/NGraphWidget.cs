using System.Runtime.Serialization;

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
	public int? DataSourceInstanceId { get; set; }

	/// <summary>
	/// The device Id
	/// </summary>
	[DataMember(Name = "hId")]
	public int? DeviceId { get; set; }

	/// <summary>
	/// The graph Id
	/// </summary>
	[DataMember(Name = "graphId")]
	public int GraphId { get; set; }

	/// <summary>
	/// The graph name
	/// </summary>
	[DataMember(Name = "graphName")]
	public string GraphName { get; set; }

	/// <summary>
	/// The device name
	/// </summary>
	[DataMember(Name = "hostName")]
	public string DeviceName { get; set; }

	/// <summary>
	/// The datasource name
	/// </summary>
	[DataMember(Name = "dsName")]
	public string DataSourceName { get; set; }

	/// <summary>
	/// The datasource instance name
	/// </summary>
	[DataMember(Name = "dsiName")]
	public string DataSourceInstanceName { get; set; }

	/// <summary>
	/// The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }
}
