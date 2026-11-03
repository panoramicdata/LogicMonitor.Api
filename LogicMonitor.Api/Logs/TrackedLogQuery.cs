namespace LogicMonitor.Api.Logs;

/// <summary>
/// A tracked log query
/// </summary>
[DataContract]
public class TrackedLogQuery
{
	/// <summary>
	/// Id of record
	/// </summary>
	[DataMember(Name = "id")]
	public long Id { get; set; }

	/// <summary>
	/// Admin id
	/// </summary>
	[DataMember(Name = "adminId")]
	public int AdminId { get; set; }

	/// <summary>
	/// The aggregate
	/// </summary>
	[DataMember(Name = "aggregate")]
	public bool Aggregate { get; set; }

	/// <summary>
	/// Epoch of creation date
	/// </summary>
	[DataMember(Name = "createEpoch")]
	public long CreateEpoch { get; set; }

	/// <summary>
	/// Group ID associated with the tracked query.
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// Group name associated with the tracked query
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	/// Flag stating paused status of tracked query
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "isPaused")]
	public bool IsPaused { get; set; }

	/// <summary>
	/// Label for log filter
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	/// Epoch of last update date
	/// </summary>
	[DataMember(Name = "lastUpdateEpoch")]
	public long LastUpdateEpoch { get; set; }

	/// <summary>
	/// Partitions associated with the query
	/// </summary>
	[DataMember(Name = "partitions")]
	public List<string> Partitions { get; set; } = [];

	/// <summary>
	/// Query for log filter
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// Tracked query uuid for log filter
	/// </summary>
	[DataMember(Name = "trackedQueryUuid")]
	public string TrackedQueryUuid { get; set; } = string.Empty;

	/// <summary>
	/// Query grammar version
	/// </summary>
	[DataMember(Name = "version")]
	public string Version { get; set; } = string.Empty;
}
