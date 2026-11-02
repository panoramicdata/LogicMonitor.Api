namespace LogicMonitor.Api.Collectors;

/// <summary>
/// The result of a collector status check
/// </summary>
[DataContract]
public class CollectorStatusCheck
{
	/// <summary>
	/// The aborted
	/// </summary>
	[DataMember(Name = "aborted")]
	public bool Aborted { get; set; }

	/// <summary>
	/// The check points
	/// </summary>
	[DataMember(Name = "checkPoints")]
	public List<CollectorStatusCheckPoint>? CheckPoints { get; set; } = [];

	/// <summary>
	/// The finished
	/// </summary>
	[DataMember(Name = "finished")]
	public bool Finished { get; set; }

	/// <summary>
	/// The finished script count
	/// </summary>
	[DataMember(Name = "finishedScriptCount")]
	public int FinishedScriptCount { get; set; }

	/// <summary>
	/// The last triggered mills
	/// </summary>
	[DataMember(Name = "lastTriggeredMills")]
	public long LastTriggeredMills { get; set; }

	/// <summary>
	/// The last update mills
	/// </summary>
	[DataMember(Name = "lastUpdateMills")]
	public long LastUpdateMills { get; set; }

	/// <summary>
	/// The skipped script count
	/// </summary>
	[DataMember(Name = "skippedScriptCount")]
	public int SkippedScriptCount { get; set; }

	/// <summary>
	/// The total script count
	/// </summary>
	[DataMember(Name = "totalScriptCount")]
	public int TotalScriptCount { get; set; }
}
