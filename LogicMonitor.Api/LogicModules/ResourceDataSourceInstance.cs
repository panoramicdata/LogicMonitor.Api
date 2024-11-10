namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A Resource DataSource Instance
/// </summary>
[SantabaReadOnly]
[DataContract]
public class ResourceDataSourceInstance : NamedItem, IHasCustomProperties
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
	public object AlertDisabledOn { get; set; } = new();

	/// <summary>
	/// The alert status priority
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	/// Whether or not UNC Monitoring enabled for device
	/// </summary>
	[DataMember(Name = "isUNCInstance")]
	public bool IsUncInstance { get; set; }

	/// <summary>
	/// Whether or not monitoring is disabled for the instance
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// The id of the unique Resource-datasource the instance is associated with
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int ResourceDataSourceId { get; set; }

	/// <summary>
	/// The instance alias. This is the descriptive name of the instance, and should be unique for the device/datasource combination
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Any instance level properties assigned to the instance
	/// Collector Id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int? CollectorId { get; set; }

	/// <summary>
	/// Custom properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; } = [];

	/// <summary>
	/// Any instance level system properties assigned to the instance
	/// </summary>
	[DataMember(Name = "systemProperties")]
	public List<EntityProperty> SystemProperties { get; set; } = [];

	/// <summary>
	/// Any instance level auto properties assigned to the instance
	/// </summary>
	[DataMember(Name = "autoProperties")]
	public List<EntityProperty> AutoProperties { get; set; } = [];

	/// <summary>
	/// The id of the datasource definition that the instance represents
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int? DataSourceId { get; set; }

	/// <summary>
	/// The DataSource Name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The type of LogicModule, e.g. DS (datasource)
	/// </summary>
	[DataMember(Name = "dataSourceType")]
	public string DataSourceType { get; set; } = string.Empty;

	/// <summary>
	/// The display name of the Resource the DataSource Instance is associated with
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The id of the Resource the DataSource Instance is associated with
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// Whether or not alerting is disabled for the instance
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The id of the instance group associated with the datasource instance
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The name of the instance group associated with the datasource instance
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	/// groupsDisabledThisSource
	/// </summary>
	[DataMember(Name = "groupsDisabledThisSource")]
	public List<DisabledGroup> GroupsDisabledThisSource { get; set; } = [];

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
	/// Whether or not Active Discovery is enabled, and thus whether or not the instance description is editable
	/// </summary>
	[DataMember(Name = "lockDescription")]
	public bool LockDescription { get; set; }

	/// <summary>
	/// WildValue
	/// </summary>
	[DataMember(Name = "wildValue")]
	public string WildValue { get; set; } = string.Empty;

	/// <summary>
	/// Only used for two dimensional active discovery. When used, during Active Discovery runs, the token ##WILDVALUE## is replaces with the value of ALIAS and the token ##WILDVALUE2## is replaced with the value of the second part alias. This value must be unique for the device/datasource/WILDVALUE combination
	/// </summary>
	[DataMember(Name = "wildValue2")]
	public string WildValue2 { get; set; } = string.Empty;

	/// <summary>
	/// SDT status
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public string SdtStatus { get; set; } = string.Empty;

	/// <summary>
	/// SDT status
	/// </summary>
	[DataMember(Name = "sdtAt")]
	public string SdtAt { get; set; } = string.Empty;
}
