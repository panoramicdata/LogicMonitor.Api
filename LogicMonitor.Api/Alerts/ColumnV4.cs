namespace LogicMonitor.Api.Alerts;

/// <summary>
/// Column v4
/// </summary>
[DataContract]
public class ColumnV4
{
	/// <summary>
	/// The maximum value
	/// </summary>
	[DataMember(Name = "maxValue")]
	public int MaxValue { get; set; }

	/// <summary>
	/// The index
	/// </summary>
	[DataMember(Name = "index")]
	public int Index { get; set; }

	/// <summary>
	/// The properties options
	/// </summary>
	[DataMember(Name = "propertiesOptions")]
	public string PropertiesOptions { get; set; } = string.Empty;

	/// <summary>
	/// Whether to enable forecast
	/// </summary>
	[DataMember(Name = "enableForecast")]
	public bool EnableForecast { get; set; }

	/// <summary>
	/// The rounding decimal
	/// </summary>
	[DataMember(Name = "roundingDecimal")]
	public int RoundingDecimal { get; set; }

	/// <summary>
	/// The RPN
	/// </summary>
	[DataMember(Name = "rpn")]
	public string Rpn { get; set; } = string.Empty;

	/// <summary>
	/// The data point name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// The minimum value
	/// </summary>
	[DataMember(Name = "minValue")]
	public int MinValue { get; set; }

	/// <summary>
	/// The display type
	/// </summary>
	[DataMember(Name = "displayType")]
	public string DisplayType { get; set; } = string.Empty;

	/// <summary>
	/// The unit label
	/// </summary>
	[DataMember(Name = "unitLabel")]
	public string UnitLabel { get; set; } = string.Empty;

	/// <summary>
	/// The ID
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The position
	/// </summary>
	[DataMember(Name = "position")]
	public int Position { get; set; }

	/// <summary>
	/// The color thresholds
	/// </summary>
	[DataMember(Name = "colorThresholds")]
	public List<ColorThreshold> ColorThresholds { get; set; } = [];

	/// <summary>
	/// The column name
	/// </summary>
	[DataMember(Name = "columnName")]
	public string ColumnName { get; set; } = string.Empty;
}