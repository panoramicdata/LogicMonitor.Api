namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// The data for a custom graph
/// </summary>
[DataContract]
public class CustomGraphWidgetData : WidgetData
{
	/// <summary>
	/// The Id of the widget data
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// The name of the graph
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The vertical label for the graph
	/// </summary>
	[DataMember(Name = "verticalLabel")]
	public string VerticalLabel { get; set; } = string.Empty;

	/// <summary>
	/// Whether the graph is rigid
	/// </summary>
	[DataMember(Name = "rigid")]
	public bool Rigid { get; set; }

	/// <summary>
	/// The width of the graph
	/// </summary>
	[DataMember(Name = "width")]
	public int Width { get; set; }

	/// <summary>
	/// The height of the graph
	/// </summary>
	[DataMember(Name = "height")]
	public int Height { get; set; }

	/// <summary>
	/// The display priority of the graph
	/// </summary>
	[DataMember(Name = "displayPrio")]
	public int DisplayPrio { get; set; }

	/// <summary>
	/// The timescale for the graph
	/// </summary>
	[DataMember(Name = "timeScale")]
	public string TimeScale { get; set; } = string.Empty;

	/// <summary>
	/// Whether this graph uses Base1024?
	/// </summary>
	[DataMember(Name = "base1024")]
	public bool Base1024 { get; set; }

	/// <summary>
	/// The maximum value that should be displayed on the y-axis
	/// </summary>
	[DataMember(Name = "maxValue")]
	public string MaxValue { get; set; } = string.Empty;

	/// <summary>
	/// The minimum value that should be displayed on the y-axis
	/// </summary>
	[DataMember(Name = "minValue")]
	public string MinValue { get; set; } = string.Empty;

	/// <summary>
	/// The start epoch for the graph
	/// </summary>
	[DataMember(Name = "startTime")]
	public long StartTime { get; set; }

	/// <summary>
	/// The end epoch for the graph
	/// </summary>
	[DataMember(Name = "endTime")]
	public long EndTime { get; set; }

	/// <summary>
	/// The timezone within which times are defined
	/// </summary>
	[DataMember(Name = "timeZone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	/// The Id of the timezone in which times are defined
	/// </summary>
	[DataMember(Name = "timeZoneId")]
	public string TimeZoneId { get; set; } = string.Empty;

	/// <summary>
	/// StartTZOffset
	/// </summary>
	[DataMember(Name = "startTZOffset")]
	public int StartTZOffset { get; set; }

	/// <summary>
	/// EndTZOffset
	/// </summary>
	[DataMember(Name = "endTZOffset")]
	public int EndTZOffset { get; set; }

	/// <summary>
	/// The size of the step between data items
	/// </summary>
	[DataMember(Name = "step")]
	public int Step { get; set; }

	/// <summary>
	/// DsName
	/// </summary>
	[DataMember(Name = "dsName")]
	public string DsName { get; set; } = string.Empty;

	/// <summary>
	/// The name of the X-axis
	/// </summary>
	[DataMember(Name = "xAxisName")]
	public string XAxisName { get; set; } = string.Empty;

	/// <summary>
	/// Base
	/// </summary>
	[DataMember(Name = "base")]
	public int Base { get; set; }

	/// <summary>
	/// The default filename to which the data would be exported
	/// </summary>
	[DataMember(Name = "exportFileName")]
	public string ExportFileName { get; set; } = string.Empty;

	/// <summary>
	/// The lines of the graph (the data series)
	/// </summary>
	[DataMember(Name = "lines")]
	public List<CustomGraphWidgetDataLine> Lines { get; set; } = [];

	/// <summary>
	/// Missing lines
	/// </summary>
	[DataMember(Name = "missingLines")]
	public List<CustomGraphWidgetDataLine> MissingLines { get; set; } = [];

	/// <summary>
	/// The timestamps for the data items (the series)
	/// </summary>
	[DataMember(Name = "timestamps")]
	public List<long> Timestamps { get; set; } = [];

	/// <summary>
	/// The scopes
	/// </summary>
	[DataMember(Name = "scopes")]
	public List<CustomGraphWidgetDataScope> Scopes { get; set; } = [];

	/// <summary>
	/// The instances
	/// </summary>
	[DataMember(Name = "instances")]
	public List<int> Instances { get; set; } = [];
}
