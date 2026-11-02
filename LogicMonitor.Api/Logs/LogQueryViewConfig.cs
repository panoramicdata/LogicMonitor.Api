namespace LogicMonitor.Api.Logs;

/// <summary>
/// How a saved log query is displayed
/// </summary>
[DataContract]
public class LogQueryViewConfig
{
	/// <summary>
	/// The aggregate config
	/// </summary>
	[DataMember(Name = "aggregateConfig")]
	public LogQueryAggregateConfig? AggregateConfig { get; set; }

	/// <summary>
	/// The field panel
	/// </summary>
	[DataMember(Name = "fieldPanel")]
	public List<LogQueryFieldPanelItem> FieldPanel { get; set; } = [];
}
