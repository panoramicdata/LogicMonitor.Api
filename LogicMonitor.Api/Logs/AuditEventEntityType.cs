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
	/// Resource DataSource Instance
	/// </summary>
	ResourceDataSourceInstance,

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

	/// <summary>
	/// Collector Group
	/// </summary>
	CollectorGroup,

	/// <summary>
	/// API Token
	/// </summary>
	ApiToken,

	/// <summary>
	/// Dashboard
	/// </summary>
	Dashboard,

	/// <summary>
	/// Dashboard Widget
	/// </summary>
	Widget,

	/// <summary>
	/// Report
	/// </summary>
	Report,

	/// <summary>
	/// Dashboard Group
	/// </summary>
	DashboardGroup,

	/// <summary>
	/// Test Script Scheduled
	/// </summary>
	TestScriptScheduled,

	/// <summary>
	/// Property Source
	/// </summary>
	PropertySource,

	/// <summary>
	/// Topology Source
	/// </summary>
	TopologySource
}
