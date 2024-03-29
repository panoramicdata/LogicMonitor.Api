namespace LogicMonitor.Api.Collectors;

/// <summary>
///    ExecuteDebugCommandResponse information
/// </summary>
[DataContract]
public class ExecuteDebugCommandResponse
{
	/// <summary>
	///    The output
	/// </summary>
	[DataMember(Name = "output")]
	public string Output { get; set; } = string.Empty;

	/// <summary>
	///    The session ID
	/// </summary>
	[DataMember(Name = "sessionId")]
	public int SessionId { get; set; }

	/// <summary>
	///    The command
	/// </summary>
	[DataMember(Name = "cmdline")]
	public string Command { get; set; } = string.Empty;

	/// <summary>
	///    The command context
	/// </summary>
	[DataMember(Name = "cmdContext")]
	public string CommandContext { get; set; } = string.Empty;

	/// <summary>
	///    The error message (if any)
	/// </summary>
	[DataMember(Name = "errorMessage")]
	public string ErrorMessage { get; set; } = string.Empty;

	/// <summary>
	///    The error detail (if any)
	/// </summary>
	[DataMember(Name = "errorDetail")]
	public object ErrorDetail { get; set; } = new();

	/// <summary>
	///    The error message (if any)
	/// </summary>
	[DataMember(Name = "errorCode")]
	public int ErrorCode { get; set; }
}
