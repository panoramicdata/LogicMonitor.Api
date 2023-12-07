namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Custom graph widget graph info
/// </summary>
[DataContract]
public class CustomGraphWidgetGraphInfo : IdentifiedItem
{
	/// <summary>
	/// The aggregate
	/// </summary>
	[DataMember(Name = "aggregate")]
	public bool Aggregate { get; set; }

	/// <summary>
	/// Whether descending
	/// </summary>
	[DataMember(Name = "desc")]
	public bool IsDescending { get; set; }

	/// <summary>
	/// The maximum value that should be displayed on the y-axis
	/// </summary>
	[DataMember(Name = "maxValue")]
	public double MaxValue { get; set; }

	/// <summary>
	/// The minimum value that should be displayed on the y-axis
	/// </summary>
	[DataMember(Name = "minValue")]
	public double MinValue { get; set; }

	/// <summary>
	/// The vertical label
	/// </summary>
	[DataMember(Name = "verticalLabel")]
	public string VerticalLabel { get; set; } = string.Empty;

	/// <summary>
	/// The top x
	/// </summary>
	[DataMember(Name = "topX")]
	public int TopX { get; set; }

	/// <summary>
	/// The scale unit
	/// </summary>
	[DataMember(Name = "scaleUnit")]
	public int ScaleUnit { get; set; }

	/// <summary>
	/// The data points
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<CustomGraphWidgetDataPoint> DataPoints { get; set; } = [];

	/// <summary>
	/// The virtual data points
	/// </summary>
	[DataMember(Name = "virtualDataPoints")]
	public List<object> VirtualDataPoints { get; set; } = [];

	/// <summary>
	/// The global consolidation function
	/// </summary>
	[DataMember(Name = "globalConsolidateFunction")]
	public string GlobalConsolidateFunction { get; set; } = string.Empty;
}
