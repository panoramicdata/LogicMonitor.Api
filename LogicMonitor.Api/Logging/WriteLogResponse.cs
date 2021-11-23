namespace LogicMonitor.Api.Logging;

/// <summary>
/// A response to a WriteLogRequest
/// </summary>
[DataContract]
public class WriteLogResponse
{
	/// <summary>
	/// Whether the log request was accepted
	/// </summary>
	[DataMember(Name = "success")]
	public bool Success { get; set; }

	/// <summary>
	/// The response message
	/// </summary>
	[DataMember(Name = "message")]
	public string Message { get; set; }

	/// <summary>
	/// Errors
	/// </summary>
	[DataMember(Name = "errors")]
	public List<WriteLogError> Errors { get; set; }
}
