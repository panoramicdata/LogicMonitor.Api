namespace LogicMonitor.Api.Reports;

/// <summary>
/// A Logs report, whose rows come from an LM Logs query
/// </summary>
[DataContract]
public class LogsReport : DateRangeReport
{
	/// <summary>
	/// The query that selects the report's log entries
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// The timezone, for example Europe/London
	/// </summary>
	[DataMember(Name = "timezone")]
	public string Timezone { get; set; } = string.Empty;

	/// <summary>
	/// The log partitions
	/// </summary>
	[DataMember(Name = "partitions")]
	public List<object> Partitions { get; set; } = [];

	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<ReportColumn> Columns { get; set; } = [];

	/// <summary>
	/// The kind of log data, for example both
	/// </summary>
	[DataMember(Name = "logsDataType")]
	public string LogsDataType { get; set; } = string.Empty;

	/// <summary>
	/// Whether visualisation is enabled
	/// </summary>
	[DataMember(Name = "isVisualizationEnabled")]
	public bool IsVisualizationEnabled { get; set; }

	/// <summary>
	/// The graph type
	/// </summary>
	[DataMember(Name = "graphType")]
	public string GraphType { get; set; } = string.Empty;

	/// <summary>
	/// The pie chart's data column
	/// </summary>
	[DataMember(Name = "pieDataColumn")]
	public string PieDataColumn { get; set; } = string.Empty;

	/// <summary>
	/// The pie chart's slice label
	/// </summary>
	[DataMember(Name = "pieSliceLabel")]
	public string PieSliceLabel { get; set; } = string.Empty;

	/// <summary>
	/// The ordering configuration
	/// </summary>
	[DataMember(Name = "orderingConfig")]
	public List<object> OrderingConfig { get; set; } = [];
}
