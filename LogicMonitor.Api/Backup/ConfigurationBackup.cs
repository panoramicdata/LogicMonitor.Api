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
	public AccountSettings AccountSettings { get; set; }

	/// <summary>
	///     Alert rules
	/// </summary>
	[DataMember]
	public List<AlertRule> AlertRules { get; set; }

	/// <summary>
	///     AppliesTo Functions
	/// </summary>
	[DataMember]
	public List<AppliesToFunction> AppliesToFunctions { get; set; }

	/// <summary>
	///     Escalation chain
	/// </summary>
	[DataMember]
	public List<EscalationChain> EscalationChains { get; set; }

	/// <summary>
	///     Collectors
	/// </summary>
	[DataMember]
	public List<Collector> Collectors { get; set; }

	/// <summary>
	///     Company Logo
	/// </summary>
	[DataMember]
	public string CompanyLogo { get; set; }

	/// <summary>
	///     ConfigSources
	/// </summary>
	[DataMember]
	public List<ConfigSource> ConfigSources { get; set; }

	/// <summary>
	///    EventSources
	/// </summary>
	[DataMember]
	public List<EventSource> EventSources { get; set; }

	/// <summary>
	///     Dashboards
	/// </summary>
	[DataMember]
	public List<Dashboard> Dashboards { get; set; }

	/// <summary>
	///     Widgets
	/// </summary>
	[DataMember]
	public List<Widget> Widgets { get; set; }

	/// <summary>
	///     Dashboard groups
	/// </summary>
	[DataMember]
	public List<DashboardGroup> DashboardGroups { get; set; }

	/// <summary>
	///     Device groups
	/// </summary>
	[DataMember]
	public List<DeviceGroup> DeviceGroups { get; set; }

	/// <summary>
	///     Devices
	/// </summary>
	[DataMember]
	public List<Device> Devices { get; set; }

	/// <summary>
	///     DataSources
	/// </summary>
	[DataMember]
	public List<DataSource> DataSources { get; set; }

	/// <summary>
	///     DataSource graphs
	/// </summary>
	[DataMember]
	public List<DataSourceGraph> DataSourceGraphs { get; set; }

	/// <summary>
	///     DataSource overview graphs
	/// </summary>
	[DataMember]
	public List<DataSourceGraph> DataSourceOverviewGraphs { get; set; }

	/// <summary>
	///     DataSourceDataPoint configurations
	/// </summary>
	[DataMember]
	public List<DataPointConfiguration> DataSourceDataPoints { get; set; }

	/// <summary>
	///     External alert destinations
	/// </summary>
	[DataMember]
	public List<ExternalAlertDestination> ExternalAlertDestinations { get; set; }

	/// <summary>
	///     Integrations
	/// </summary>
	[DataMember]
	public List<Integration> Integrations { get; set; }

	/// <summary>
	///     JobMonitors
	/// </summary>
	[DataMember]
	public List<JobMonitor> JobMonitors { get; set; }

	/// <summary>
	///     Log Items
	/// </summary>
	[DataMember]
	public List<LogItem> LogItems { get; set; }

	/// <summary>
	///     Login Logo
	/// </summary>
	[DataMember]
	public string LoginLogo { get; set; }

	/// <summary>
	///     Message template set
	/// </summary>
	[DataMember]
	public MessageTemplateSet MessageTemplateSet { get; set; }

	/// <summary>
	///     NetscanPolicies
	/// </summary>
	[DataMember]
	public List<Netscans.Netscan> Netscans { get; set; }

	/// <summary>
	///     NewUserMessageTemplate
	/// </summary>
	[DataMember]
	public NewUserMessageTemplate NewUserMessageTemplate { get; set; }

	/// <summary>
	///     OpsNotes
	/// </summary>
	[DataMember]
	public List<OpsNote> OpsNotes { get; set; }

	/// <summary>
	///    PropertySources
	/// </summary>
	[DataMember]
	public List<PropertySource> PropertySources { get; set; }

	/// <summary>
	///     Recipient groups
	/// </summary>
	[DataMember]
	public List<RecipientGroup> RecipientGroups { get; set; }

	/// <summary>
	///     ScheduledDownTimes
	/// </summary>
	[DataMember]
	public List<ScheduledDownTime> ScheduledDownTimes { get; set; }

	/// <summary>
	///     Websites
	/// </summary>
	[DataMember]
	public List<Website> Websites { get; set; }

	/// <summary>
	///     Website Groups
	/// </summary>
	[DataMember]
	public List<WebsiteGroup> WebsiteGroups { get; set; }

	/// <summary>
	///     SnmpSysOidMaps
	/// </summary>
	[DataMember]
	public List<SnmpSysOidMap> SnmpSysOidMaps { get; set; }

	/// <summary>
	///     RoleGroups
	/// </summary>
	[DataMember]
	public List<RoleGroup> RoleGroups { get; set; }

	/// <summary>
	///     Roles
	/// </summary>
	[DataMember]
	public List<Role> Roles { get; set; }

	/// <summary>
	///     UserGroups
	/// </summary>
	[DataMember]
	public List<UserGroup> UserGroups { get; set; }

	/// <summary>
	///     Users
	/// </summary>
	[DataMember]
	public List<User> Users { get; set; }

	/// <summary>
	///     SingleSignOn
	/// </summary>
	[DataMember]
	public SingleSignOn SingleSignOn { get; set; }
}
