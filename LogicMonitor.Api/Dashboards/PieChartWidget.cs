namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A pie chart widget
/// </summary>
[DataContract]
public class PieChartWidget : Widget, IWidget
{
	/// <summary>
	/// The pie chart info
	/// </summary>
	[DataMember(Name = "pieChartInfo")]
	public PieChartWidgetInfo Info { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
