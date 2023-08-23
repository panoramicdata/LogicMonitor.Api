namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A netscan policy credential set
/// </summary>
[DataContract(Name = "credentials")]
public class NetscanPolicyCredential
{
	/// <summary>
	/// The ID of the device group that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	/// Custom credentials that should be used for this scan
	/// </summary>
	[DataMember(Name = "custom")]
	public object? Custom { get; set; }

	/// <summary>
	/// The name of the device group that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceGroupName")]
	public string? DeviceGroupName { get; set; }

	/// <summary>
	/// The ID of the device that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The name of the device that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceName")]
	public string? DeviceName { get; set; }

	/// <summary>
	/// snmpV3Credentials
	/// </summary>
	[DataMember(Name = "snmpV3Credentials")]
	public List<string> SnmpV3Credentials { get; set; } = new List<string>();
}
