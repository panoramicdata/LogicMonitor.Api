namespace LogicMonitor.Api.Collectors;

/// <summary>
/// Collector sizing recommendations
/// </summary>
[DataContract]
public class CollectorSizingResponse
{
	/// <summary>
	/// The logs
	/// </summary>
	[DataMember(Name = "logs")]
	public CollectorSizingRecommendation? Logs { get; set; }

	/// <summary>
	/// The netflow
	/// </summary>
	[DataMember(Name = "netflow")]
	public CollectorSizingRecommendation? Netflow { get; set; }

	/// <summary>
	/// The polling
	/// </summary>
	[DataMember(Name = "polling")]
	public CollectorSizingRecommendation? Polling { get; set; }

	/// <summary>
	/// The summary
	/// </summary>
	[DataMember(Name = "summary")]
	public CollectorSizingSummary? Summary { get; set; }
}
