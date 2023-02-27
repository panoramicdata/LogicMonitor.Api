namespace LogicMonitor.Api.Users;

/// <summary>
/// A user group
/// </summary>
[DataContract]
public class UserGroup : NamedItem, IHasEndpoint
{
	/// <summary>
	/// The parent group Id
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
	/// The user permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermissionValues UserPermission { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/admin/groups";
}
