namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Data line that forms part of a custom graph
/// </summary>
[DataContract]
public class CustomGraphWidgetDataLine
{
	/// <summary>
	/// Name of the color for this line
	/// </summary>
	[DataMember(Name = "colorName")]
	public string ColorName { get; set; } = string.Empty;

	/// <summary>
	/// Standard deviation of the data values
	/// </summary>
	[DataMember(Name = "std")]
	public double Std { get; set; }

	/// <summary>
	/// Minimum data value
	/// </summary>
	[DataMember(Name = "min")]
	public double Min { get; set; }

	/// <summary>
	/// Maximum data value
	/// </summary>
	[DataMember(Name = "max")]
	public double Max { get; set; }

	/// <summary>
	/// Average data value
	/// </summary>
	[DataMember(Name = "avg")]
	public double Avg { get; set; }

	/// <summary>
	/// Whether this line is visible
	/// </summary>
	[DataMember(Name = "visible")]
	public bool Visible { get; set; }

	/// <summary>
	/// The color associated with this line
	/// </summary>
	[DataMember(Name = "color")]
	public string Color { get; set; } = string.Empty;

	/// <summary>
	/// The legend for this line
	/// </summary>
	[DataMember(Name = "legend")]
	public string Legend { get; set; } = string.Empty;

	/// <summary>
	/// The type of the line
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Whether to use YMax
	/// </summary>
	[DataMember(Name = "useYMax")]
	public bool UseYMax { get; set; }

	/// <summary>
	/// The description for the line
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The label for the line
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	/// The decimal 
	/// </summary>
	[DataMember(Name = "decimal")]
	public int Decimal { get; set; }

	/// <summary>
	/// The data values for this line
	/// </summary>
	[DataMember(Name = "data")]
	[JsonConverter(typeof(DoubleOrNAConverter))]
	public List<double> Data { get; set; } = new();
}
