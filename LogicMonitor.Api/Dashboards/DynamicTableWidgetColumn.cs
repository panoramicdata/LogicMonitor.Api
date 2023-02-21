namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A dynamic table widget column
/// </summary>
public class DynamicTableWidgetColumn
{
	/// <summary>
	///     The column name
	/// </summary>
	[DataMember(Name = "columnName")]
	public string ColumnName { get; set; }

	/// <summary>
	///     The DataPoint id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///     The DataPoint name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }

	/// <summary>
	///     Whether to enable forecast
	/// </summary>
	[DataMember(Name = "enableForecast")]
	public bool EnableForecast { get; set; }

	/// <summary>
	///     The minimum value
	/// </summary>
	[DataMember(Name = "minValue")]
	public double? MinValue { get; set; }

	/// <summary>
	///     The maximum value
	/// </summary>
	[DataMember(Name = "maxValue")]
	public double? MaxValue { get; set; }

	/// <summary>
	///     The rounding decimal
	/// </summary>
	[DataMember(Name = "roundingDecimal")]
	public int RoundingDecimal { get; set; }

	/// <summary>
	///     The RPN
	/// </summary>
	[DataMember(Name = "rpn")]
	public string Rpn { get; set; }

	/// <summary>
	///     The display type
	/// </summary>
	[DataMember(Name = "displayType")]
	public DisplayType DisplayType { get; set; }

	/// <summary>
	///     The color thresholds
	/// </summary>
	[DataMember(Name = "colorThresholds")]
	public List<ColorThreshold> ColorThresholds { get; set; }

	/// <summary>
	///     The unit label
	/// </summary>
	[DataMember(Name = "unitLabel")]
	public string UnitLabel { get; set; }
}
