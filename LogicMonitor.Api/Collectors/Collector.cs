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
	public string AckComment { get; set; } = string.Empty;

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
	public string AckedBy { get; set; } = string.Empty;

	/// <summary>
	///     Acked on local (human-readable acknowledgement DateTime as a string)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "ackedOnLocal")]
	public string AckedOnLocalString { get; set; } = string.Empty;

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
	public string Architecture { get; set; } = string.Empty;

	/// <summary>
	///     The automatic upgrade information
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "automaticUpgradeInfo")]
	public AutomaticUpgradeInfo AutomaticUpgradeInfo { get; set; } = new AutomaticUpgradeAutomaticUpgradeInfo();

	/// <summary>
	///     This collector's backup collector's Id
	/// </summary>
	[DataMember(Name = "backupAgentId")]
	public int BackupCollectorId { get; set; }

	/// <summary>
	///     This collector's bearer token
	/// </summary>
	[DataMember(Name = "bearerToken")]
	public string BearerToken { get; set; } = string.Empty;

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
	public string CanDowngradeReason { get; set; } = string.Empty;

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
	public string CollectorConfiguration { get; set; } = string.Empty;

	/// <summary>
	///     Collector configuration
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "collectorType")]
	public CollectorType Type { get; set; }

	/// <summary>
	/// This is key value pairs of collector config properties
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "agentConfFields")]
	public string ConfigurationFields { get; set; } = string.Empty;

	/// <summary>
	///     The copy URL
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "copyUrl")]
	public string CopyUrl { get; set; } = string.Empty;

	/// <summary>
	///     The download URL
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "downloadUrl")]
	public string DownloadUrl { get; set; } = string.Empty;

	/// <summary>
	///     The collector device id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "collectorDeviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	///     The encoded config data
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "encodedConfigData")]
	public string EncodedConfigData { get; set; } = string.Empty;

	/// <summary>
	///     The format
	/// </summary>
	[DataMember(Name = "format")]
	public string Format { get; set; } = string.Empty;

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
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	///     The Agent configuration
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "config")]
	public string Configuration { get; set; } = string.Empty;

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
	public string CreatedOnLocalString { get; set; } = string.Empty;

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
	public string Credential { get; set; } = string.Empty;

	/// <summary>
	///     The other credential
	/// </summary>
	[DataMember(Name = "credential2")]
	public string Credential2 { get; set; } = string.Empty;

	/// <summary>
	///     Custom properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; } = [];

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
	///     Enable fail back
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
	public EscalationChain EscalationChain { get; set; } = new();

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
	public string HostName { get; set; } = string.Empty;

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
	///     Whether the collector is encoded
	/// </summary>
	[DataMember(Name = "isEncoded")]
	public bool IsEncoded { get; set; }

	/// <summary>
	///     Whether LM Logs is enabled
	/// </summary>
	[DataMember(Name = "enableLmLogs")]
	public bool IsLmLogsEnabled { get; set; }

	/// <summary>
	/// Whether LM Logs is enabled for syslog
	/// </summary>
	[DataMember(Name = "isLmlogsSyslogEnabled")]
	public bool IsLmLogsSyslogEnabled { get; set; }

	/// <summary>
	///     Last sent notification on local (human-readable acknowledgement DateTime as a string)
	/// </summary>
	[DataMember(Name = "lastSentNotificationOnLocal")]
	public string LastSentNotificationOnLocal { get; set; } = string.Empty;

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
	public CollectorNextUpgradeInfo NextUpgradeInfo { get; set; } = new();

	/// <summary>
	///     The one-time downgrade information
	/// </summary>
	[DataMember(Name = "onetimeDowngradeInfo")]
	public string OneTimeDowngradeInfo { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use OneTimeDowngradeInfo", true)]
	public string OnetimeDowngradeInfo => OneTimeDowngradeInfo;

	/// <summary>
	///     One time upgrade info
	/// </summary>
	[DataMember(Name = "onetimeUpgradeInfo")]
	public OneTimeUpgradeInfo OneTimeUpgradeInfo { get; set; } = new();

	/// <summary>
	///     The OTEL Id
	/// </summary>
	[DataMember(Name = "otelId")]
	public int? OtelId { get; set; }

	/// <summary>
	///     The OTEL version
	/// </summary>
	[DataMember(Name = "otelVersion")]
	public string OtelVersion { get; set; } = string.Empty;

	/// <summary>
	///     Platform
	/// </summary>
	[DataMember(Name = "platform")]
	public string Platform { get; set; } = string.Empty;

	/// <summary>
	///     The predefined configuration
	/// </summary>
	[DataMember(Name = "predefinedConfig")]
	public object PredefinedConfiguration { get; set; } = new();

	/// <summary>
	///     The previous version
	/// </summary>
	[DataMember(Name = "previousVersion")]
	public int PreviousVersion { get; set; }

	/// <summary>
	/// The Proxy\u0027s configuration
	/// </summary>
	[DataMember(Name = "sbproxyConf")]
	public string ProxyConfiguration { get; set; } = string.Empty;

	/// <summary>
	///     Resend interval
	/// </summary>
	[DataMember(Name = "resendIval")]
	public int ResendIntervalSeconds { get; set; }

	/// <summary>
	///     Website configuration
	/// </summary>
	[DataMember(Name = "websiteConf")]
	public string WebsiteConfiguration { get; set; } = string.Empty;

	/// <summary>
	///     Number of Website
	/// </summary>
	[DataMember(Name = "numberOfWebsites")]
	public int WebsiteCount { get; set; }

	/// <summary>
	///     Size
	/// </summary>
	[DataMember(Name = "collectorSize")]
	public string Size { get; set; } = string.Empty;

	/// <summary>
	///     The specified Collector ResourceGroup Id
	/// </summary>
	[DataMember(Name = "specifiedCollectorDeviceGroupId")]
	public int SpecifiedCollectorResourceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use SpecifiedCollectorResourceGroupId", true)]
	public int SpecifiedCollectorDeviceGroupId => SpecifiedCollectorResourceGroupId;

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
	/// Whether the collector can monitor Synthetic devices (Selenium grid property must be defined)
	/// </summary>
	[DataMember(Name = "syntheticsEnabled")]
	public bool IsSyntheticsEnables { get; set; }

	/// <summary>
	///     Updated on local (human-readable acknowledgement DateTime as a string)
	/// </summary>
	[DataMember(Name = "updatedOnLocal")]
	public string UpdatedOnLocalString { get; set; } = string.Empty;

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
	public string UserChangeOnLocal { get; set; } = string.Empty;

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
	public string WatchdogConfiguration { get; set; } = string.Empty;

	/// <summary>
	///     Updated on local (human-readable acknowledgement DateTime as a string)
	/// </summary>
	[DataMember(Name = "watchdogUpdatedOnLocal")]
	public string WatchdogUpdatedOnLocal { get; set; } = string.Empty;

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
	public string WrapperConfiguration { get; set; } = string.Empty;

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
