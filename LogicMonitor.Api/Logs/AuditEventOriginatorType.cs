namespace LogicMonitor.Api.Logs;

/// <summary>
/// The originator type of an audit event
/// </summary>
public enum AuditEventOriginatorType
{
	/// <summary>
	/// Unknown / not set
	/// </summary>
	Unknown,

	/// <summary>
	/// Collector Active Discovery
	/// </summary>
	System,

	/// <summary>
	/// Collector Kubenetes
	/// </summary>
	CollectorKubernetes,

	/// <summary>
	/// Collector (other)
	/// </summary>
	CollectorOther,

	/// <summary>
	/// User-initiated
	/// </summary>
	User
}
