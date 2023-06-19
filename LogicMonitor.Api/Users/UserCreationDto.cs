namespace LogicMonitor.Api.Users;

/// <summary>
/// A User creation DTO
/// </summary>
[DataContract]
public class UserCreationDto : CreationDto<User>
{
	/// <summary>
	/// The view permission
	/// </summary>
	[DataMember(Name = "viewPermission")]
	public ViewPermission ViewPermission { get; set; } = new();

	/// <summary>
	/// The username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; } = string.Empty;

	/// <summary>
	/// The first name
	/// </summary>
	[DataMember(Name = "firstName")]
	public string FirstName { get; set; } = string.Empty;

	/// <summary>
	/// The last name
	/// </summary>
	[DataMember(Name = "lastName")]
	public string LastName { get; set; } = string.Empty;

	/// <summary>
	/// The e-mail address
	/// </summary>
	[DataMember(Name = "email")]
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// The password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = string.Empty;

	/// <summary>
	/// The password again
	/// </summary>
	[DataMember(Name = "password1")]
	public string Password1 { get; set; } = string.Empty;

	/// <summary>
	/// Whether to force password changes
	/// </summary>
	[DataMember(Name = "forcePasswordChange")]
	public bool ForcePasswordChange { get; set; }

	/// <summary>
	/// Whether 2FA is enabled
	/// </summary>
	[DataMember(Name = "twoFAEnabled")]
	public bool TwoFAEnabled { get; set; }

	/// <summary>
	/// The SMS e-mail
	/// </summary>
	[DataMember(Name = "smsEmail")]
	public string SmsEmail { get; set; } = string.Empty;

	/// <summary>
	/// The SMS Email format
	/// </summary>
	[DataMember(Name = "smsEmailFormat")]
	public string SmsEmailFormat { get; set; } = string.Empty;

	/// <summary>
	/// The timezone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string Timezone { get; set; } = string.Empty;

	/// <summary>
	/// The view permissions
	/// </summary>
	[DataMember(Name = "view-permission")]
	public List<bool> ViewPermissions { get; set; } = new();

	/// <summary>
	/// The status
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;

	/// <summary>
	/// The note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; } = string.Empty;

	/// <summary>
	/// The roles
	/// </summary>
	[DataMember(Name = "roles")]
	public List<Role> Roles { get; set; } = new();

	/// <summary>
	/// The API tokens
	/// </summary>
	[DataMember(Name = "apiTokens")]
	public List<object> ApiTokens { get; set; } = new();

	/// <summary>
	/// Phone number
	/// </summary>
	[DataMember(Name = "phone")]
	public string Phone { get; set; } = string.Empty;

	/// <summary>
	/// Set to true to only permit API-based login
	/// </summary>
	[DataMember(Name = "apionly")]
	public bool ApiOnly { get; set; }

	/// <summary>
	/// A list of user groups to which to add the user.
	/// </summary>
	[DataMember(Name = "adminGroupIds")]
	public List<int> UserGroupIds { get; set; } = new List<int> { 1 };
}
