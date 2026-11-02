namespace LogicMonitor.Api.Logs;

/// <summary>
/// A log query to import
/// </summary>
[DataContract]
public class LogQueryImportItem
{
	/// <summary>
	/// The duration
	/// </summary>
	[DataMember(Name = "duration")]
	public string Duration { get; set; } = string.Empty;

	/// <summary>
	/// The label
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	/// The partitions
	/// </summary>
	[DataMember(Name = "partitions")]
	public List<string> Partitions { get; set; } = [];

	/// <summary>
	/// The query
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// The version
	/// </summary>
	[DataMember(Name = "version")]
	public string Version { get; set; } = string.Empty;

	/// <summary>
	/// The view config
	/// </summary>
	[DataMember(Name = "viewConfig")]
	public LogQueryViewConfig? ViewConfig { get; set; }
}
