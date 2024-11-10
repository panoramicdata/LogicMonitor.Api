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
	/// The Resource Cluster Alert Def ID
	/// </summary>
	[DataMember(Name = "deviceClusterAlertDefId")]
	public int ResourceClusterAlertDefId { get; set; }

	/// <summary>
	/// The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// The ResourceGroup ID
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int ResourceGroupId { get; set; }
}
