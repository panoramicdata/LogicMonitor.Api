namespace LogicMonitor.Api.Data;

/// <summary>
/// Graph plot line
/// </summary>

[DataContract]
public class GraphPlotLine
{
	/// <summary>
	/// The color name
	/// </summary>
	[DataMember(Name = "colorName")]
	public string ColorName { get; set; } = string.Empty;

	/// <summary>
	/// The standard deviation value
	/// </summary>
	[DataMember(Name = "std")]
	public object Std { get; set; } = new();

	/// <summary>
	/// true | false\nSpecifies whether the graph will be visible or not
	/// </summary>
	[DataMember(Name = "visible")]
	public bool Visible { get; set; }

	/// <summary>
	/// The color of the graph
	/// </summary>
	[DataMember(Name = "color")]
	public string Color { get; set; } = string.Empty;

	/// <summary>
	/// The polled data used to plot the graph
	/// </summary>
	[DataMember(Name = "data")]
	public List<object> Data { get; set; } = new();

	/// <summary>
	/// The max value of datapoint or instance
	/// </summary>
	[DataMember(Name = "max")]
	public object Max { get; set; } = new();

	/// <summary>
	/// The legend of the datapoint or instance
	/// </summary>
	[DataMember(Name = "legend")]
	public string Legend { get; set; } = string.Empty;

	/// <summary>
	/// The description for the datapoint or instance
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The label for the datapoint or instance
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	/// line | area | stack | column | statusBar\nSpecifies how the data of the datapoint or instance will be plotted
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The min value of the datapoint or instance
	/// </summary>
	[DataMember(Name = "min")]
	public object Min { get; set; } = new();

	/// <summary>
	/// The average value of the datapoint or instance
	/// </summary>
	[DataMember(Name = "avg")]
	public object Avg { get; set; } = new();

	/// <summary>
	/// -1 | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8\nThe decimal value
	/// </summary>
	[DataMember(Name = "decimal")]
	public int LineDecimal { get; set; }

	/// <summary>
	/// true | false\nSpecifies whether to use YMax or not
	/// </summary>
	[DataMember(Name = "useYMax")]
	public bool UseYMax { get; set; }
}
