namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A response from LogicMonitor when requesting a netscan policy execution
/// </summary>
[DataContract]
public class NetscanExecutionResponse
{
	/// <summary>
	///    The response id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	///    The scan id
	/// </summary>
	[DataMember(Name = "scanId")]
	public string ScanId { get; set; }

	/// <summary>
	///    The status
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; }

	/// <summary>
	///    The Id
	/// </summary>
	[DataMember(Name = "nspId")]
	public int NetscanPolicyId { get; set; }

	/// <summary>
	///    The time that the scan started in seconds since the Epoch UTC
	/// </summary>
	[DataMember(Name = "startEpoch")]
	public long StartEpochUtcSeconds { get; set; }

	/// <summary>
	///    The date/time that the scan started
	/// </summary>
	public DateTime StartDateTimeUtc => StartEpochUtcSeconds.ToDateTimeUtc();

	/// <summary>
	///    The time that the scan ended in seconds since the Epoch UTC
	/// </summary>
	[DataMember(Name = "endEpoch")]
	public long EndEpochUtcSeconds { get; set; }

	/// <summary>
	///    The date/time that the scan ended
	/// </summary>
	public DateTime EndDateTimeUtc => StartEpochUtcSeconds.ToDateTimeUtc();

	/// <summary>
	///    The user that ran the netscan
	/// </summary>
	[DataMember(Name = "runBy")]
	public string RunBy { get; set; }
}
