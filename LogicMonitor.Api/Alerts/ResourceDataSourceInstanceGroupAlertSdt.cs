namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A ResourceDataSource instance group SDT
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
	/// The Resource DataSource Instance Group ID
	/// </summary>
	[DataMember(Name = "deviceDataSourceInstanceGroupId")]
	public int ResourceDataSourceInstanceGroupId { get; set; }

	/// <summary>
	/// The Resource DataSource Instance Group name
	/// </summary>
	[DataMember(Name = "deviceDataSourceInstanceGroupName")]
	public string ResourceDataSourceInstanceGroupName { get; set; } = string.Empty;

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
