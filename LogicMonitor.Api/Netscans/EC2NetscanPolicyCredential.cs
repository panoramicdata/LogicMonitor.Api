namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A netscan policy credential set
/// </summary>
[DataContract(Name = "credentials")]
public class EC2NetscanPolicyCredential
{
	/// <summary>
	/// The ID of the device group that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceGroupId", IsRequired = false)]
	public int DeviceGroupId { get; set; }

	/// <summary>
	/// Custom credentials that should be used for this scan
	/// </summary>
	[DataMember(Name = "custom", IsRequired = false)]
	public object? Custom { get; set; }

	/// <summary>
	/// The name of the device group that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceGroupName", IsRequired = false)]
	public string? DeviceGroupName { get; set; }

	/// <summary>
	/// The ID of the device that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceId", IsRequired = false)]
	public int DeviceId { get; set; }

	/// <summary>
	/// The name of the device that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceName", IsRequired = false)]
	public string? DeviceName { get; set; }
}
