namespace LogicMonitor.Api.Logs;

/// <summary>
/// A log query group to import
/// </summary>
[DataContract]
public class LogQueryGroupImportItem
{
	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The log query
	/// </summary>
	[DataMember(Name = "logQuery")]
	public List<LogQueryImportItem> LogQuery { get; set; } = [];

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}
