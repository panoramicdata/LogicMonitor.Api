namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Device Noc Widget Noc Item
/// </summary>
[DataContract]
public class DeviceNocWidgetItem
{
	/// <summary>
	/// The DeviceGroup Full Path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string DeviceGroupFullPath { get; set; }

	/// <summary>
	/// The Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// The DataSource display name
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; }

	/// <summary>
	/// The DataSource Instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string DataSourceInstanceName { get; set; }

	/// <summary>
	/// The DataPoint name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; }

	/// <summary>
	/// The group by
	/// </summary>
	[DataMember(Name = "groupBy")]
	public string GroupBy { get; set; }

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }
}
