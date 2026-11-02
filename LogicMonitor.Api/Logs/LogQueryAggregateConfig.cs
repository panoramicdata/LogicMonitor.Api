namespace LogicMonitor.Api.Logs;

/// <summary>
/// The aggregation display settings of a saved log query
/// </summary>
[DataContract]
public class LogQueryAggregateConfig
{
	/// <summary>
	/// The chart metadata
	/// </summary>
	[DataMember(Name = "chartMetadata")]
	public string ChartMetadata { get; set; } = string.Empty;

	/// <summary>
	/// The chart type
	/// </summary>
	[DataMember(Name = "chartType")]
	public string ChartType { get; set; } = string.Empty;

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<LogQueryAggregateColumn> Columns { get; set; } = [];

	/// <summary>
	/// The data
	/// </summary>
	[DataMember(Name = "data")]
	public string Data { get; set; } = string.Empty;

	/// <summary>
	/// The donut
	/// </summary>
	[DataMember(Name = "donut")]
	public string Donut { get; set; } = string.Empty;

	/// <summary>
	/// The label
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	/// The sort by
	/// </summary>
	[DataMember(Name = "sortBy")]
	public string SortBy { get; set; } = string.Empty;
}
