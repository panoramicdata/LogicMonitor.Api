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
	[DataMember(Name = "IP", IsRequired = false)]
	public string? Ip { get; set; }

	/// <summary>
	/// Source DNS
	/// </summary>
	[DataMember(Name = "dns", IsRequired = false)]
	public string? Dns { get; set; }

	/// <summary>
	/// Last seen in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastSeen", IsRequired = false)]
	public long LastSeenSeconds { get; set; }

	/// <summary>
	/// Last data seen
	/// </summary>
	[IgnoreDataMember]
	public DateTime LastSeenDateTimeUtc => LastSeenSeconds.ToDateTimeUtc();

	/// <summary>
	/// Usage in bytes
	/// </summary>
	[DataMember(Name = "usage", IsRequired = false)]
	public long UsageBytes { get; set; }

	/// <summary>
	/// Usage in percent
	/// </summary>
	[DataMember(Name = "percentUsage", IsRequired = false)]
	public double PercentUsage { get; set; }

	/// <summary>
	/// Type
	/// </summary>
	[DataMember(Name = "type", IsRequired = false)]
	public string? Type { get; set; }

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{Dns}({Ip}) [{UsageBytes} bytes] last seen {LastSeenDateTimeUtc} UTC";
}
