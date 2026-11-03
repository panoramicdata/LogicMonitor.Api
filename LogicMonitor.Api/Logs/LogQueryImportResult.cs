namespace LogicMonitor.Api.Logs;

/// <summary>
/// The result of importing a log query
/// </summary>
[DataContract]
public class LogQueryImportResult
{
	/// <summary>
	/// Saved query time range.
	/// </summary>
	[DataMember(Name = "duration")]
	public string Duration { get; set; } = string.Empty;

	/// <summary>
	/// The is valid
	/// </summary>
	[DataMember(Name = "isValid")]
	public bool IsValid { get; set; }

	/// <summary>
	/// Saved query label.
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	/// Optional log partitions selected for the query.
	/// </summary>
	[DataMember(Name = "partitions")]
	public List<string> Partitions { get; set; } = [];

	/// <summary>
	/// Log query expression.
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// Validation or import status reason for the query.
	/// </summary>
	[DataMember(Name = "reason")]
	public string Reason { get; set; } = string.Empty;

	/// <summary>
	/// Optional saved view configuration for the query.
	/// </summary>
	[DataMember(Name = "viewConfig")]
	public LogQueryViewConfig? ViewConfig { get; set; }
}
