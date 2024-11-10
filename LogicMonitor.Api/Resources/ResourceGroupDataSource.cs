namespace LogicMonitor.Api.Resources;

/// <summary>
/// A ResourceGroup data source
/// </summary>
[DataContract]
public class ResourceGroupDataSource
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
	public string DataSourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The DataSource unique name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The DataSourceGroup Name
	/// </summary>
	[DataMember(Name = "dataSourceGroupName")]
	public string DataSourceGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int ResourceGroupId { get; set; }

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
	public string DataSourceType { get; set; } = string.Empty;
}
