namespace LogicMonitor.Api.Devices;

/// <summary>
/// A device group event source
/// </summary>
[DataContract]
public class DeviceGroupEventSource
{
	/// <summary>
	/// The EventSource Id
	/// </summary>
	[DataMember(Name = "eventSourceId")]
	public int EventSourceId { get; set; }

	/// <summary>
	/// The DeviceGroup Id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

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
	/// The DataSource unique name
	/// </summary>
	[DataMember(Name = "eventSourceName")]
	public string EventSourceName { get; set; } = string.Empty;

	/// <summary>
	/// The DataSourceGroup Name
	/// </summary>
	[DataMember(Name = "eventSourceGroupName")]
	public string EventSourceGroupName { get; set; } = string.Empty;

	/// <summary>
	/// List of devices
	/// </summary>
	[DataMember(Name = "EventSourceDeviceList")]
	public List<EventSourceDevice> EventSourceDevices { get; set; } = [];

	/// <summary>
	/// The EventSourceDisplayName
	/// </summary>
	[DataMember(Name = "eventSourceDisplayName")]
	public string EventSourceDisplayName { get; set; } = string.Empty;
}
