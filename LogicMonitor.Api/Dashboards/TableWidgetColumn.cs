namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Table widget column
/// </summary>
[DataContract]
public class TableWidgetColumn
{
	/// <summary>
	///     The column name
	/// </summary>
	[DataMember(Name = "columnName")]
	public string ColumnName { get; set; } = string.Empty;

	/// <summary>
	///     The dataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public TableWidgetColumnDataPoint DataPoint { get; set; } = new();

	/// <summary>
	///     The alternate DataPoints
	/// </summary>
	[DataMember(Name = "alternateDataPoints")]
	public object AlternateDataPoints { get; set; } = new();

	/// <summary>
	///     The enableForecast
	/// </summary>
	[DataMember(Name = "enableForecast")]
	public bool IsForecastEnabled { get; set; }

	/// <summary>
	///     The roundingDecimal
	/// </summary>
	[DataMember(Name = "roundingDecimal")]
	public int RoundingDecimal { get; set; }

	/// <summary>
	///     The reverse polish notation
	/// </summary>
	[DataMember(Name = "rpn")]
	public string Rpn { get; set; } = string.Empty;

	/// <summary>
	/// The unit label
	/// </summary>
	[DataMember(Name = "unitLabel")]
	public string UnitLabel { get; set; } = string.Empty;
}
