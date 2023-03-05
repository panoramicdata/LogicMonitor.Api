namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     Complex graph widget
/// </summary>
[DataContract]
public class CustomGraphWidget : GraphWidget
{
	/// <summary>
	/// The graph info
	/// </summary>
	[DataMember(Name = "graphInfo")]
	public CustomGraphWidgetGraphInfo GraphInfo { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
