namespace LogicMonitor.Api.Users;

/// <summary>
/// RoleGroup creation DTO
/// </summary>
[DataContract]
public class UserGroupCreationDto : CreationDto<UserGroup>
{
	/// <summary>
	/// Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }
}
