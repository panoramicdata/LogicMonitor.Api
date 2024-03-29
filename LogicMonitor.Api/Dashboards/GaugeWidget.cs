namespace LogicMonitor.Api.Dashboards;

/// <summary>A Gauge widget</summary>
public class GaugeWidget : Widget, IWidget
{
	/// <summary>
	///     The display type
	/// </summary>
	[DataMember(Name = "displayType")]
	public int DisplayType { get; set; }

	/// <summary>
	///     The display unit
	/// </summary>
	[DataMember(Name = "displayUnit")]
	public string DisplayUnit { get; set; } = string.Empty;

	/// <summary>
	///     Whether to show the peak
	/// </summary>
	[DataMember(Name = "showPeak")]
	public bool ShowPeak { get; set; }

	/// <summary>
	///     The peak time range
	/// </summary>
	[DataMember(Name = "peakTimeRange")]
	public string PeakTimeRange { get; set; } = string.Empty;

	/// <summary>
	///     The legend
	/// </summary>
	[DataMember(Name = "legend")]
	public string Legend { get; set; } = string.Empty;

	/// <summary>
	///     The max value
	/// </summary>
	[DataMember(Name = "maxValue")]
	public double MaxValue { get; set; }

	/// <summary>
	///     The min value
	/// </summary>
	[DataMember(Name = "minValue")]
	public double MinValue { get; set; }

	/// <summary>
	///     The data point
	/// </summary>
	[DataMember(Name = "dataPoint")]
	public GaugeWidgetDataPoint DataPoint { get; set; } = new();

	/// <summary>
	///     The color thresholds
	/// </summary>
	[DataMember(Name = "colorThresholds")]
	public object ColorThresholds { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
