namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A NOC widget item
/// </summary>
[DataContract]
public class NocWidgetItem
{
	/// <summary>
	/// The deviceGroupFullPath
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string DeviceGroupFullPath { get; set; }

	/// <summary>
	/// The deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The Website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; }

	/// <summary>
	/// The Website group name
	/// </summary>
	[DataMember(Name = "websiteGroupName")]
	public string WebsiteGroupName { get; set; }

	/// <summary>
	/// The dataSourceDisplayName
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; }

	/// <summary>
	/// The instanceName
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; }

	/// <summary>
	/// The dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; }

	/// <summary>
	/// The groupBy
	/// </summary>
	[DataMember(Name = "groupBy")]
	public string GroupBy { get; set; }

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }
}
