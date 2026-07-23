namespace LogicMonitor.Api.Logs;

/// <summary>
///     A single ingested LM Logs line returned by an LM Logs search.
/// </summary>
[DataContract]
public class LogMessage
{
	/// <summary>
	///     The raw log message.
	/// </summary>
	[DataMember(Name = "message")]
	public string Message { get; set; } = string.Empty;

	/// <summary>
	///     The time the log line was ingested/occurred, in epoch milliseconds.
	/// </summary>
	[DataMember(Name = "timestamp")]
	public long Timestamp { get; set; }

	/// <summary>
	///     The resource the log line was mapped to (name, id and group membership). Kept as a
	///     <see cref="JObject"/> because the shape varies by resource and mapping.
	/// </summary>
	[DataMember(Name = "_resource")]
	public JObject? Resource { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Timestamp}: {Message}";
}
