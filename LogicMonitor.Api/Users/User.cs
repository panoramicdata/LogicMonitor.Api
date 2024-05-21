namespace LogicMonitor.Api.Users;

/// <summary>
///    User logged event
/// </summary>
[DataContract]
public class User : IdentifiedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	///    Whether this user is for API use only
	/// </summary>
	[DataMember(Name = "apionly")]
	public bool IsApiOnly { get; set; }

	/// <summary>
	///    The roles assigned to this user
	/// </summary>
	[DataMember(Name = "roles")]
	public List<Role> Roles { get; set; } = [];

	/// <summary>
	///    The user's preferred contact method
	/// </summary>
	[DataMember(Name = "contactMethod")]
	public string ContactMethod { get; set; } = string.Empty;

	/// <summary>
	///    The e-mail address
	/// </summary>
	[DataMember(Name = "email")]
	public string Email { get; set; } = string.Empty;

	/// <summary>
	///    The Training e-mail address
	/// </summary>
	[DataMember(Name = "trainingEmail")]
	public string TrainingEmail { get; set; } = string.Empty;

	/// <summary>
	///    The phone number
	/// </summary>
	[DataMember(Name = "phone")]
	public string Phone { get; set; } = string.Empty;

	/// <summary>
	///    The SMS e-mail address
	/// </summary>
	[DataMember(Name = "smsEmail")]
	public string SmsEmail { get; set; } = string.Empty;

	/// <summary>
	///    The SMS e-mail address format
	/// </summary>
	[DataMember(Name = "smsEmailFormat")]
	public string SmsEmailFormat { get; set; } = string.Empty;

	/// <summary>
	///    The UserName
	/// </summary>
	[DataMember(Name = "username")]
	public string UserName { get; set; } = string.Empty;

	/// <summary>
	///    The first name
	/// </summary>
	[DataMember(Name = "firstName")]
	public string FirstName { get; set; } = string.Empty;

	/// <summary>
	///    The last name
	/// </summary>
	[DataMember(Name = "lastName")]
	public string LastName { get; set; } = string.Empty;

	/// <summary>
	///    The password (asterisked out)
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = string.Empty;

	/// <summary>
	///    The status ("active" for users who can log in)
	/// </summary>
	[DataMember(Name = "status")]
	public UserStatus Status { get; set; }

	/// <summary>
	///    The status ("active" for users who can log in)
	/// </summary>
	[DataMember(Name = "lastLoginOn")]
	public long LastLoginOnSeconds { get; set; }

	/// <summary>
	///    The note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; } = string.Empty;

	/// <summary>
	///    The user that created this user
	/// </summary>
	[DataMember(Name = "createdBy")]
	public string CreatedBy { get; set; } = string.Empty;

	/// <summary>
	///    Whether the password will be force changed on next login
	/// </summary>
	[DataMember(Name = "forcePasswordChange")]
	public bool ForcePasswordChange { get; set; }

	/// <summary>
	///    The user's API tokens
	/// </summary>
	[DataMember(Name = "apiTokens")]
	public List<ApiToken> ApiTokens { get; set; } = [];

	/// <summary>
	///    The view permissions
	/// </summary>
	[DataMember(Name = "viewPermission")]
	public ViewPermission? ViewPermission { get; set; }

	/// <summary>
	///    The last action performed by this user
	/// </summary>
	[DataMember(Name = "lastAction")]
	public string LastAction { get; set; } = string.Empty;

	/// <summary>
	///    The last local action time as a string
	/// </summary>
	[DataMember(Name = "lastActionOnLocal")]
	public string LastActionOnLocal { get; set; } = string.Empty;

	/// <summary>
	///    The last action time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastActionOn")]
	public long LastActionOnSeconds { get; set; }

	/// <summary>
	///    The accept EULA time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "acceptEULAOn")]
	public long AcceptEulaOnSeconds { get; set; }

	/// <summary>
	///    Whether the user has accepted the EULA
	/// </summary>
	[DataMember(Name = "acceptEULA")]
	public bool AcceptEula { get; set; }

	/// <summary>
	///    Two factor authentication is enabled
	/// </summary>
	[DataMember(Name = "twoFAEnabled")]
	public bool IsTwoFactorAuthenticationEnabled { get; set; }

	/// <summary>
	///    Requires two factor authentication
	/// </summary>
	[DataMember(Name = "twoFARequired")]
	public bool IsTwoFactorAuthenticationRequiredByUser { get; set; }

	/// <summary>
	///    The setupWizardStatus
	/// </summary>
	[DataMember(Name = "setupWizardStatus")]
	public string SetupWizardStatus { get; set; } = string.Empty;

	/// <summary>
	///    The tenant ID
	/// </summary>
	[DataMember(Name = "tenantId")]
	public int TenantId { get; set; }

	/// <summary>
	///    Two factor authentication is enabled
	/// </summary>
	[DataMember(Name = "timezone")]
	public string Timezone { get; set; } = string.Empty;

	/// <summary>
	///    The user group ids
	/// </summary>
	[DataMember(Name = "adminGroupIds")]
	public List<int> UserGroupIds { get; set; } = [];

	/// <summary>
	///    The user permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission? UserPermission { get; set; }

	/// <summary>
	///    The DateTime the user last logged in UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime? LastLoginOnUtc => LastLoginOnSeconds.ToNullableDateTimeUtc();

	/// <summary>
	///    The DateTime the user accepted the EULA UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime? ViewMessageOnUtc => AcceptEulaOnSeconds.ToNullableDateTimeUtc();

	/// <summary>
	///    The last user action time in UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime? LastActionOnUtc => LastActionOnSeconds.ToNullableDateTimeUtc();

	/// <summary>
	///    Whether they should be forced to logout immediately
	/// </summary>
	[DataMember(Name = "immediateForceLogout")]
	public bool ImmediateForceLogout { get; set; }

	/// <summary>
	///    The last authorized IP address
	/// </summary>
	[DataMember(Name = "lastAuthIp")]
	public string LastAuthIp { get; set; } = string.Empty;

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/admins";

	/// <summary>
	///    Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{FirstName} {LastName} ({UserName})";
}
