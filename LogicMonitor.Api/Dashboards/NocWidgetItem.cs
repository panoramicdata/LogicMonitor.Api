namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A NOC widget item
/// </summary>
[DataContract]
public class NocWidgetItem
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
	/// The deviceDisplayName
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
	/// The Website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; } = string.Empty;

	/// <summary>
	/// The Website group name
	/// </summary>
	[DataMember(Name = "websiteGroupName")]
	public string WebsiteGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The dataSourceDisplayName
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The instanceName
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string? DataPointName { get; set; }

	/// <summary>
	/// The groupBy
	/// </summary>
	[DataMember(Name = "groupBy")]
	public string GroupBy { get; set; } = string.Empty;

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;
}
