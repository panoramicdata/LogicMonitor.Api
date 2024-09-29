namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A device data source SDT
/// </summary>
[DataContract]
public class ResourceDataSourceAlertSdt : AlertSdt
{
	/// <summary>
	/// The DataSource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The DataSource ID
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The Resource DataSource ID
	/// </summary>
	[DataMember(Name = "hostDataSourceId")]
	public int ResourceDataSourceId { get; set; }

	/// <summary>
	/// The Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDisplayName instead", true)]
	public string DeviceDisplayName => ResourceDisplayName;

	/// <summary>
	/// The Resource ID
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
