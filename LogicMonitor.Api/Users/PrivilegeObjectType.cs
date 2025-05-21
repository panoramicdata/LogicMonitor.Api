namespace LogicMonitor.Api.Users;

/// <summary>
/// A role privilege object type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum PrivilegeObjectType
{
	/// <summary>
	/// Dashboard
	/// </summary>
	[EnumMember(Value = "dashboard")]
	Dashboard,

	/// <summary>
	/// DashboardGroup
	/// </summary>
	[EnumMember(Value = "dashboard_group")]
	DashboardGroup,

	/// <summary>
	/// ResourceGroup
	/// </summary>
	[EnumMember(Value = "host_group")]
	ResourceGroup,

	/// <summary>
	/// Help
	/// </summary>
	[EnumMember(Value = "help")]
	Help,

	/// <summary>
	/// ReportGroup
	/// </summary>
	[EnumMember(Value = "report_group")]
	ReportGroup,

	/// <summary>
	/// WebsiteGroup
	/// </summary>
	[EnumMember(Value = "website_group")]
	WebsiteGroup,

	/// <summary>
	/// Setting
	/// </summary>
	[EnumMember(Value = "setting")]
	Setting,

	/// <summary>
	/// Remote session
	/// </summary>
	[EnumMember(Value = "remoteSession")]
	RemoteSession,

	/// <summary>
	/// Resource Dashboard
	/// </summary>
	[EnumMember(Value = "deviceDashboard")]
	ResourceDashboard,

	/// <summary>
	/// ConfigNeedResourceManagePermission
	/// </summary>
	[EnumMember(Value = "configNeedDeviceManagePermission")]
	ConfigNeedResourceManagePermission,

	/// <summary>
	/// Map
	/// </summary>
	[EnumMember(Value = "map")]
	Map,

	/// <summary>
	/// Resource Map Tab
	/// </summary>
	[EnumMember(Value = "resourceMapTab")]
	ResourceMapTab,

	/// <summary>
	/// Logs
	/// </summary>
	[EnumMember(Value = "logs")]
	Logs,

	/// <summary>
	/// TracesManageTab
	/// </summary>
	[EnumMember(Value = "tracesManageTab")]
	TracesManageTab,

	/// <summary>
	/// ManualMapping
	/// </summary>
	[DataMember(Name = "manualMapping")]
	ManualMapping,

	/// <summary>
	/// Dexda
	/// </summary>
	[DataMember(Name = "dexda")]
	Dexda,

	/// <summary>
	/// Module
	/// </summary>
	[DataMember(Name = "module")]
	Module,

	/// <summary>
	/// Cost Optimization
	/// </summary>
	[DataMember(Name = "costOptimization")]
	CostOptimization,

	/// <summary>
	/// Cost Optimization Recommendations
	/// </summary>
	[EnumMember(Value = "costOptimization.recommendations")]
	CostOptimizationRecommendations,

	/// <summary>
	/// Cost Optimization Billing Account
	/// </summary>
	[EnumMember(Value = "costOptimization.billing.account")]
	CostOptimizationBillingAccount,

	/// <summary>
	/// Cost Optimization Billing Management configuration
	/// </summary>
	[EnumMember(Value = "costOptimization.billing.management.configuration")]
	CostOptimizationBillingManagementConfiguration,

	/// <summary>
	/// Diagnostic Source
	/// </summary>
	[EnumMember(Value = "diagnosticSource")]
	DiagnosticSource
}