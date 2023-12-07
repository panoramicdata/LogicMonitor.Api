namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Website SLA widget
/// </summary>
[DataContract]
public class WebsiteSlaWidget : Widget, IWidget
{
	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "items")]
	public List<WebsiteSlaWidgetMetric> Items { get; set; } = [];

	/// <summary>
	/// The color thresholds
	/// </summary>
	[DataMember(Name = "colorThresholds")]
	public List<ColorThreshold> ColorThresholds { get; set; } = [];

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
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
