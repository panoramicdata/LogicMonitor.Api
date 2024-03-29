namespace LogicMonitor.Api.Flows;

/// <summary>
/// A Flow
/// </summary>
[DataContract]
public class NetflowEndpoint : NetflowDataBase
{
	/// <summary>
	/// IP address
	/// </summary>
	[DataMember(Name = "IP")]
	public string Ip { get; set; } = string.Empty;

	/// <summary>
	/// Source DNS
	/// </summary>
	[DataMember(Name = "dns")]
	public string Dns { get; set; } = string.Empty;

	/// <summary>
	/// Last seen in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastSeen")]
	public long LastSeenSeconds { get; set; }

	/// <summary>
	/// Last data seen
	/// </summary>
	[IgnoreDataMember]
	public DateTime LastSeenDateTimeUtc => LastSeenSeconds.ToDateTimeUtc();

	/// <summary>
	/// Usage in bytes
	/// </summary>
	[DataMember(Name = "usage")]
	public long UsageBytes { get; set; }

	/// <summary>
	/// Usage in percent
	/// </summary>
	[DataMember(Name = "percentUsage")]
	public double PercentUsage { get; set; }

	/// <summary>
	/// Type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{Dns}({Ip}) [{UsageBytes} bytes] last seen {LastSeenDateTimeUtc} UTC";
}
