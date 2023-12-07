namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DataSource Graph
/// </summary>
[DataContract]
public class DataSourceOverviewGraph : UndescribedNamedItem
{
	/// <summary>
	/// is the overview graph aggregated
	/// </summary>
	[DataMember(Name = "aggregated")]
	public bool Aggregated { get; set; }

	/// <summary>
	/// base1024 graph or not
	/// </summary>
	[DataMember(Name = "base1024")]
	public bool Base1024 { get; set; }

	/// <summary>
	/// DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// the graph display priority
	/// </summary>
	[DataMember(Name = "displayPrio")]
	public int DisplayPriority { get; set; }

	/// <summary>
	/// the graph height
	/// </summary>
	[DataMember(Name = "height")]
	public int Height { get; set; }

	/// <summary>
	/// the graph lines
	/// </summary>
	[DataMember(Name = "lines")]
	public List<GraphLine> Lines { get; set; } = [];

	/// <summary>
	/// graph max value
	/// </summary>
	[DataMember(Name = "maxValue")]
	public string MaxValue { get; set; } = string.Empty;

	/// <summary>
	/// graph min value
	/// </summary>
	[DataMember(Name = "minValue")]
	public string MinValue { get; set; } = string.Empty;

	/// <summary>
	/// the rigid, true|false
	/// </summary>
	[DataMember(Name = "rigid")]
	public bool Rigid { get; set; }

	/// <summary>
	/// the graph time scale, 1hour|2hour|5hour|day|yesterday|week|lastweek|month|3month|year
	/// </summary>
	[DataMember(Name = "timeScale")]
	public string TimeScale { get; set; } = string.Empty;

	/// <summary>
	/// the graph title
	/// </summary>
	[DataMember(Name = "title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// the graph vertical label
	/// </summary>
	[DataMember(Name = "verticalLabel")]
	public string VerticalLabel { get; set; } = string.Empty;

	/// <summary>
	/// the graph width
	/// </summary>
	[DataMember(Name = "width")]
	public int Width { get; set; }

	/// <summary>
	/// the graph data point list
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<OverviewGraphDataPoint> DataPoints { get; set; } = [];

	/// <summary>
	/// the virtual data point list
	/// </summary>
	[DataMember(Name = "virtualDataPoints")]
	public List<GraphVirtualDataPoint> VirtualDataPoints { get; set; } = [];

	/// <summary>
	/// ToString override
	/// </summary>
	public override string ToString() => $"{Name} ({Id})";
}
