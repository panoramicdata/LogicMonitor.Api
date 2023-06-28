namespace LogicMonitor.Api.Devices;

/// <summary>
/// Netflow device info
/// </summary>

[DataContract]
public class NetflowDeviceInfo : IdentifiedItem
{
	/// <summary>
	/// deleted
	/// </summary>
	[DataMember(Name = "deleted")]
	public bool Deleted { get; set; }

	/// <summary>
	/// displayName
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;
}
