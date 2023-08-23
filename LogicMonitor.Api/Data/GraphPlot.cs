namespace LogicMonitor.Api.Data;

/// <summary>
/// Graph plot
/// </summary>

[DataContract]
public class GraphPlot : WidgetData
{
	/// <summary>
	/// The Missing lines of the graph
	/// </summary>
	[DataMember(Name = "missinglines")]
	public List<string> Missinglines { get; set; } = new();

	/// <summary>
	/// The specified timescale for the graph
	/// </summary>
	[DataMember(Name = "timeScale")]
	public string TimeScale { get; set; } = string.Empty;

	/// <summary>
	/// The matched instances of graph
	/// </summary>
	[DataMember(Name = "instances")]
	public List<int> Instances { get; set; } = new();

	/// <summary>
	/// The timestamps of the graph
	/// </summary>
	[DataMember(Name = "timestamps")]
	public List<int> Timestamps { get; set; } = new();

	/// <summary>
	/// Specifies the minimum value of the graph
	/// </summary>
	[DataMember(Name = "minValue")]
	public object MinValue { get; set; } = new();

	/// <summary>
	/// Specifies the start-time of the graph
	/// </summary>
	[DataMember(Name = "startTime")]
	public int StartTime { get; set; }

	/// <summary>
	/// The Id of the graph
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// true | false\nSpecifies if the graph is rigid or not
	/// </summary>
	[DataMember(Name = "rigid")]
	public bool Rigid { get; set; }

	/// <summary>
	/// The properties of the graph and graph lines
	/// </summary>
	[DataMember(Name = "lines")]
	public List<GraphPlotLine> Lines { get; set; } = new();

	/// <summary>
	/// Specifies the height of graph
	/// </summary>
	[DataMember(Name = "height")]
	public int Height { get; set; }

	/// <summary>
	/// Specifies the end TimeZone Offset of the graph
	/// </summary>
	[DataMember(Name = "endTZOffset")]
	public int EndTZOffset { get; set; }

	/// <summary>
	/// true | false\nChanges base scale from 1000 to 1024 if value is set to true
	/// </summary>
	[DataMember(Name = "base1024")]
	public bool Base1024 { get; set; }

	/// <summary>
	/// The name of the DataSource to be used to plot the graph
	/// </summary>
	[DataMember(Name = "dsName")]
	public string DsName { get; set; } = string.Empty;

	/// <summary>
	/// Specifies the maximum value of the graph
	/// </summary>
	[DataMember(Name = "maxValue")]
	public object MaxValue { get; set; } = new();

	/// <summary>
	/// The display priority of the graph in your LogicMonitor portal
	/// </summary>
	[DataMember(Name = "displayPrio")]
	public int DisplayPrio { get; set; }

	/// <summary>
	/// The Id of selected Time Zone
	/// </summary>
	[DataMember(Name = "timeZoneId")]
	public string TimeZoneId { get; set; } = string.Empty;

	/// <summary>
	/// The selected timezone for the graph
	/// </summary>
	[DataMember(Name = "timeZone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	/// Specifies the start TimeZone Offset of the graph
	/// </summary>
	[DataMember(Name = "startTZOffset")]
	public int StartTZOffset { get; set; }

	/// <summary>
	/// The label that will be displayed along the X axis
	/// </summary>
	[DataMember(Name = "xAxisName")]
	public string XAxisName { get; set; } = string.Empty;

	/// <summary>
	/// Specifies the width of graph
	/// </summary>
	[DataMember(Name = "width")]
	public int Width { get; set; }

	/// <summary>
	/// The Name of the Graph
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The label that will be displayed along the y axis (Vertical Label)
	/// </summary>
	[DataMember(Name = "verticalLabel")]
	public string VerticalLabel { get; set; } = string.Empty;

	/// <summary>
	/// The Step of the graph
	/// </summary>
	[DataMember(Name = "step")]
	public int Step { get; set; }

	/// <summary>
	/// Specifies the end-time of the graph
	/// </summary>
	[DataMember(Name = "endTime")]
	public int EndTime { get; set; }

	/// <summary>
	/// Scopes: use this field to find match opsnote
	/// </summary>
	[DataMember(Name = "scopes")]
	public List<GraphOpsNoteScope> Scopes { get; set; } = new();

	/// <summary>
	/// The Base of the graph
	/// </summary>
	[DataMember(Name = "base")]
	public int Base { get; set; }

	/// <summary>
	/// The export file name
	/// </summary>
	[DataMember(Name = "exportFileName")]
	public string ExportFileName { get; set; } = string.Empty;
}
