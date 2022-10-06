namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A netscan policy credential set
/// </summary>
[DataContract(Name = "credentials")]
public class NetscanCredentials
{
	/// <summary>
	/// Device Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// Device Name
	/// </summary>
	[DataMember(Name = "deviceName")]
	public string DeviceName { get; set; } = string.Empty;

	/// <summary>
	///    The device group id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	///    The device group name
	/// </summary>
	[DataMember(Name = "deviceGroupName")]
	public string DeviceGroupName { get; set; }

	/// <summary>
	///    The custom
	/// </summary>
	[DataMember(Name = "custom")]
	public object Custom { get; set; }
}
