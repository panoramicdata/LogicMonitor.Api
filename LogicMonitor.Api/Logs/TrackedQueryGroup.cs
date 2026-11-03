namespace LogicMonitor.Api.Logs;

/// <summary>
/// A group of tracked log queries
/// </summary>
[DataContract]
public class TrackedQueryGroup : NamedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	/// Owner of the tracked query group
	/// </summary>
	[DataMember(Name = "ownerId")]
	public int OwnerId { get; set; }

	/// <summary>
	/// Parent ID of the tracked query group
	/// </summary>
	[DataMember(Name = "parentId")]
	public int ParentId { get; set; }

	/// <summary>
	/// List of tracked queries under this group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "trackedQuery")]
	public List<TrackedLogQuery> TrackedQuery { get; set; } = [];

	/// <inheritdoc />
	public string Endpoint() => "trackedquerygroups";
}
