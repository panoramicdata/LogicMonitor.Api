using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A collection of devices to which the DataSource applies
/// </summary>
[DataContract]
public class DataSourceAppliesToCollection : IdentifiedItem
{
	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The dataSourceDisplayName
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; }

	/// <summary>
	/// The dataSourceGroupName
	/// </summary>
	[DataMember(Name = "dataSourceGroupName")]
	public string DataSourceGroupName { get; set; }

	/// <summary>
	/// The deviceGroupId
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	/// The stopMonitoringstopMonitoring
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// The disableAlerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceDeviceList")]
	public List<DataSourceDevice> DataSourceDevices { get; set; }

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{DataSourceDisplayName} ({DataSourceId}) with {DataSourceDevices?.Count.ToString() ?? "0"} devices";
}
