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
	[DataMember(Name = "deviceDisplayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Resource Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// Send in MBytes
	/// </summary>
	[DataMember(Name = "send")]
	public long SendMb { get; set; }

	/// <summary>
	/// Receive in MBytes
	/// </summary>
	[DataMember(Name = "receive")]
	public long ReceiveMb { get; set; }

	/// <summary>
	/// Usage in MBytes
	/// </summary>
	[DataMember(Name = "usage")]
	public long UsageMb { get; set; }

	/// <summary>
	/// percentUsage
	/// </summary>
	[DataMember(Name = "percentUsage")]
	public long PercentUsage { get; set; }

	/// <summary>
	/// Returns a string that represents the current object
	/// </summary>
	public override string ToString() => $"{DisplayName}. Send: {SendMb} MB. Receive: {ReceiveMb} MB. Usage: {UsageMb} MB]";
}
