namespace LogicMonitor.Api.Backup;

/// <summary>
///     A configuration
/// </summary>
[DataContract]
public class ConfigurationBackup
{
	/// <summary>
	///     AccountSettings
	/// </summary>
	[DataMember]
	public AccountSettings AccountSettings { get; set; } = new();

	/// <summary>
	///     Alert rules
	/// </summary>
	[DataMember]
	public List<AlertRule> AlertRules { get; set; } = new();

	/// <summary>
	///     AppliesTo Functions
	/// </summary>
	[DataMember]
	public List<AppliesToFunction> AppliesToFunctions { get; set; } = new();

	/// <summary>
	///     Escalation chain
	/// </summary>
	[DataMember]
	public List<EscalationChain> EscalationChains { get; set; } = new();

	/// <summary>
	///     Collectors
	/// </summary>
	[DataMember]
	public List<Collector> Collectors { get; set; } = new();

	/// <summary>
	///     Company Logo
	/// </summary>
	[DataMember]
	public string CompanyLogo { get; set; } = string.Empty;

	/// <summary>
	///     ConfigSources
	/// </summary>
	[DataMember]
	public List<ConfigSource> ConfigSources { get; set; } = new();

	/// <summary>
	///    EventSources
	/// </summary>
	[DataMember]
	public List<EventSource> EventSources { get; set; } = new();

	/// <summary>
	///     Dashboards
	/// </summary>
	[DataMember]
	public List<Dashboard> Dashboards { get; set; } = new();

	/// <summary>
	///     Widgets
	/// </summary>
	[DataMember]
	public List<Widget> Widgets { get; set; } = new();

	/// <summary>
	///     Dashboard groups
	/// </summary>
	[DataMember]
	public List<DashboardGroup> DashboardGroups { get; set; } = new();

	/// <summary>
	///     Device groups
	/// </summary>
	[DataMember]
	public List<DeviceGroup> DeviceGroups { get; set; } = new();

	/// <summary>
	///     Devices
	/// </summary>
	[DataMember]
	public List<Device> Devices { get; set; } = new();

	/// <summary>
	///     DataSources
	/// </summary>
	[DataMember]
	public List<DataSource> DataSources { get; set; } = new();

	/// <summary>
	///     DataSource graphs
	/// </summary>
	[DataMember]
	public List<DataSourceOverviewGraph> DataSourceGraphs { get; set; } = new();

	/// <summary>
	///     DataSource overview graphs
	/// </summary>
	[DataMember]
	public List<DataSourceOverviewGraph> DataSourceOverviewGraphs { get; set; } = new();

	/// <summary>
	///     DataSourceDataPoint configurations
	/// </summary>
	[DataMember]
	public List<DataPointConfiguration> DataSourceDataPoints { get; set; } = new();

	/// <summary>
	///     External alert destinations
	/// </summary>
	[DataMember]
	public List<ExternalAlertDestination> ExternalAlertDestinations { get; set; } = new();

	/// <summary>
	///     Integrations
	/// </summary>
	[DataMember]
	public List<Integration> Integrations { get; set; } = new();

	/// <summary>
	///     JobMonitors
	/// </summary>
	[DataMember]
	public List<JobMonitor> JobMonitors { get; set; } = new();

	/// <summary>
	///     Log Items
	/// </summary>
	[DataMember]
	public List<LogItem> LogItems { get; set; } = new();

	/// <summary>
	///     Login Logo
	/// </summary>
	[DataMember]
	public string LoginLogo { get; set; } = string.Empty;

	/// <summary>
	///     Message template set
	/// </summary>
	[DataMember]
	public MessageTemplateSet MessageTemplateSet { get; set; } = new();

	/// <summary>
	///     NetscanPolicies
	/// </summary>
	[DataMember]
	public List<Netscan> Netscans { get; set; } = new();

	/// <summary>
	///     NewUserMessageTemplate
	/// </summary>
	[DataMember]
	public NewUserMessageTemplate NewUserMessageTemplate { get; set; } = new();

	/// <summary>
	///     OpsNotes
	/// </summary>
	[DataMember]
	public List<OpsNote> OpsNotes { get; set; } = new();

	/// <summary>
	///    PropertySources
	/// </summary>
	[DataMember]
	public List<PropertySource> PropertySources { get; set; } = new();

	/// <summary>
	///     Recipient groups
	/// </summary>
	[DataMember]
	public List<RecipientGroup> RecipientGroups { get; set; } = new();

	/// <summary>
	///     ScheduledDownTimes
	/// </summary>
	[DataMember]
	public List<ScheduledDownTime> ScheduledDownTimes { get; set; } = new();

	/// <summary>
	///     Websites
	/// </summary>
	[DataMember]
	public List<Website> Websites { get; set; } = new();

	/// <summary>
	///     Website Groups
	/// </summary>
	[DataMember]
	public List<WebsiteGroup> WebsiteGroups { get; set; } = new();

	/// <summary>
	///     SnmpSysOidMaps
	/// </summary>
	[DataMember]
	public List<SnmpSysOidMap> SnmpSysOidMaps { get; set; } = new();

	/// <summary>
	///     RoleGroups
	/// </summary>
	[DataMember]
	public List<RoleGroup> RoleGroups { get; set; } = new();

	/// <summary>
	///     Roles
	/// </summary>
	[DataMember]
	public List<Role> Roles { get; set; } = new();

	/// <summary>
	///     UserGroups
	/// </summary>
	[DataMember]
	public List<UserGroup> UserGroups { get; set; } = new();

	/// <summary>
	///     Users
	/// </summary>
	[DataMember]
	public List<User> Users { get; set; } = new();

	/// <summary>
	///     SingleSignOn
	/// </summary>
	[DataMember]
	public SingleSignOn SingleSignOn { get; set; } = new();
}
