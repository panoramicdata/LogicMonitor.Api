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
	[DataMember(Name = "lastName", IsRequired = false)]
	public string? LastName { get; set; }

	/// <summary>
	/// Any notes assocaited with the user
	/// </summary>
	[DataMember(Name = "note", IsRequired = false)]
	public string? Note { get; set; }

	/// <summary>
	/// The account tabs that will be visible to the user
	/// </summary>
	[DataMember(Name = "viewPermission", IsRequired = false)]
	public string? ViewPermission { get; set; }

	/// <summary>
	/// The timezone of the user
	/// </summary>
	[DataMember(Name = "timezone", IsRequired = false)]
	public string? Timezone { get; set; }

	/// <summary>
	/// The roles assigned to the user
	/// </summary>
	[DataMember(Name = "roles", IsRequired = true)]
	public List<Role> Roles { get; set; } = null!;

	/// <summary>
	/// The time that the user last logged in, in epoch format
	/// </summary>
	[DataMember(Name = "lastLoginOn", IsRequired = false)]
	public long LastLoginOn { get; set; }

	/// <summary>
	/// The time, in local format, of the user\u0027s last action
	/// </summary>
	[DataMember(Name = "lastActionOnLocal", IsRequired = false)]
	public string? LastActionOnLocal { get; set; }

	/// <summary>
	/// sms | fullText, where sms \u003d 160 characters and fullText\u003d all characters
	/// </summary>
	[DataMember(Name = "smsEmailFormat", IsRequired = false)]
	public string? SmsEmailFormat { get; set; }

	/// <summary>
	/// Whether it is a API only user
	/// </summary>
	[DataMember(Name = "apionly", IsRequired = false)]
	public bool Apionly { get; set; }

	/// <summary>
	/// Any API Tokens associated with the user
	/// </summary>
	[DataMember(Name = "apiTokens", IsRequired = false)]
	public List<ApiToken>? ApiTokens { get; set; }

	/// <summary>
	/// The Id(s) of the groups the admin is in, where multiple group ids are comma separated
	/// </summary>
	[DataMember(Name = "adminGroupIds", IsRequired = false)]
	public List<int>? AdminGroupIds { get; set; }

	/// <summary>
	/// The password associated with the user
	/// </summary>
	[DataMember(Name = "password", IsRequired = true)]
	public string Password { get; set; } = null!;

	/// <summary>
	/// The last action taken by the user
	/// </summary>
	[DataMember(Name = "lastAction", IsRequired = false)]
	public string? LastAction { get; set; }

	/// <summary>
	/// The email address for user\u0027s Training account
	/// </summary>
	[DataMember(Name = "trainingEmail", IsRequired = false)]
	public string? TrainingEmail { get; set; }

	/// <summary>
	/// The time, in epoch format, of the user\u0027s last action
	/// </summary>
	[DataMember(Name = "lastActionOn", IsRequired = false)]
	public long LastActionOn { get; set; }

	/// <summary>
	/// The email address associated with the user
	/// </summary>
	[DataMember(Name = "email", IsRequired = false)]
	public string Email { get; set; } = null!;

	/// <summary>
	/// email | smsemail
	/// </summary>
	[DataMember(Name = "contactMethod", IsRequired = false)]
	public string? ContactMethod { get; set; }

	/// <summary>
	/// The time, in epoch format, that the user accepted the EULA (if required to)
	/// </summary>
	[DataMember(Name = "acceptEULAOn", IsRequired = false)]
	public long AcceptEULAOn { get; set; }

	/// <summary>
	/// The permission of current user with the admin. values can be write|read|none
	/// </summary>
	[DataMember(Name = "userPermission", IsRequired = false)]
	public string? UserPermission { get; set; }

	/// <summary>
	/// The sms email address associated with the user
	/// </summary>
	[DataMember(Name = "smsEmail", IsRequired = false)]
	public string? SmsEmail { get; set; }

	/// <summary>
	/// Whether or not two factor authentication is enabled for the user
	/// </summary>
	[DataMember(Name = "twoFAEnabled", IsRequired = false)]
	public bool TwoFAEnabled { get; set; }

	/// <summary>
	/// The first name associated with the user
	/// </summary>
	[DataMember(Name = "firstName", IsRequired = false)]
	public string? FirstName { get; set; }

	/// <summary>
	/// The phone number associated with the user
	/// </summary>
	[DataMember(Name = "phone", IsRequired = false)]
	public string? Phone { get; set; }

	/// <summary>
	/// Who created the user. This may be another user, SAML or LogicMonitor
	/// </summary>
	[DataMember(Name = "createdBy", IsRequired = false)]
	public string? CreatedBy { get; set; }

	/// <summary>
	/// Whether or not the user should be forced to change their password on the next login
	/// </summary>
	[DataMember(Name = "forcePasswordChange", IsRequired = false)]
	public string? ForcePasswordChange { get; set; }

	/// <summary>
	/// The tenant id of the user
	/// </summary>
	[DataMember(Name = "tenantId", IsRequired = false)]
	public int TenantId { get; set; }

	/// <summary>
	/// Whether or not the user is required to accept the EULA (end user license agreement)
	/// </summary>
	[DataMember(Name = "acceptEULA", IsRequired = false)]
	public bool AcceptEULA { get; set; }

	/// <summary>
	/// The username associated with the user
	/// </summary>
	[DataMember(Name = "username", IsRequired = true)]
	public string Username { get; set; } = null!;

	/// <summary>
	/// The user\u0027s status. Should be one of active and suspended
	/// </summary>
	[DataMember(Name = "status", IsRequired = false)]
	public string? Status { get; set; }
}
