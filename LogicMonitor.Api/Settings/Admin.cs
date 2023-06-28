namespace LogicMonitor.Api.Settings;

/// <summary>
/// Admin
/// </summary>

[DataContract]
public class Admin : IdentifiedItem
{
	/// <summary>
	/// The last name associated with the user
	/// </summary>
	[DataMember(Name = "lastName")]
	public string LastName { get; set; } = string.Empty;

	/// <summary>
	/// Any notes assocaited with the user
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; } = string.Empty;

	/// <summary>
	/// The account tabs that will be visible to the user
	/// </summary>
	[DataMember(Name = "viewPermission")]
	public string ViewPermission { get; set; } = string.Empty;

	/// <summary>
	/// The timezone of the user
	/// </summary>
	[DataMember(Name = "timezone")]
	public string Timezone { get; set; } = string.Empty;

	/// <summary>
	/// The roles assigned to the user
	/// </summary>
	[DataMember(Name = "roles")]
	public List<Role> Roles { get; set; } = new();

	/// <summary>
	/// The time that the user last logged in, in epoch format
	/// </summary>
	[DataMember(Name = "lastLoginOn")]
	public long LastLoginOn { get; set; }

	/// <summary>
	/// The time, in local format, of the user\u0027s last action
	/// </summary>
	[DataMember(Name = "lastActionOnLocal")]
	public string LastActionOnLocal { get; set; } = string.Empty;

	/// <summary>
	/// sms | fullText, where sms \u003d 160 characters and fullText\u003d all characters
	/// </summary>
	[DataMember(Name = "smsEmailFormat")]
	public string SmsEmailFormat { get; set; } = string.Empty;

	/// <summary>
	/// Whether it is a API only user
	/// </summary>
	[DataMember(Name = "apionly")]
	public bool Apionly { get; set; }

	/// <summary>
	/// Any API Tokens associated with the user
	/// </summary>
	[DataMember(Name = "apiTokens")]
	public List<ApiToken> ApiTokens { get; set; } = new();

	/// <summary>
	/// The Id(s) of the groups the admin is in, where multiple group ids are comma separated
	/// </summary>
	[DataMember(Name = "adminGroupIds")]
	public List<int> AdminGroupIds { get; set; } = new();

	/// <summary>
	/// The password associated with the user
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = string.Empty;

	/// <summary>
	/// The last action taken by the user
	/// </summary>
	[DataMember(Name = "lastAction")]
	public string LastAction { get; set; } = string.Empty;

	/// <summary>
	/// The email address for user\u0027s Training account
	/// </summary>
	[DataMember(Name = "trainingEmail")]
	public string TrainingEmail { get; set; } = string.Empty;

	/// <summary>
	/// The time, in epoch format, of the user\u0027s last action
	/// </summary>
	[DataMember(Name = "lastActionOn")]
	public long LastActionOn { get; set; }

	/// <summary>
	/// The email address associated with the user
	/// </summary>
	[DataMember(Name = "email")]
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// email | smsemail
	/// </summary>
	[DataMember(Name = "contactMethod")]
	public string ContactMethod { get; set; } = string.Empty;

	/// <summary>
	/// The time, in epoch format, that the user accepted the EULA (if required to)
	/// </summary>
	[DataMember(Name = "acceptEULAOn")]
	public long AcceptEULAOn { get; set; }

	/// <summary>
	/// The permission of current user with the admin. values can be write|read|none
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; } = string.Empty;

	/// <summary>
	/// The sms email address associated with the user
	/// </summary>
	[DataMember(Name = "smsEmail")]
	public string SmsEmail { get; set; } = string.Empty;

	/// <summary>
	/// Whether or not two factor authentication is enabled for the user
	/// </summary>
	[DataMember(Name = "twoFAEnabled")]
	public bool TwoFAEnabled { get; set; }

	/// <summary>
	/// The first name associated with the user
	/// </summary>
	[DataMember(Name = "firstName")]
	public string FirstName { get; set; } = string.Empty;

	/// <summary>
	/// The phone number associated with the user
	/// </summary>
	[DataMember(Name = "phone")]
	public string Phone { get; set; } = string.Empty;

	/// <summary>
	/// Who created the user. This may be another user, SAML or LogicMonitor
	/// </summary>
	[DataMember(Name = "createdBy")]
	public string CreatedBy { get; set; } = string.Empty;

	/// <summary>
	/// Whether or not the user should be forced to change their password on the next login
	/// </summary>
	[DataMember(Name = "forcePasswordChange")]
	public string ForcePasswordChange { get; set; } = string.Empty;

	/// <summary>
	/// The tenant id of the user
	/// </summary>
	[DataMember(Name = "tenantId")]
	public int TenantId { get; set; }

	/// <summary>
	/// Whether or not the user is required to accept the EULA (end user license agreement)
	/// </summary>
	[DataMember(Name = "acceptEULA")]
	public bool AcceptEULA { get; set; }

	/// <summary>
	/// The username associated with the user
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; } = string.Empty;

	/// <summary>
	/// The user\u0027s status. Should be one of active and suspended
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;
}
