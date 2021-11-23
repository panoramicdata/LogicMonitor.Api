namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A collection of devices to which the EventSource applies
/// </summary>
[DataContract]
public class EventSourceAppliesToCollection : IdentifiedItem
{
	/// <summary>
	/// The EventSource Id
	/// </summary>
	[DataMember(Name = "eventSourceId")]
	public int EventSourceId { get; set; }

	/// <summary>
	/// The EventSourceDisplayName
	/// </summary>
	[DataMember(Name = "eventSourceDisplayName")]
	public string EventSourceDisplayName { get; set; }

	/// <summary>
	/// The EventSourceGroupName
	/// </summary>
	[DataMember(Name = "eventSourceGroupName")]
	public string EventSourceGroupName { get; set; }

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
	/// The EventSource Id
	/// </summary>
	[DataMember(Name = "EventSourceDeviceList")]
	public List<EventSourceDevice> EventSourceDevices { get; set; }

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{EventSourceDisplayName} ({EventSourceId}) with {EventSourceDevices?.Count.ToString() ?? "0"} devices";
}
