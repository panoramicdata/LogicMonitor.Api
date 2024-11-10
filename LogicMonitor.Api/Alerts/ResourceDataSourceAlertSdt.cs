namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A ResourceDataSource SDT
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
	/// The Resource DataSource ID again for some reason
	/// </summary>
	[DataMember(Name = "hdsId")]
	public int ResourceDataSourceId2 { get; set; }

	/// <summary>
	/// The Resource display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Resource ID
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }
}
