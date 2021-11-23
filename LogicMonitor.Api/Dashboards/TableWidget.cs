namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Table widget
/// </summary>
[DataContract]
public class TableWidget : Widget
{
	/// <summary>
	///     The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<TableWidgetColumn> Columns { get; set; }

	/// <summary>
	///     The rows
	/// </summary>
	[DataMember(Name = "rows")]
	public List<TableWidgetRow> Rows { get; set; }

	/// <summary>
	///     The forecast
	/// </summary>
	[DataMember(Name = "forecast")]
	public TableWidgetForecast Forecast { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }
}
