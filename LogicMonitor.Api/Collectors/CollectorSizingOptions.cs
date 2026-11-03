namespace LogicMonitor.Api.Collectors;

/// <summary>
/// Options for collector sizing
/// </summary>
[DataContract]
public class CollectorSizingOptions
{
	/// <summary>
	/// The forced size
	/// </summary>
	[DataMember(Name = "forcedSize")]
	public string ForcedSize { get; set; } = string.Empty;

	/// <summary>
	/// The logs failover
	/// </summary>
	[DataMember(Name = "logsFailover")]
	public bool LogsFailover { get; set; }

	/// <summary>
	/// The max load percent
	/// </summary>
	[DataMember(Name = "maxLoadPercent")]
	public int MaxLoadPercent { get; set; }

	/// <summary>
	/// The netflow failover
	/// </summary>
	[DataMember(Name = "netflowFailover")]
	public bool NetflowFailover { get; set; }

	/// <summary>
	/// The polling failover
	/// </summary>
	[DataMember(Name = "pollingFailover")]
	public bool PollingFailover { get; set; }
}
