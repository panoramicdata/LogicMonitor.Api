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
	public List<Property> CustomProperties { get; set; }

	/// <summary>
	/// System properties
	/// </summary>
	[DataMember(Name = "systemProperties")]
	public List<Property> SystemProperties { get; set; }

	/// <summary>
	/// System properties
	/// </summary>
	[DataMember(Name = "autoProperties")]
	public List<Property> AutoProperties { get; set; }

	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int? DataSourceId { get; set; }

	/// <summary>
	/// The DataSource Name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; }

	/// <summary>
	/// DataSourceType
	/// </summary>
	[DataMember(Name = "dataSourceType")]
	public string DataSourceType { get; set; }

	/// <summary>
	/// DeviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The Device Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int? DeviceId { get; set; }

	/// <summary>
	/// Disable Alerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The id of the instance group associated with the datasource instance
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The Group Name
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; }

	/// <summary>
	/// groupsDisabledThisSource
	/// </summary>
	[DataMember(Name = "groupsDisabledThisSource")]
	public List<DisabledGroup> GroupsDisabledThisSource { get; set; }



	/// <summary>
	/// The last collected time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastCollectedTime")]
	public long LastCollectedTimeSeconds { get; set; }

	/// <summary>
	/// The last updated time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastUpdatedTime")]
	public long LastUpdatedTimeSeconds { get; set; }

	/// <summary>
	/// The lockDescription
	/// </summary>
	[DataMember(Name = "lockDescription")]
	public bool LockDescription { get; set; }


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
	/// WildValue
	/// </summary>
	[DataMember(Name = "wildValue")]
	public string WildValue { get; set; }

	/// <summary>
	/// Only used for two dimensional active discovery. When used, during Active Discovery runs, the token ##WILDVALUE## is replaces with the value of ALIAS and the token ##WILDVALUE2## is replaced with the value of the second part alias. This value must be unique for the device/datasource/WILDVALUE combination
	/// </summary>
	[DataMember(Name = "wildValue2")]
	public string WildValue2 { get; set; }
}
