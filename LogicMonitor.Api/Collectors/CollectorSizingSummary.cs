namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A summary of collector sizing recommendations
/// </summary>
[DataContract]
public class CollectorSizingSummary
{
	/// <summary>
	/// The active workloads
	/// </summary>
	[DataMember(Name = "activeWorkloads")]
	public List<string> ActiveWorkloads { get; set; } = [];

	/// <summary>
	/// The configuration used
	/// </summary>
	[DataMember(Name = "configurationUsed")]
	public Dictionary<string, object> ConfigurationUsed { get; set; } = [];

	/// <summary>
	/// The largest size
	/// </summary>
	[DataMember(Name = "largestSize")]
	public string LargestSize { get; set; } = string.Empty;

	/// <summary>
	/// The total collectors
	/// </summary>
	[DataMember(Name = "totalCollectors")]
	public int TotalCollectors { get; set; }
}
