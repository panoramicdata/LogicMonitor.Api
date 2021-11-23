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
	public string ColumnName { get; set; }

	/// <summary>
	///     The dataPoint
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public TableWidgetColumnDataPoint DataPoint { get; set; }

	/// <summary>
	///     The alternate DataPoints
	/// </summary>
	[DataMember(Name = "alternateDataPoints")]
	public object AlternateDataPoints { get; set; }

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
	///     The RPN
	/// </summary>
	[DataMember(Name = "rpn")]
	public string Rpn { get; set; }
}
