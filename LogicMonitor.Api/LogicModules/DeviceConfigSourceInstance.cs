namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DeviceDataSourceInstance
/// </summary>
[DataContract]
public class DeviceConfigSourceInstance : NamedItem, IHasCustomProperties
{
	/// <summary>
	/// Alert Disable Status
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	///     The Alerting disabled on
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public AlertingDisabledOn AlertingDisabledOn { get; set; }

	/// <summary>
	/// Alert Status
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// Alert Status priority
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	/// The auto properties
	/// </summary>
	[DataMember(Name = "autoProperties")]
	public List<Property> AutoProperties { get; set; }

	/// <summary>
	/// The custom properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<Property> CustomProperties { get; set; }

	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The DataSource type
	/// </summary>
	[DataMember(Name = "dataSourceType")]
	public string DataSourceType { get; set; }

	/// <summary>
	/// The DeviceConfigSourceId
	/// </summary>
	[DataMember(Name = "deviceConfigSourceId")]
	public int DeviceConfigSourceId { get; set; }

	/// <summary>
	/// The DeviceDataSource Id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int DeviceDataSourceId { get; set; }

	/// <summary>
	/// The Device Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int? DeviceId { get; set; }

	/// <summary>
	/// The device DisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The disableAlerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The DisplayName
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; }

	/// <summary>
	/// The group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The group name
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; }

	/// <summary>
	/// Whether monitoring is disabled at the group level
	/// </summary>
	[DataMember(Name = "groupsDisabledThisSource")]
	public List<DisabledGroup> GroupsDisabledThisSource { get; set; }

	/// <summary>
	/// groupsDisabledThisSource
	/// </summary>
	[DataMember(Name = "isUNCInstance")]
	public bool? IsUncInstance { get; set; }

	/// <summary>
	/// The last collected time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastCollectedTime")]
	public long? LastCollectedTimeSeconds { get; set; }

	/// <summary>
	/// The last collected time
	/// </summary>
	[IgnoreDataMember]
	public DateTime? LastCollectedTimeUtc => LastCollectedTimeSeconds?.ToDateTimeUtc();

	/// <summary>
	/// The last updated time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastUpdatedTime")]
	public long? LastUpdatedTimeSeconds { get; set; }

	/// <summary>
	/// The last updated time
	/// </summary>
	[IgnoreDataMember]
	public DateTime? LastUpdatedTimeUtc => LastUpdatedTimeSeconds?.ToDateTimeUtc();

	/// <summary>
	/// The lockDescription
	/// </summary>
	[DataMember(Name = "lockDescription")]
	public bool LockDescription { get; set; }

	/// <summary>
	/// The lockDescription
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// SDT Status
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	/// SDT at
	/// </summary>
	[DataMember(Name = "sdtAt")]
	public string SdtAt { get; set; }

	/// <summary>
	/// The system properties
	/// </summary>
	[DataMember(Name = "systemProperties")]
	public List<Property> SystemProperties { get; set; }

	/// <summary>
	/// WildValue
	/// </summary>
	[DataMember(Name = "wildValue")]
	public string WildValue { get; set; }

	/// <summary>
	/// WildValue2
	/// </summary>
	[DataMember(Name = "wildValue2")]
	public string WildValue2 { get; set; }
}
