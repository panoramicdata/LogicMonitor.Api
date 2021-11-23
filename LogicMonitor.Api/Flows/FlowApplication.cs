namespace LogicMonitor.Api.Flows;

/// <summary>
/// A Flow
/// </summary>
[DataContract]
public class FlowApplication
{
	/// <summary>
	/// The data type
	/// </summary>
	[DataMember(Name = "dataType")]
	public string DataType { get; set; }

	/// <summary>
	/// Source DNS
	/// </summary>
	[DataMember(Name = "dns")]
	public string Dns { get; set; }

	/// <summary>
	/// IP address
	/// </summary>
	[DataMember(Name = "IP")]
	public string Ip { get; set; }

	/// <summary>
	/// The protocol
	/// </summary>
	[DataMember(Name = "protocol")]
	public string Protocol { get; set; }

	/// <summary>
	/// The port number
	/// </summary>
	[DataMember(Name = "port")]
	public int Port { get; set; }

	/// <summary>
	/// The percentage usage
	/// </summary>
	[DataMember(Name = "percentUsage")]
	public double PercentUsage { get; set; }

	/// <summary>
	/// First seen in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "firstEpochInSec")]
	public long FirstSeenSeconds { get; set; }

	/// <summary>
	/// First data seen
	/// </summary>
	[IgnoreDataMember]
	public DateTime FirstSeenDateTimeUtc => FirstSeenSeconds.ToDateTimeUtc();

	/// <summary>
	/// Last seen in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastEpochInSec")]
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
	/// Source bytes
	/// </summary>
	[DataMember(Name = "sourceBytes")]
	public long SourceBytes { get; set; }

	/// <summary>
	/// Source megabytes
	/// </summary>
	[DataMember(Name = "sourceMBytes")]
	public long SourceMb { get; set; }

	/// <summary>
	/// Destination bytes
	/// </summary>
	[DataMember(Name = "destinationBytes")]
	public long DestinationBytes { get; set; }

	/// <summary>
	/// Destination megabytes
	/// </summary>
	[DataMember(Name = "destinationMBytes")]
	public long DestinationMb { get; set; }

	/// <summary>
	/// Flow count
	/// </summary>
	[DataMember(Name = "flowCount")]
	public int FlowCount { get; set; }

	/// <summary>
	/// Flow count
	/// </summary>
	[DataMember(Name = "clientCount")]
	public int ClientCount { get; set; }

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{Dns}({Ip}) [{UsageBytes} bytes] last seen {LastSeenDateTimeUtc} UTC";
}
