namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A device data source instance group SDT
/// </summary>
[DataContract]
public class ResourceDataSourceInstanceGroupAlertSdt : AlertSdt
{
	/// <summary>
	/// The DataSource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource DataSource ID
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int ResourceDataSourceId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDataSourceId instead", true)]
	public int DeviceDataSourceId => ResourceDataSourceId;

	/// <summary>
	/// The Resource DataSource Instance Group ID
	/// </summary>
	[DataMember(Name = "deviceDataSourceInstanceGroupId")]
	public int ResourceDataSourceInstanceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDataSourceInstanceGroupId instead", true)]
	public int DeviceDataSourceInstanceGroupId => ResourceDataSourceInstanceGroupId;

	/// <summary>
	/// The Device DataSource Instance Group name
	/// </summary>
	[DataMember(Name = "deviceDataSourceInstanceGroupName")]
	public string ResourceDataSourceInstanceGroupName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDataSourceInstanceGroupId instead", true)]
	public string DeviceDataSourceInstanceGroupName => ResourceDataSourceInstanceGroupName;

	/// <summary>
	/// The Resource display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDataSourceInstanceGroupId instead", true)]
	public string DeviceDisplayName => ResourceDisplayName;

	/// <summary>
	/// The Device ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }


	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceId instead", true)]
	public int DeviceId => ResourceId;
}
