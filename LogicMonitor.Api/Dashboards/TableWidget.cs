namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Table widget
/// </summary>
[DataContract]
public class TableWidget : Widget, IWidget
{
	/// <summary>
	///     The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<TableWidgetColumn> Columns { get; set; } = [];

	/// <summary>
	///     The rows
	/// </summary>
	[DataMember(Name = "rows")]
	public List<TableWidgetRow> Rows { get; set; } = [];

	/// <summary>
	///     The forecast
	/// </summary>
	[DataMember(Name = "forecast")]
	public TableWidgetForecast Forecast { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
