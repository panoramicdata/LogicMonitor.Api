using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The EventSource Device
/// </summary>
[DataContract]
public class EventSourceDevice
{
	/// <summary>
	/// The deviceEventSourceId
	/// </summary>
	[DataMember(Name = "deviceEventSourceId")]
	public int DeviceEventSourceId { get; set; }

	/// <summary>
	/// The EventSource Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }
}
