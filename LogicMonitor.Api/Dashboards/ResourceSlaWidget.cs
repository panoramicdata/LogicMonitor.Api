namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Resource SLA widget
/// </summary>
[DataContract]
public class ResourceSlaWidget : Widget, IWidget
{
	/// <summary>
	/// The bottom label
	/// </summary>
	[DataMember(Name = "bottomLabel")]
	public string BottomLabel { get; set; } = string.Empty;

	/// <summary>
	/// Calculation method: 0 \u003d percent all resources available, 1 \u003d average of all SLA metrics
	/// </summary>
	[DataMember(Name = "calculationMethod")]
	public int CalculationMethod { get; set; }

	/// <summary>
	/// Whether a progress bar is displayed in list mode
	/// </summary>
	[DataMember(Name = "displayPercentageBar")]
	public bool DisplayPercentageBar { get; set; }

	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<ResourceSlaWidgetMetric> Metrics { get; set; } = [];

	/// <summary>
	/// The daysInWeek
	/// </summary>
	[DataMember(Name = "daysInWeek")]
	public string DaysInWeek { get; set; } = string.Empty;

	/// <summary>
	/// The periodInOneDay
	/// </summary>
	[DataMember(Name = "periodInOneDay")]
	public string PeriodInOneDay { get; set; } = string.Empty;

	/// <summary>
	/// The unmonitoredTimeType
	/// </summary>
	[DataMember(Name = "unmonitoredTimeType")]
	public int UnmonitoredTimeType { get; set; }

	/// <summary>
	/// The displayType
	/// </summary>
	[DataMember(Name = "displayType")]
	public int DisplayType { get; set; }

	/// <summary>
	/// The unit label
	/// </summary>
	[DataMember(Name = "unitLabel")]
	public string UnitLabel { get; set; } = string.Empty;

	/// <summary>
	/// The top X
	/// </summary>
	[DataMember(Name = "topX")]
	public int TopX { get; set; }

	/// <summary>
	/// The color thresholds
	/// </summary>
	[DataMember(Name = "colorThresholds")]
	public List<ColorThreshold> ColorThresholds { get; set; } = [];

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
