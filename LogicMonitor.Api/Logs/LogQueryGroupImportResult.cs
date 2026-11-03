namespace LogicMonitor.Api.Logs;

/// <summary>
/// The result of importing a log query group
/// </summary>
[DataContract]
public class LogQueryGroupImportResult
{
	/// <summary>
	/// Description of the target log query group.
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The is valid
	/// </summary>
	[DataMember(Name = "isValid")]
	public bool IsValid { get; set; }

	/// <summary>
	/// Per-query import results for this group.
	/// </summary>
	[DataMember(Name = "logQuery")]
	public List<LogQueryImportResult> LogQuery { get; set; } = [];

	/// <summary>
	/// Name of the target log query group.
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Validation or import status reason for the group.
	/// </summary>
	[DataMember(Name = "reason")]
	public string Reason { get; set; } = string.Empty;
}
