namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A netscan policy credential set
/// </summary>
[DataContract(Name = "credentials")]
public class RestNMapNetscanPolicyCredential
{
	/// <summary>
	/// The ID of the ResourceGroup that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[Obsolete("Use ResourceGroupId instead", true)]
	[JsonIgnore, IgnoreDataMember]
	public int DeviceGroupId => ResourceGroupId;

	/// <summary>
	/// Custom credentials that should be used for this scan
	/// </summary>
	[DataMember(Name = "custom")]
	public object? Custom { get; set; }

	/// <summary>
	/// The name of the ResourceGroup that credentials should be inherited from, for this scan
	/// </summary>
	[DataMember(Name = "deviceGroupName")]
	public string? ResourceGroupName { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[Obsolete("Use ResourceGroupName instead", true)]
	[JsonIgnore, IgnoreDataMember]
	public string? DeviceGroupName => ResourceGroupName;

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
}
