namespace LogicMonitor.Api.Devices;

/// <summary>
/// A DeviceGroup and its Devices. For internal use only.
/// </summary>
[DataContract]
public class DeviceGroupAndDevices
{
	/// <summary>
	/// DeviceGroup
	/// </summary>
	[DataMember(Name = "hostGroup")]
	public DeviceGroup DeviceGroup { get; set; }

	/// <summary>
	/// Devices
	/// </summary>
	[DataMember(Name = "hosts")]
	public List<Device> Devices { get; set; }
}
