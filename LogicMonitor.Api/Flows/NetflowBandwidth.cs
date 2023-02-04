namespace LogicMonitor.Api.Flows;

/// <summary>
/// A Flow
/// </summary>
[DataContract]
public class NetflowBandwidth : NetflowDataBase
{
	/// <summary>
	/// deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName", IsRequired = false)]
	public string DisplayName { get; set; }

	/// <summary>
	/// deviceId
	/// </summary>
	[DataMember(Name = "deviceId", IsRequired = false)]
	public int DeviceId { get; set; }

	/// <summary>
	/// Send in MBytes
	/// </summary>
	[DataMember(Name = "send", IsRequired = false)]
	public long SendMb { get; set; }

	/// <summary>
	/// Receive in MBytes
	/// </summary>
	[DataMember(Name = "receive", IsRequired = false)]
	public long ReceiveMb { get; set; }

	/// <summary>
	/// Usage in MBytes
	/// </summary>
	[DataMember(Name = "usage", IsRequired = false)]
	public long UsageMb { get; set; }

	/// <summary>
	/// Returns a string that represents the current object
	/// </summary>
	public override string ToString() => $"{DisplayName}. Send: {SendMb} MB. Receive: {ReceiveMb} MB. Usage: {UsageMb} MB]";
}
