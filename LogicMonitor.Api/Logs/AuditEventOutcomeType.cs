namespace LogicMonitor.Api.Logs;

/// <summary>
/// The entity type of an audit event
/// </summary>
public enum AuditEventOutcomeType
{
	/// <summary>
	/// None
	/// </summary>
	None,

	/// <summary>
	/// Success
	/// </summary>
	Success,

	/// <summary>
	/// Failure
	/// </summary>
	Failure,

	/// <summary>
	/// Unknown (e.g. a process has started)
	/// </summary>
	Unknown,
}
