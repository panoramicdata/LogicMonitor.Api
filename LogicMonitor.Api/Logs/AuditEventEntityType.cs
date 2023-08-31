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
	/// Device
	/// </summary>
	CollectorGroup,

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
	/// A ResourceGroup Property
	/// </summary>
	ResourceGroupProperty,

	/// <summary>
	/// ResourceGroup
	/// </summary>
	ResourceGroup,

	/// <summary>
	/// DataSource
	/// </summary>
	DataSource,

	/// <summary>
	/// A Resource Property
	/// </summary>
	ResourceProperty,

	/// <summary>
	/// ResourceGroups
	/// </summary>
	ResourceGroups,

	/// <summary>
	/// User account
	/// </summary>
	Account,

	/// <summary>
	/// DataSourceGraph
	/// </summary>
	DataSourceGraph,

	/// <summary>
	/// Alert Note
	/// </summary>
	AlertNote,

	/// <summary>
	/// Ops Note
	/// </summary>
	OpsNote,

	/// <summary>
	/// Alert Rule
	/// </summary>
	AlertRule,

	/// <summary>
	/// User Role
	/// </summary>
	UserRole,

	/// <summary>
	/// ConfigSource
	/// </summary>
	ConfigSource,

	/// <summary>
	/// ConfigSourceInstance
	/// </summary>
	ConfigSourceInstance,

	/// <summary>
	/// Collector
	/// </summary>
	Collector,
}
