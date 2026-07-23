namespace LogicMonitor.Api.Logs;

/// <summary>
///     The result of an LM Logs search. The search runs asynchronously: a submit returns a
///     <see cref="QueryId"/>, and polling continues until <see cref="Progress"/> reaches 1.0 and/or
///     <see cref="Logs"/> is populated.
/// </summary>
[DataContract]
public class LogQueryResult
{
	/// <summary>
	///     The server-assigned id of the search.
	/// </summary>
	[DataMember(Name = "queryId")]
	public string QueryId { get; set; } = string.Empty;

	/// <summary>
	///     Search completion progress, 0.0 to 1.0. Null while the search is still initialising.
	/// </summary>
	[DataMember(Name = "progress")]
	public double? Progress { get; set; }

	/// <summary>
	///     The matched log lines. Null until the search has produced results.
	/// </summary>
	[DataMember(Name = "logs")]
	public List<LogMessage>? Logs { get; set; }
}
