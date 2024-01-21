namespace LogicMonitor.Api.Users;

/// <summary>
/// RoleGroup creation DTO
/// </summary>
[DataContract]
public class RoleGroupCreationDto : CreationDto<RoleGroup>, IHasName, IHasDescription
{
	/// <summary>
	/// Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;
}
