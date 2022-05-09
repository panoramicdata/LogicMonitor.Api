namespace LogicMonitor.Api.Logs;

/// <summary>
/// The action type of an audit event
/// </summary>
public enum AuditEventActionType
{
	/// <summary>
	/// Unknown / not set
	/// </summary>
	None,

	/// <summary>
	/// Create
	/// </summary>
	Create,

	/// <summary>
	/// Read
	/// </summary>
	Read,

	/// <summary>
	/// Update
	/// </summary>
	Update,

	/// <summary>
	/// Delete
	/// </summary>
	Delete,

	/// <summary>
	/// Scheduled health check script
	/// </summary>
	ScheduledHealthCheckScript,
	Login,
	GeneralApi,
}
