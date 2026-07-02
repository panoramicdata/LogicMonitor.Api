namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// An Advanced Metrics Widget, which visualises metric data via a
/// LogicMonitor Query Language (LMQL) query.
/// </summary>
/// <remarks>
/// See <see href="https://www.logicmonitor.com/support/advanced-metrics-widget">Advanced Metrics Widget</see>
/// and <see href="https://www.logicmonitor.com/support/logicmonitor-query-language-reference">LMQL Reference</see>
/// for the query syntax.
/// </remarks>
[DataContract]
public class AdvancedMetricsWidget : Widget, IWidget
{
	/// <summary>
	/// The LMQL query that drives the widget's data.
	/// </summary>
	[DataMember(Name = "lmql")]
	public string LmqlQuery { get; set; } = string.Empty;

	/// <summary>
	/// Display settings (column visibility, formatting, etc.).
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
