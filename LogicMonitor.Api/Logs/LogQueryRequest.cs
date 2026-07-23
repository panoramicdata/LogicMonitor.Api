namespace LogicMonitor.Api.Logs;

/// <summary>
///     A request to query ingested LM Logs data (not audit/access logs — see <see cref="LogItem"/> for those).
///     Submitted to the LM Logs search API, which runs asynchronously (submit then poll).
/// </summary>
[DataContract]
public class LogQueryRequest
{
	/// <summary>
	///     The LM Logs query-language string (e.g. "error", or a field/glob expression). Defaults to "*" (all).
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = "*";

	/// <summary>
	///     The start of the time range, in epoch milliseconds.
	/// </summary>
	[DataMember(Name = "startTime")]
	public long StartTime { get; set; }

	/// <summary>
	///     The end of the time range, in epoch milliseconds.
	/// </summary>
	[DataMember(Name = "endTime")]
	public long EndTime { get; set; }

	/// <summary>
	///     The maximum number of log lines to return.
	/// </summary>
	[DataMember(Name = "size")]
	public int Size { get; set; } = 50;

	/// <summary>
	///     Optional resource (device) ids to restrict the query to.
	/// </summary>
	[DataMember(Name = "deviceIds")]
	public List<int>? DeviceIds { get; set; }
}
