namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DeviceDataSourceInstance
/// </summary>
[SantabaReadOnly]
[DataContract]
public class DeviceDataSourceInstance : NamedItem, IHasCustomProperties
{
	/// <summary>
	///     The Alerting disabled on
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public object AlertingDisabledOn { get; set; }
	// LogicMonitor sometimes returns a string, so the following cannot be used
	// public AlertingDisabledOn AlertingDisabledOn { get;set; }

	/// <summary>
	/// AlertDisableStatus
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	/// Alert Status
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// Alert Status Priority
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	/// Custom properties
	/// </summary>
	[DataMember(Name = "customProperties")]
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
	/// Disable Alerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// Display Name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; }

	/// <summary>
	/// The Group Id
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
	/// groupsDisabledThisSource
	/// </summary>
	[DataMember(Name = "isUNCInstance")]
	public bool? IsUncInstance { get; set; }

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
