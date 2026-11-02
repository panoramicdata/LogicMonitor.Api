namespace LogicMonitor.Api.Logs;

/// <summary>
/// A group of saved log queries
/// </summary>
[DataContract]
public class LogQueryGroup : NamedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	/// The group type
	/// </summary>
	[DataMember(Name = "groupType")]
	public string GroupType { get; set; } = string.Empty;

	/// <summary>
	/// List of log queries in this group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "logQuery")]
	public List<SavedLogQuery> LogQuery { get; set; } = [];

	/// <summary>
	/// Owner of the log query group
	/// </summary>
	[DataMember(Name = "ownerId")]
	public int OwnerId { get; set; }

	/// <summary>
	/// Parent group ID if nested
	/// </summary>
	[DataMember(Name = "parentId")]
	public int ParentId { get; set; }

	/// <summary>
	/// Effective user permission for this group
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; } = string.Empty;

	/// <inheritdoc />
	public string Endpoint() => "log/logquerygroups";
}
