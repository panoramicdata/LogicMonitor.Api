namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A response from LogicMonitor when requesting a netscan policy execution
/// </summary>
[DataContract]
public class NetscanExecution
{
	/// <summary>
	///    The response id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "policyName")]
	public string PolicyName { get; set; }

	/// <summary>
	///    The netscan poicy id
	/// </summary>
	[DataMember(Name = "nspId")]
	public int NetscanPolicyId { get; set; }

	/// <summary>
	///    The Collector Id
	/// </summary>
	[DataMember(Name = "collector")]
	public int CollectorId { get; set; }

	/// <summary>
	///    Full scan
	/// </summary>
	[DataMember(Name = "fullScan")]
	public string FullScan { get; set; }

	/// <summary>
	///    The summary text
	/// </summary>
	[DataMember(Name = "summary")]
	public string Summary { get; set; }

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
