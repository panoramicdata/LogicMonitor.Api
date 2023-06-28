namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Pie chart widget info
/// </summary>
[DataContract]
public class PieChartWidgetInfo
{
	/// <summary>
	/// The title
	/// </summary>
	[DataMember(Name = "counters")]
	public List<object> Counters { get; set; } = new();

	/// <summary>
	/// Whether to hide zero percent slices
	/// </summary>
	[DataMember(Name = "hideZeroPercentSlices")]
	public bool HideZeroPercentSlices { get; set; }

	/// <summary>
	/// The title
	/// </summary>
	[DataMember(Name = "title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Whether to show labels and lines
	/// </summary>
	[DataMember(Name = "showLabelsAndLinesOnPC")]
	public bool ShowLabelsAndLines { get; set; }

	/// <summary>
	/// The title
	/// </summary>
	[DataMember(Name = "maxSlicesCanBeShown")]
	public int MaxVisibleSliceCount { get; set; }

	/// <summary>
	/// Whether to group remaining as "others"
	/// </summary>
	[DataMember(Name = "groupRemainingAsOthers")]
	public bool GroupRemainingAsOthers { get; set; }

	/// <summary>
	/// The data points
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<PieChartWidgetDataPoint> DataPoints { get; set; } = new();

	/// <summary>
	/// The title
	/// </summary>
	[DataMember(Name = "virtualDataPoints")]
	public List<object> VirtualDataPoints { get; set; } = new();

	/// <summary>
	/// The title
	/// </summary>
	[DataMember(Name = "pieChartItems")]
	public List<PieChartWidgetInfoItem> Items { get; set; } = new();
}
