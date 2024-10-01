namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Resource Noc Widget Noc Item
/// </summary>
[DataContract]
public class ResourceNocWidgetItem
{
	/// <summary>
	///     The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupFullPath instead", true)]
	public string DeviceGroupFullPath => ResourceGroupFullPath;

	/// <summary>
	/// The Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The DataSource display name
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The DataSource Instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The DataPoint name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// The group by
	/// </summary>
	[DataMember(Name = "groupBy")]
	public string GroupBy { get; set; } = string.Empty;

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}
