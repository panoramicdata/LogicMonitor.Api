namespace LogicMonitor.Api.Websites;

/// <summary>
/// PingCheck
/// </summary>

[DataContract]
public class PingCheck
{
	/// <summary>
	/// The percentage of packets that should be returned in the time period specified by timeoutInMSPktsNotReceive for each ping check
	/// </summary>
	[DataMember(Name = "percentPktsNotReceiveInTime")]
	public int PercentPacketsNotReceiveInTime { get; set; }

	/// <summary>
	/// The number of packets to send
	/// </summary>
	[DataMember(Name = "count")]
	public int Count { get; set; }

	/// <summary>
	/// The URL to check, without the scheme or protocol (e.g http or https)\nE.g. if the URL is \"http://www.google.com, then the host\u003d\"www.google.com\"
	/// </summary>
	[DataMember(Name = "host")]
	public string HostName { get; set; } = null!;

	/// <summary>
	/// The time period that the percentage of packets specified by percentPktsNotReceiveInTime must be returned in for each ping check
	/// </summary>
	[DataMember(Name = "timeoutInMSPktsNotReceive")]
	public long PacketsNotReceivedTimeoutMs { get; set; }
}
