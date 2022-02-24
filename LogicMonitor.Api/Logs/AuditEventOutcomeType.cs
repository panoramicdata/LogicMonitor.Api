namespace LogicMonitor.Api.Logs;

/// <summary>
/// The entity type of an audit event
/// </summary>
public enum AuditEventOutcomeType
{
	/// <summary>
	/// Unknown
	/// </summary>
	Unknown,

	/// <summary>
	/// Success
	/// </summary>
	Success,

	/// <summary>
	/// Failure
	/// </summary>
	Failure,
}
