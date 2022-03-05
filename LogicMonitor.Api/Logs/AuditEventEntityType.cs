namespace LogicMonitor.Api.Logs;

/// <summary>
/// The entity type of an audit event
/// </summary>
public enum AuditEventEntityType
{
	/// <summary>
	/// Unknown
	/// </summary>
	None,

	/// <summary>
	/// Resource
	/// </summary>
	Resource,

	/// <summary>
	/// Device DataSource Instance
	/// </summary>
	DeviceDataSourceInstance,

	/// <summary>
	/// Scheduled Down Time
	/// </summary>
	ScheduledDownTime,

	/// <summary>
	/// All Collectors
	/// </summary>
	AllCollectors,

	/// <summary>
	/// A ResourceGroup property
	/// </summary>
	ResourceGroupProperty,
}
