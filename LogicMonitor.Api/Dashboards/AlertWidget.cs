namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     An alert widget
/// </summary>
[DataContract]
public class AlertWidget : Widget
{
	/// <summary>
	///     The alert filter
	/// </summary>
	[DataMember(Name = "filters")]
	public AlertWidgetFilter AlertFilter { get; set; } = new();

	/// <summary>
	///     The alert filter
	/// </summary>
	[DataMember(Name = "parsedFilters")]
	public ParsedAlertFilters ParsedAlertFilters { get; set; } = new();

	/// <summary>
	///     The alert filter
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
