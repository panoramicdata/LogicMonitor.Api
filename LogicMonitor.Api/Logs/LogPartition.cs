namespace LogicMonitor.Api.Logs;

/// <summary>
/// A log partition
/// </summary>
[DataContract]
public class LogPartition : StringIdentifiedItem, IHasEndpoint
{
	/// <summary>
	/// Partition active state
	/// </summary>
	[DataMember(Name = "active")]
	public bool Active { get; set; }

	/// <summary>
	/// Active contract
	/// </summary>
	[DataMember(Name = "activeContract")]
	public LogPartitionContract? ActiveContract { get; set; }

	/// <summary>
	/// Partition Criteria
	/// </summary>
	[DataMember(Name = "criteria")]
	public string Criteria { get; set; } = string.Empty;

	/// <summary>
	/// Partition description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Partition Fullname
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "fullname")]
	public string Fullname { get; set; } = string.Empty;

	/// <summary>
	/// Future contract
	/// </summary>
	[DataMember(Name = "futureContract")]
	public LogPartitionContract? FutureContract { get; set; }

	/// <summary>
	/// Partition Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Parent Partition Id
	/// </summary>
	[DataMember(Name = "parent")]
	public int Parent { get; set; }

	/// <summary>
	/// Epoch time when partition was paused
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "pausedAt")]
	public long PausedAt { get; set; }

	/// <summary>
	/// Previous (expired) contract
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "previousContract")]
	public LogPartitionContract? PreviousContract { get; set; }

	/// <summary>
	/// Partition Priority
	/// </summary>
	[DataMember(Name = "priority")]
	public int Priority { get; set; }

	/// <summary>
	/// Tenant Name
	/// </summary>
	[DataMember(Name = "tenant")]
	public string Tenant { get; set; } = string.Empty;

	/// <summary>
	/// Partition user permission
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; } = string.Empty;

	/// <inheritdoc />
	public string Endpoint() => "log/partitions";
}
