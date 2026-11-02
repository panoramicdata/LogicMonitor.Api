namespace LogicMonitor.Api.Logs;

/// <summary>
/// A request to create a log query group
/// </summary>
[DataContract]
public class LogQueryGroupCreationDto : CreationDto<LogQueryGroup>
{
	/// <summary>
	/// Description of the log query group
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The group type
	/// </summary>
	[DataMember(Name = "groupType")]
	public string GroupType { get; set; } = string.Empty;

	/// <summary>
	/// The name of the log query group
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

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
}
