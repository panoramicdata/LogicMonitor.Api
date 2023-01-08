namespace LogicMonitor.Api.Collectors;

/// <summary>
///     A LogicMonitor Collector
/// </summary>
[DataContract]
public class Collector : DescribedItem, IHasCustomProperties, IHasEndpoint
{
	/// <summary>
	///     The collector down acknowledgement comment
	/// </summary>
	[DataMember(Name = "ackComment")]
	public string AckComment { get; set; }

	/// <summary>
	///     Platform
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "acked")]
	public bool Acked { get; set; }

	/// <summary>
	///     The user that acknowledged the collector down alert
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "ackedBy")]
	public string AckedBy { get; set; }

	/// <summary>
	///     Acked on local (human-readable acknowledgement DateTime as a string)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "ackedOnLocal")]
	public string AckedOnLocalString { get; set; }

	/// <summary>
	///     When the collector was Acked in UTC (or null if not acked)
	/// </summary>
	[IgnoreDataMember]
	public DateTime? AckedOnUtc => AckedOnUtcTimestampUtc?.ToNullableDateTimeUtc();

	/// <summary>
	///     The UTC time that the collector down alert was acknowledged as a string
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "ackedOn")]
	public long? AckedOnUtcTimestampUtc { get; set; }

	/// <summary>
	///     The architecture
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "arch")]
	public string Architecture { get; set; }

	/// <summary>
	///     The automatic upgrade information
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "automaticUpgradeInfo", IsRequired = false)]
	public object AutomaticUpgradeInfo { get; set; } // TODO - use the AutomaticUpgradeInfo class

	/// <summary>
	///     This collector's backup collector's Id
	/// </summary>
	[DataMember(Name = "backupAgentId")]
	public int BackupCollectorId { get; set; }

	/// <summary>
	///     Netscan version
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "build")]
	public int Build { get; set; }

	/// <summary>
	///     Whether the collector can be downgraded
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "canDowngrade")]
	public bool CanDowngrade { get; set; }

	/// <summary>
	///     The reason that the collector can/cannot be downgraded
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "canDowngradeReason")]
	public string CanDowngradeReason { get; set; }

	/// <summary>
	///     Whether a clear has been sent for a collector down alert
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "clearSent")]
	public bool ClearSent { get; set; }

	/// <summary>
	///     Collector configuration
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "collectorConf")]
	public string CollectorConfiguration { get; set; }

	/// <summary>
	///     Collector configuration
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "collectorType")]
	public CollectorType Type { get; set; }

	/// <summary>
	///     Configuration fields
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "agentConfFields")]
	public string ConfigurationFields { get; set; }

	/// <summary>
	///     The collector device id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "collectorDeviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	///     The collector group id
	/// </summary>
	[DataMember(Name = "collectorGroupId")]
	public int GroupId { get; set; }

	/// <summary>
	///     The collector group name
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "collectorGroupName")]
	public string GroupName { get; set; }

	/// <summary>
	///     The Agent configuration
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "agentConf")]
	public string Configuration { get; set; }

	/// <summary>
	///     The configuration version
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "confVersion")]
	public int ConfigurationVersion { get; set; }

	/// <summary>
	///     Created on local string
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "createdOnLocal")]
	public string CreatedOnLocalString { get; set; }

	/// <summary>
	///     Created on
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "createdOn")]
	public long CreatedOnTimeStampUtc { get; set; }

	/// <summary>
	///     The credential
	/// </summary>
	[DataMember(Name = "credential")]
	public string Credential { get; set; }

	/// <summary>
	///     The other credential
	/// </summary>
	[DataMember(Name = "credential2")]
	public string Credential2 { get; set; }

	/// <summary>
	///     Custom properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<Property> CustomProperties { get; set; }

	/// <summary>
	///     Number of hosts
	/// </summary>
	[DataMember(Name = "numberOfHosts")]
	public int DeviceCount { get; set; }

	/// <summary>
	///     Whether it is Early Access
	/// </summary>
	[DataMember(Name = "ea")]
	public bool Ea { get; set; }

	/// <summary>
	///     Enable failback
	/// </summary>
	[DataMember(Name = "enableFailBack")]
	public bool EnableFailBack { get; set; }

	/// <summary>
	///     Enable failover on collector device
	/// </summary>
	[DataMember(Name = "enableFailOverOnCollectorDevice")]
	public bool EnableFailOverOnCollectorDevice { get; set; }

	/// <summary>
	///     The escalation chain
	/// </summary>
	[DataMember(Name = "escalatingChain")]
	public EscalationChain EscalationChain { get; set; }

	/// <summary>
	///     Escalating chain Id
	/// </summary>
	[DataMember(Name = "escalatingChainId")]
	public int EscalationChainId { get; set; }

	/// <summary>
	///     Whether it has a fail-over device
	/// </summary>
	[DataMember(Name = "hasFailOverDevice")]
	public bool HasFailOverDevice { get; set; }

	/// <summary>
	///     Hostname
	/// </summary>
	[DataMember(Name = "hostname")]
	public string HostName { get; set; }

	/// <summary>
	///     In SDT
	/// </summary>
	[DataMember(Name = "inSDT")]
	public bool InSdt { get; set; }

	/// <summary>
	///     Whether the collector is down
	/// </summary>
	[DataMember(Name = "isDown")]
	public bool IsDown { get; set; }

	/// <summary>
	///     Whether LM Logs is enabled
	/// </summary>
	[DataMember(Name = "enableLmLogs")]
	public bool IsLmLogsEnabled { get; set; }

	/// <summary>
	///     Last sent notification on local (human-readable acknowledgement DateTime as a string)
	/// </summary>
	[DataMember(Name = "lastSentNotificationOnLocal")]
	public string LastSentNotificationOnLocal { get; set; }

	/// <summary>
	///     Last send notification on UTC
	/// </summary>
	[DataMember(Name = "lastSentNotificationOn")]
	public int LastSentNotificationOnTimeStampUtc { get; set; }

	/// <summary>
	///     Whether it needs AutoCreateCollectorDevice
	/// </summary>
	[DataMember(Name = "needAutoCreateCollectorDevice")]
	public bool NeedAutoCreateCollectorDevice { get; set; }

	/// <summary>
	///     Netscan version
	/// </summary>
	[DataMember(Name = "netscanVersion")]
	public int NetscanVersion { get; set; }

	/// <summary>
	///     Next recipient
	/// </summary>
	[DataMember(Name = "nextRecipient")]
	public int NextRecipient { get; set; }

	/// <summary>
	///     Collector next upgrade info
	/// </summary>
	[DataMember(Name = "nextUpgradeInfo")]
	public CollectorNextUpgradeInfo NextUpgradeInfo { get; set; }

	/// <summary>
	///     The one-time downgrade information
	/// </summary>
	[DataMember(Name = "onetimeDowngradeInfo")]
	public string OnetimeDowngradeInfo { get; set; }

	/// <summary>
	///     One time upgrade info
	/// </summary>
	[DataMember(Name = "onetimeUpgradeInfo")]
	public OneTimeUpgradeInfo OneTimeUpgradeInfo { get; set; }

	/// <summary>
	///     Platform
	/// </summary>
	[DataMember(Name = "platform")]
	public string Platform { get; set; }

	/// <summary>
	///     The predefined configuration
	/// </summary>
	[DataMember(Name = "predefinedConfig")]
	public object PredefinedConfiguration { get; set; }

	/// <summary>
	///     The previous version
	/// </summary>
	[DataMember(Name = "previousVersion")]
	public int PreviousVersion { get; set; }

	/// <summary>
	///     The sbproxyConf file contents
	/// </summary>
	[DataMember(Name = "sbproxyConf")]
	public string ProxyConfiguration { get; set; }

	/// <summary>
	///     Resend interval
	/// </summary>
	[DataMember(Name = "resendIval")]
	public int ResendIntervalSeconds { get; set; }

	/// <summary>
	///     Website configuration
	/// </summary>
	[DataMember(Name = "websiteConf")]
	public string WebsiteConfiguration { get; set; }

	/// <summary>
	///     Number of Website
	/// </summary>
	[DataMember(Name = "numberOfWebsites")]
	public int WebsiteCount { get; set; }

	/// <summary>
	///     Size
	/// </summary>
	[DataMember(Name = "collectorSize")]
	public string Size { get; set; }

	/// <summary>
	///     The specified collector DeviceGroupId
	/// </summary>
	[DataMember(Name = "specifiedCollectorDeviceGroupId")]
	public int SpecifiedCollectorDeviceGroupId { get; set; }

	/// <summary>
	///     Status
	/// </summary>
	[DataMember(Name = "status")]
	public int Status { get; set; }

	/// <summary>
	///     Suppress alert clear
	/// </summary>
	[DataMember(Name = "suppressAlertClear")]
	public bool SuppressAlertClear { get; set; }

	/// <summary>
	///     Whether synthetics is enabled
	/// </summary>
	[DataMember(Name = "syntheticsEnabled")]
	public bool IsSyntheticsEnables { get; set; }

	/// <summary>
	///     Updated on local (human-readable acknowledgement DateTime as a string)
	/// </summary>
	[DataMember(Name = "updatedOnLocal")]
	public string UpdatedOnLocalString { get; set; }

	/// <summary>
	///     The upgrade time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "upgradeTimeEpoch")]
	public long UpgradeTimeUtcSeconds { get; set; }

	/// <summary>
	///     Updated on
	/// </summary>
	[DataMember(Name = "updatedOn")]
	public long? UpdatedOnTimeStampUtc { get; set; }

	/// <summary>
	///     Uptime
	/// </summary>
	[DataMember(Name = "upTime")]
	public int UptimeSeconds { get; set; }

	/// <summary>
	///     The local time of user change
	/// </summary>
	[DataMember(Name = "userChangeOnLocal")]
	public string UserChangeOnLocal { get; set; }

	/// <summary>
	///     The local time of user change
	/// </summary>
	[DataMember(Name = "userChangeOn")]
	public long UserChangeOnUtcSeconds { get; set; }

	/// <summary>
	///     User permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <summary>
	///     User visible hosts number
	/// </summary>
	[DataMember(Name = "userVisibleHostsNum")]
	public int UserVisibleDeviceCount { get; set; }

	/// <summary>
	///     User visible Website number
	/// </summary>
	[DataMember(Name = "userVisibleWebsitesNum")]
	public int UserVisibleWebsiteCount { get; set; }

	/// <summary>
	///     Watchdog configuration
	/// </summary>
	[DataMember(Name = "watchdogConf")]
	public string WatchdogConfiguration { get; set; }

	/// <summary>
	///     Updated on local (human-readable acknowledgement DateTime as a string)
	/// </summary>
	[DataMember(Name = "watchdogUpdatedOnLocal")]
	public string WatchdogUpdatedOnLocal { get; set; }

	/// <summary>
	///     Watchdog updated on
	/// </summary>
	[DataMember(Name = "watchdogUpdatedOn")]
	public long? WatchdogUpdatedOnSeconds { get; set; }

	/// <summary>
	///     When last updated (UTC)
	/// </summary>
	[IgnoreDataMember]
	public DateTime? WatchdogUpdatedOnUtc => WatchdogUpdatedOnSeconds?.ToNullableDateTimeUtc();

	/// <summary>
	///     Wrapper configuration
	/// </summary>
	[DataMember(Name = "wrapperConf")]
	public string WrapperConfiguration { get; set; }

	/// <summary>
	///    The instance count
	/// </summary>
	[DataMember(Name = "numberOfInstances")]
	public int InstanceCount { get; set; }

	/// <summary>
	///     The subUrl for setting by id
	/// </summary>
	public string Endpoint() => "setting/collector/collectors";
}
