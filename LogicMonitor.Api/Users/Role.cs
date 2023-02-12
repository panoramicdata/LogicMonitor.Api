namespace LogicMonitor.Api.Users;

/// <summary>
///    A LogicMonitor Role
/// </summary>
[DataContract]
public class Role : NamedItem, IHasEndpoint
{
	/// <summary>
	/// Whether or not users assigned this role should be required to acknowledge the EULA (end user license agreement)
	/// </summary>
	[DataMember(Name = "requireEULA", IsRequired = false)]
	public bool RequireEULA { get; set; }

	/// <summary>
	/// Whether Remote Session should be enabled at the account level
	/// </summary>
	[DataMember(Name = "enableRemoteSessionInCompanyLevel", IsRequired = false)]
	public bool IsRemoteSessionEnabledAtCompanyLevel { get; set; }

	/// <summary>
	/// Whether Two-Factor Authentication should be required for this role
	/// </summary>
	[DataMember(Name = "twoFARequired", IsRequired = false)]
	public bool TwoFARequired { get; set; }

	/// <summary>
	/// Whether Two-Factor Authentication should be required for the entire account
	/// </summary>
	[DataMember(Name = "acctRequireTwoFA", IsRequired = false)]
	public bool AcctRequireTwoFA { get; set; }

	/// <summary>
	/// The label for the custom help URL as it will appear in the Help \u0026 Support dropdown menu
	/// </summary>
	[DataMember(Name = "customHelpLabel", IsRequired = false)]
	public string? CustomHelpLabel { get; set; }

	/// <summary>
	/// The URL that should be added to the Help \u0026 Support dropdown menu
	/// </summary>
	[DataMember(Name = "customHelpUrl", IsRequired = false)]
	public string? CustomHelpUrl { get; set; }

	/// <summary>
	/// The count of the users which are belongs to the role
	/// </summary>
	[DataMember(Name = "associatedUserCount", IsRequired = false)]
	public int AssociatedUserCount { get; set; }

	/// <summary>
	/// The account privileges associated with the role. Privileges can be added to a role for each area of your account
	/// </summary>
	[DataMember(Name = "privileges", IsRequired = true)]
	public List<RolePrivilege> Privileges { get; set; } = null!;

	/// <summary>
	/// The group Id of the role
	/// </summary>
	[DataMember(Name = "roleGroupId", IsRequired = false)]
	public int RoleGroupId { get; set; }

	/// <summary>
	/// The permission of current role with the admin. values can be write|read|none
	/// </summary>
	[DataMember(Name = "userPermission", IsRequired = false)]
	public UserPermission? UserPermission { get; set; }

	/// <summary>
	/// Whether this is a SAML role
	/// </summary>
	[DataMember(Name = "isSamlRole", IsRequired = false)]
	public bool IsSamlRole { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/roles";
}
