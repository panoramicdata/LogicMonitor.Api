namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DeviceDataSourceInstance
/// </summary>
[SantabaReadOnly]
[DataContract]
public class DeviceDataSourceInstance : NamedItem, IHasCustomProperties
{
	/// <summary>
	/// The alert status
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// The alert disabled status
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	/// The alert disabled on
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public object? AlertDisabledOn { get; set; }

	/// <summary>
	/// The alert status priority
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	/// Whether or not UNC Monitoring enabled for device
	/// </summary>
	[DataMember(Name = "isUNCInstance", IsRequired = false)]
	public bool IsUncInstance { get; set; }

	/// <summary>
	/// Whether or not monitoring is disabled for the instance
	/// </summary>
	[DataMember(Name = "stopMonitoring", IsRequired = false)]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// The id of the unique device-datasource the instance is associated with
	/// </summary>
	[DataMember(Name = "deviceDataSourceId", IsRequired = false)]
	public int DeviceDataSourceId { get; set; }

	/// <summary>
	/// The instance alias. This is the descriptive name of the instance, and should be unique for the device/datasource combination
	/// </summary>
	[DataMember(Name = "displayName", IsRequired = true)]
	public string DisplayName { get; set; }

	/// <summary>
	/// Any instance level properties assigned to the instance
	/// Collector Id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int? CollectorId { get; set; }

	/// <summary>
	/// Custom properties
	/// </summary>
	[DataMember(Name = "customProperties", IsRequired = false)]
	public List<EntityProperty> CustomProperties { get; set; }

	/// <summary>
	/// Any instance level system properties assigned to the instance
	/// </summary>
	[DataMember(Name = "systemProperties", IsRequired = false)]
	public List<EntityProperty> SystemProperties { get; set; }

	/// <summary>
	/// Any instance level auto properties assigned to the instance
	/// </summary>
	[DataMember(Name = "autoProperties", IsRequired = false)]
	public List<EntityProperty> AutoProperties { get; set; }

	/// <summary>
	/// The id of the datasource definition that the instance represents
	/// </summary>
	[DataMember(Name = "dataSourceId", IsRequired = false)]
	public int? DataSourceId { get; set; }

	/// <summary>
	/// The DataSource Name
	/// </summary>
	[DataMember(Name = "dataSourceName", IsRequired = false)]
	public string DataSourceName { get; set; }

	/// <summary>
	/// The type of LogicModule, e.g. DS (datasource)
	/// </summary>
	[DataMember(Name = "dataSourceType", IsRequired = false)]
	public string DataSourceType { get; set; }

	/// <summary>
	/// The display name of the device the datasource instance is associated with
	/// </summary>
	[DataMember(Name = "deviceDisplayName", IsRequired = false)]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The id of the device the datasource instance is associated with
	/// </summary>
	[DataMember(Name = "deviceId", IsRequired = false)]
	public int? DeviceId { get; set; }

	/// <summary>
	/// Whether or not alerting is disabled for the instance
	/// </summary>
	[DataMember(Name = "disableAlerting", IsRequired = false)]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The id of the instance group associated with the datasource instance
	/// </summary>
	[DataMember(Name = "groupId", IsRequired = false)]
	public int GroupId { get; set; }

	/// <summary>
	/// The name of the instance group associated with the datasource instance
	/// </summary>
	[DataMember(Name = "groupName", IsRequired = false)]
	public string GroupName { get; set; }

	/// <summary>
	/// groupsDisabledThisSource
	/// </summary>
	[DataMember(Name = "groupsDisabledThisSource", IsRequired = false)]
	public List<DisabledGroup> GroupsDisabledThisSource { get; set; }

	/// <summary>
	/// The last collected time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastCollectedTime", IsRequired = false)]
	public long LastCollectedTimeSeconds { get; set; }

	/// <summary>
	/// The last updated time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastUpdatedTime", IsRequired = false)]
	public long LastUpdatedTimeSeconds { get; set; }

	/// <summary>
	/// Whether or not Active Discovery is enabled, and thus whether or not the instance description is editable
	/// </summary>
	[DataMember(Name = "lockDescription", IsRequired = false)]
	public bool LockDescription { get; set; }

	/// <summary>
	/// WildValue
	/// </summary>
	[DataMember(Name = "wildValue", IsRequired = false)]
	public string? WildValue { get; set; }

	/// <summary>
	/// Only used for two dimensional active discovery. When used, during Active Discovery runs, the token ##WILDVALUE## is replaces with the value of ALIAS and the token ##WILDVALUE2## is replaced with the value of the second part alias. This value must be unique for the device/datasource/WILDVALUE combination
	/// </summary>
	[DataMember(Name = "wildValue2", IsRequired = false)]
	public string? WildValue2 { get; set; }

	/// <summary>
	/// SDT status
	/// </summary>
	[DataMember(Name = "sdtStatus", IsRequired = false)]
	public string? SdtStatus { get; set; }

	/// <summary>
	/// SDT status
	/// </summary>
	[DataMember(Name = "sdtAt", IsRequired = false)]
	public string? SdtAt { get; set; }
}
