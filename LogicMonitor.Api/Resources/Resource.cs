namespace LogicMonitor.Api.Resources;

/// <summary>
///    Resource (previously Device or Host, but now a more generic term)
/// </summary>
[DataContract]
public class Resource : NamedItem, IHasCustomProperties, IPatchable
{
	/// <summary>
	/// The Auto Balanced Collector Group id. 0 means not monitored by ABCG
	/// </summary>
	[DataMember(Name = "autoBalancedCollectorGroupId")]
	public int AutoBalancedCollectorGroupId { get; set; }

	/// <summary>
	///    The alert disable status
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	///    The alert disable status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "alertingDisabledOn")]
	public object AlertingDisabledOn { get; set; } = new();

	/// <summary>
	///    The alert status
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	///    The alert status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	///    Whether the ancestorHasDisabledLogicModule
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "ancestorHasDisabledLogicModule")]
	public bool AncestorHasDisabledLogicModule { get; set; }

	/// <summary>
	/// The auto properties
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "autoProperties")]
	public List<EntityProperty> AutoProperties { get; set; } = [];

	/// <summary>
	///    The time that the auto-properties were assigned in seconds since the Epoch
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "autoPropsAssignedOn")]
	public long? AutoPropertiesAssignedOnSeconds { get; set; }

	/// <summary>
	///    The time that the auto-properties were updated
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "autoPropsUpdatedOn")]
	public long? AutoPropertiesUpdatedOnSeconds { get; set; }

	/// <summary>
	///    The device AWS status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "awsState")]
	public AwsState AwsState { get; set; }

	/// <summary>
	/// The azure instance state (if applicable): 1 indicates that the instance is running, 2 indicates that the instance is stopped and 3 the instance is terminated.
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "azureState")]
	public AzureState AzureState { get; set; }

	/// <summary>
	/// The gcp instance state (if applicable): 1 indicates that the instance is running, 2 indicates that the instance is stopped and 3 the instance is terminated.
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "gcpState")]
	public GcpState GcpState { get; set; }

	/// <summary>
	///    Whether the device can use remote session
	/// </summary>
	[DataMember(Name = "canUseRemoteSession")]
	public bool CanUseRemoteSession { get; set; }

	/// <summary>
	///    The Collector description
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; } = string.Empty;

	/// <summary>
	///    Whether the device contains multi-value
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "containsMultiValue")]
	public bool ContainsMultiValue { get; set; }

	/// <summary>
	///    When the device was created
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "createdOn")]
	public long? CreatedOnSeconds { get; set; }

	/// <summary>
	/// The id of the collector currently monitoring the device and discovering instances
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "currentCollectorId")]
	public int CurrentCollectorId { get; set; }

	/// <summary>
	/// The id of the Log collector currently collecting logs.
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "currentLogCollectorId")]
	public int CurrentLogCollectorId { get; set; }

	/// <summary>
	///    Custom properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; } = [];

	/// <summary>
	///    The deleted time in ms
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "deletedTimeInMs")]
	public long DeletedTimeInMs { get; set; }

	/// <summary>
	///    The deleted time in ms
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use DeletedTimeinMs instead", true)]
	public long DeletedTimeinMs => DeletedTimeInMs;

	/// <summary>
	///    The device type
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "deviceType")]
	public ResourceType DeviceType { get; set; }

	/// <summary>
	///    Whether alerting is effectively enabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool IsAlertingDisabled { get; set; }

	/// <summary>
	/// The display name of the device
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	///    Whether alerting is effectively enabled
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "effectiveAlertEnabled")]
	public bool EffectiveAlertEnabled { get; set; }

	/// <summary>
	///    Whether Netflow is enabled
	/// </summary>
	[DataMember(Name = "enableNetflow")]
	public bool EnableNetflow { get; set; }

	/// <summary>
	///    Whether the device has an active instance
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "hasActiveInstance")]
	public bool HasActiveInstance { get; set; }

	/// <summary>
	///    Whether the device has a disabled sub-resource
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "hasDisabledSubResource")]
	public bool HasDisabledSubResource { get; set; }

	/// <summary>
	///    Whether the device has more (?)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "hasMore")]
	public bool HasMore { get; set; }

	/// <summary>
	/// The ResourceGroup ids as a string
	/// </summary>
	[DataMember(Name = "hostGroupIds")]
	public string ResourceGroupIdsString { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupId", true)]
	public string DeviceGroupIdsString => ResourceGroupIdsString;

	/// <summary>
	///    The device status
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "hostStatus")]
	public Level DeviceStatus { get; set; }

	/// <summary>
	///    Inherited properties
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "inheritedProperties")]
	public List<EntityProperty> InheritedProperties { get; set; } = [];

	/// <summary>
	///    The instances
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "instance")]
	public List<ResourceDataSourceInstanceSummary> Instances { get; set; } = [];

	/// <summary>
	/// Indicates whether Preferred Log Collector is configured  (true) or not (false) for the device
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "isPreferredLogCollectorConfigured")]
	public bool IsPreferredLogCollectorConfigured { get; set; }

	/// <summary>
	///    The last time that raw data was received for the device
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "lastDataTime")]
	public long? LastDataTimeSeconds { get; set; }

	/// <summary>
	///    The last time that raw data was received for the device
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "lastRawDataTime")]
	public long? LastRawDataTimeSeconds { get; set; }

	/// <summary>
	///    The device's configured URL
	/// </summary>
	[DataMember(Name = "link")]
	public string Link { get; set; } = string.Empty;

	/// <summary>
	/// The description/name of the log collector for this device
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "logCollectorDescription")]
	public string LogCollectorDescription { get; set; } = string.Empty;

	/// <summary>
	/// The id of the Collector Group associated with the device\u0027s log collection
	/// </summary>
	[DataMember(Name = "logCollectorGroupId")]
	public int LogCollectorGroupId { get; set; }

	/// <summary>
	/// The name of the Collector Group associated with the device\u0027s.
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "logCollectorGroupName")]
	public string LogCollectorGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The Id of the netflow collector associated with the device
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "logCollectorId")]
	public int LogCollectorId { get; set; }

	/// <summary>
	///    Whether the device has a disabled sub-resource
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "manualDiscoveryFlags")]
	public ManualDiscoveryFlags ManualDiscoveryFlags { get; set; } = new();

	/// <summary>
	///    The Netflow Collector Id
	/// </summary>
	[DataMember(Name = "netflowCollectorId")]
	public int NetflowCollectorId { get; set; }

	/// <summary>
	///    The Netflow Collector description
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "netflowCollectorDescription")]
	public string NetflowCollectorDescription { get; set; } = string.Empty;

	/// <summary>
	///    The Netflow Collector Group Id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "netflowCollectorGroupId")]
	public int NetflowCollectorGroupId { get; set; }

	/// <summary>
	///    The Netflow Collector Group name
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "netflowCollectorGroupName")]
	public string NetflowCollectorGroupName { get; set; } = string.Empty;

	/// <summary>
	/// Op
	/// </summary>
	[DataMember(Name = "op")]
	public string Op { get; set; } = string.Empty;

	/// <summary>
	///    The preferred Collector Id
	/// </summary>
	[DataMember(Name = "preferredCollectorId")]
	public int PreferredCollectorId { get; set; }

	/// <summary>
	///    The preferred CollectorGroup Id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "preferredCollectorGroupId")]
	public int PreferredCollectorGroupId { get; set; }

	/// <summary>
	///    The preferred CollectorGroup Id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "preferredCollectorGroupName")]
	public string PreferredCollectorGroupName { get; set; } = string.Empty;

	/// <summary>
	///    The ID of the related device
	/// </summary>
	[DataMember(Name = "relatedDeviceId")]
	public int RelatedDeviceId { get; set; }

	/// <summary>
	///    The AWS test result
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "awsTestResult")]
	public string AwsTestResult { get; set; } = string.Empty;

	/// <summary>
	///    The AWS test result code
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "awsTestResultCode")]
	public int AwsTestResultCode { get; set; }

	/// <summary>
	///    The Azure test result
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "azureTestResult")]
	public string AzureTestResult { get; set; } = string.Empty;

	/// <summary>
	///    The Azure test result code
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "azureTestResultCode")]
	public int AzureTestResultCode { get; set; }

	/// <summary>
	///    The GCP test result
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "gcpTestResult")]
	public string GcpTestResult { get; set; } = string.Empty;

	/// <summary>
	///    The GCP test result code
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "gcpTestResultCode")]
	public int GcpTestResultCode { get; set; }

	/// <summary>
	///    The SaaS test result
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "saasTestResult")]
	public string SaasTestResult { get; set; } = string.Empty;

	/// <summary>
	///    The SaaS test result code
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "saasTestResultCode")]
	public int SaasTestResultCode { get; set; }

	/// <summary>
	///    The auto visual result
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "autoVisualResult")]
	public string AutoVisualResult { get; set; } = string.Empty;

	/// <summary>
	///    The Scan config ID
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "scanConfigId")]
	public int ScanConfigId { get; set; }

	/// <summary>
	///    Whether the device is currently in SDT
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	///    The device's system properties
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "systemProperties")]
	public List<EntityProperty> SystemProperties { get; set; } = [];

	/// <summary>
	///    The time in Ms before the device will be deleted
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "toDeleteTimeInMs")]
	public long ToDeleteTimeInMs { get; set; }

	/// <summary>
	///    Uptime in seconds
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "upTimeInSeconds")]
	public int UptimeInSeconds { get; set; }

	/// <summary>
	///    The last time that the device was updated in seconds since the Epoch
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "updatedOn")]
	public long? UpdatedOnSeconds { get; set; }

	/// <summary>
	///    LoadBalance CollectorGroupId
	/// </summary>
	[DataMember(Name = "loadBalanceCollectorGroupId")]
	public int LoadBalanceCollectorGroupId { get; set; }

	/// <summary>
	/// Any non-system properties (aside from system.categories) defined for this device
	/// </summary>
	[DataMember(Name = "resourceIds")]
	public List<int>? ResourceIds { get; set; }

	/// <summary>
	/// The list of ids of the collectors currently monitoring the resource and discovering instances
	/// </summary>
	[DataMember(Name = "syntheticsCollectorIds")]
	public List<int>? SyntheticsCollectorIds { get; set; }

	/// <summary>
	/// The role privilege operation(s) for this device that are granted to the user who made the API request
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "rolePrivileges")]
	public List<RolePrivilege> RolePrivileges { get; set; } = [];

	/// <summary>
	///    User Permission
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <summary>
	///    The UTC DateTime that the auto-properties were assigned
	/// </summary>
	[IgnoreDataMember]
	public DateTime? AutoPropertiesAssignedOnUtc => AutoPropertiesAssignedOnSeconds?.ToNullableDateTimeUtc();

	/// <summary>
	///    The UTC DateTime that the auto-properties were updated
	/// </summary>
	[IgnoreDataMember]
	public DateTime? AutoPropertiesUpdatedOnUtc => AutoPropertiesUpdatedOnSeconds?.ToNullableDateTimeUtc();

	/// <summary>
	///    The UTC DateTime that the device was created in the system
	/// </summary>
	[IgnoreDataMember]
	public DateTime? CreatedOnUtc => CreatedOnSeconds?.ToNullableDateTimeUtc();

	/// <summary>
	///    The UTC DateTime that raw data was last received
	/// </summary>
	[IgnoreDataMember]
	public DateTime? LastRawDataUtc => LastRawDataTimeSeconds?.ToNullableDateTimeUtc();

	/// <summary>
	///    The UTC DateTime that data was last received
	/// </summary>
	[IgnoreDataMember]
	public DateTime? LastDataUtc => LastDataTimeSeconds?.ToNullableDateTimeUtc();

	/// <summary>
	///    The UTC DateTime that the device was updated in the system
	/// </summary>
	[IgnoreDataMember]
	public DateTime? UpdatedOnUtc => UpdatedOnSeconds?.ToNullableDateTimeUtc();

	/// <inheritdoc />
	public string Endpoint() => "device/devices";

	/// <inheritdoc />
	public override string ToString() => $"{Id} : {(!string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : Name)}";
}
