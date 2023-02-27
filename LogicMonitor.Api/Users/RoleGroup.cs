namespace LogicMonitor.Api.Users;

/// <summary>
/// A role group
/// </summary>
[DataContract]
public class RoleGroup : NamedItem, IHasEndpoint
{
	/// <summary>
	/// parentId
	/// </summary>
	[DataMember(Name = "parentId")]
	public int ParentId { get; set; }

	/// <summary>
	/// CreatedOn
	/// </summary>
	[DataMember(Name = "createdOn")]
	public long CreatedOn { get; set; }

	/// <summary>
	/// Updated On
	/// </summary>
	[DataMember(Name = "updatedOn")]
	public long UpdatedOn { get; set; }

	/// <summary>
	/// User Permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermissionValues UserPermission { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/role/groups";
}
