namespace LogicMonitor.Api.Devices;

/// <summary>
/// A device group data source
/// </summary>
[DataContract]
public class DeviceGroupDataSource
{
	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The DataSource Display Name
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; }

	/// <summary>
	/// The DataSource unique name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; }

	/// <summary>
	/// The DataSourceGroup Name
	/// </summary>
	[DataMember(Name = "dataSourceGroupName")]
	public string DataSourceGroupName { get; set; }

	/// <summary>
	/// The DeviceGroup Id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	/// Whether to stop monitoring
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// Whether to disable alerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// DataSourceType
	/// </summary>
	[DataMember(Name = "dataSourceType")]
	public string DataSourceType { get; set; }
}
