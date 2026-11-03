namespace LogicMonitor.Api.Logs;

/// <summary>
/// An aggregate column of a saved log query
/// </summary>
[DataContract]
public class LogQueryAggregateColumn
{
	/// <summary>
	/// The column
	/// </summary>
	[DataMember(Name = "column")]
	public string Column { get; set; } = string.Empty;

	/// <summary>
	/// The column size
	/// </summary>
	[DataMember(Name = "columnSize")]
	public int ColumnSize { get; set; }
}
