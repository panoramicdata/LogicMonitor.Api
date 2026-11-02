namespace LogicMonitor.Api.Logs;

/// <summary>
/// A request to create a tracked query group
/// </summary>
[DataContract]
public class TrackedQueryGroupCreationDto : CreationDto<TrackedQueryGroup>
{
	/// <summary>
	/// Description of the tracked query group
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Name of the tracked query group
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

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
}
