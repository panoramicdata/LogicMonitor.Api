namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Resource Cluster Alert Def SDT
/// </summary>
[DataContract]
public class ResourceClusterAlertDefSdt : AlertSdt
{
	/// <summary>
	/// The DataSource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The Device Cluster Alert Def ID
	/// </summary>
	[DataMember(Name = "deviceClusterAlertDefId")]
	public int ResourceClusterAlertDefId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceClusterAlertDefId", true)]
	public int DeviceClusterAlertDefId => ResourceClusterAlertDefId;

	/// <summary>
	/// The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[Obsolete("Use ResourceGroupFullPath instead", true)]
	[JsonIgnore, IgnoreDataMember]
	public string DeviceGroupFullPath => ResourceGroupFullPath;

	/// <summary>
	/// The ResourceGroup ID
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupId", true)]
	public int DeviceGroupId => ResourceGroupId;
}
