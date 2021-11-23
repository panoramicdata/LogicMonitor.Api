using LogicMonitor.Api.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DataSource Graph
/// </summary>
[DataContract]
public class DataSourceGraph : UndescribedNamedItem
{
	/// <summary>
	/// Aggregated
	/// </summary>
	[DataMember(Name = "aggregated")]
	public bool Aggregated { get; set; }

	/// <summary>
	/// The display name
	/// </summary>
	[DataMember(Name = "base1024")]
	public bool Base1024 { get; set; }

	/// <summary>
	/// DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// Display Priority
	/// </summary>
	[DataMember(Name = "displayPrio")]
	public int DisplayPriority { get; set; }

	/// <summary>
	/// Width
	/// </summary>
	[DataMember(Name = "height")]
	public int Height { get; set; }

	/// <summary>
	/// Width
	/// </summary>
	[DataMember(Name = "lines")]
	public List<Line> Lines { get; set; }

	/// <summary>
	/// MaxValue
	/// </summary>
	[DataMember(Name = "maxValue")]
	public string MaxValue { get; set; }

	/// <summary>
	/// MinValue
	/// </summary>
	[DataMember(Name = "minValue")]
	public string MinValue { get; set; }

	/// <summary>
	/// MinValue
	/// </summary>
	[DataMember(Name = "rigid")]
	public bool Rigid { get; set; }

	/// <summary>
	/// Whether it's base 1024
	/// </summary>
	[DataMember(Name = "timeScale")]
	public string TimeScale { get; set; }

	/// <summary>
	/// Title
	/// </summary>
	[DataMember(Name = "title")]
	public string Title { get; set; }

	/// <summary>
	/// Vertical Label
	/// </summary>
	[DataMember(Name = "verticalLabel")]
	public string VerticalLabel { get; set; }

	/// <summary>
	/// Width
	/// </summary>
	[DataMember(Name = "width")]
	public int Width { get; set; }

	/// <summary>
	/// The data points
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<DataSourceDataPoint> DataPoints { get; set; }

	/// <summary>
	/// The virtual data points
	/// </summary>
	[DataMember(Name = "virtualDataPoints")]
	public List<VirtualDataPoint> VirtualDataPoints { get; set; }

	/// <summary>
	/// ToString override
	/// </summary>
	public override string ToString() => $"{Name} ({Id})";
}
