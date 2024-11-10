namespace LogicMonitor.Api.LogicModules;

/// <summary>
///    A Resource DataSource
/// </summary>
public class ResourceDataSource : IdentifiedItem
{
	/// <summary>
	///    Alert Disable Status
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	///    Alert Status
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	///    Alert Status priority
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	///    The time it was created in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "assignedOn")]
	public long AssignedOnSeconds { get; set; }

	/// <summary>
	///    The Created on DateTime (UTC)
	/// </summary>
	[IgnoreDataMember]
	public DateTime AssignedOnUtc => AssignedOnSeconds.ToDateTimeUtc();

	/// <summary>
	///     The Alerting disabled on
	/// </summary>
	[DataMember(Name = "alertingDisabledOn")]
	public object AlertingDisabledOn { get; set; } = new();

	/// <summary>
	///    The collection method
	/// </summary>
	[DataMember(Name = "collectMethod")]
	public CollectionMethod CollectionMethod { get; set; }

	/// <summary>
	///    The time it was created in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "createdOn")]
	public long CreatedOnSeconds { get; set; }

	/// <summary>
	///    The Created on DateTime (UTC)
	/// </summary>
	[IgnoreDataMember]
	public DateTime CreatedOnUtc => CreatedOnSeconds.ToDateTimeUtc();

	/// <summary>
	///    The data source type
	/// </summary>
	[DataMember(Name = "dataSourceType")]
	public string DataSourceType { get; set; } = string.Empty;

	/// <summary>
	///    The time it was updated in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "updatedOn")]
	public long UpdatedOnSeconds { get; set; }

	/// <summary>
	///    The Updated on DateTime (UTC)
	/// </summary>
	[IgnoreDataMember]
	public DateTime UpdatedOnUtc => UpdatedOnSeconds.ToDateTimeUtc();

	/// <summary>
	///    DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	///    DataSource Name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	///    DataSource Description
	/// </summary>
	[DataMember(Name = "dataSourceDescription")]
	public string DataSourceDescription { get; set; } = string.Empty;

	/// <summary>
	///    DataSource DisplayName
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///    Graph summary info.  Note that not all fields are completed
	/// </summary>
	[DataMember(Name = "graphs")]
	public List<DataSourceGraph> DataSourceGraphs { get; set; } = [];

	/// <summary>
	///    The Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	///    The device name
	/// </summary>
	[DataMember(Name = "deviceName")]
	public string ResourceName { get; set; } = string.Empty;

	/// <summary>
	///    The device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///    The group name
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	///    Disabled InstanceGroups
	/// </summary>
	[DataMember(Name = "groupsDisabledThisSource")]
	public object DisabledInstanceGroups { get; set; } = new();

	/// <summary>
	///    Instance autogroup enabled
	/// </summary>
	[DataMember(Name = "instanceAutoGroupEnabled")]
	public bool InstanceAutoGroupEnabled { get; set; }

	/// <summary>
	///    The instance number
	/// </summary>
	[DataMember(Name = "instanceNumber")]
	public int InstanceCount { get; set; }

	/// <summary>
	///    The monitoring instance number
	/// </summary>
	[DataMember(Name = "monitoringInstanceNumber")]
	public int MonitoringInstanceCount { get; set; }

	/// <summary>
	///    The time it was created in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "nextAutoDiscoveryOn")]
	public long NextAutoDiscoveryOnSeconds { get; set; }

	/// <summary>
	///    The Created on DateTime (UTC)
	/// </summary>
	[IgnoreDataMember]
	public DateTime NextAutoDiscoveryOnUtc => NextAutoDiscoveryOnSeconds.ToDateTimeUtc();

	/// <summary>
	///    Graph summary info.  Note that not all fields are completed
	/// </summary>
	[DataMember(Name = "overviewGraphs")]
	public List<DataSourceGraph> OverviewGraphs { get; set; } = [];

	/// <summary>
	///    AutoDiscovery is enabled
	/// </summary>
	[DataMember(Name = "autoDiscovery")]
	public bool IsAutoDiscoveryEnabled { get; set; }

	/// <summary>
	///    Is Monitoring disabled
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool IsMonitoringDisabled { get; set; }

	/// <summary>
	///    Is multiple
	/// </summary>
	[DataMember(Name = "isMultiple")]
	public bool IsMultiple { get; set; }

	/// <summary>
	///    SDT Status
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	///    The status
	/// </summary>
	[DataMember(Name = "status")]
	public int Status { get; set; }

	/// <summary>
	///    SDT at
	/// </summary>
	[DataMember(Name = "sdtAt")]
	public string SdtAt { get; set; } = string.Empty;

	/// <inheritdoc />
	public override string ToString() => $"{Id}: {DataSourceName} ({DataSourceId}) on device {ResourceDisplayName} ({ResourceId})";
}
