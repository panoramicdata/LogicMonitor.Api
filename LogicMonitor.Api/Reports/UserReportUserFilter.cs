namespace LogicMonitor.Api.Reports;

/// <summary>
/// User report user filter
/// </summary>
[DataContract]
public class UserReportUserFilter
{
	/// <summary>
	/// The status
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;

	/// <summary>
	/// The enable2fa
	/// </summary>
	[DataMember(Name = "enable2fa")]
	public string Enable2fa { get; set; } = string.Empty;

	/// <summary>
	/// The apiOnlyUser
	/// </summary>
	[DataMember(Name = "apiOnlyUser")]
	public string ApiOnlyUser { get; set; } = string.Empty;

	/// <summary>
	/// The roleAssignment
	/// </summary>
	[DataMember(Name = "roleAssignment")]
	public string RoleAssignment { get; set; } = string.Empty;

	/// <summary>
	/// The firstName
	/// </summary>
	[DataMember(Name = "firstName")]
	public string FirstName { get; set; } = string.Empty;

	/// <summary>
	/// The lastName
	/// </summary>
	[DataMember(Name = "lastName")]
	public string LastName { get; set; } = string.Empty;

	/// <summary>
	/// The email
	/// </summary>
	[DataMember(Name = "email")]
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// The username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; } = string.Empty;

	/// <summary>
	/// The email verification
	/// </summary>
	[DataMember(Name = "emailVerification")]
	public string EmailVerification { get; set; } = string.Empty;

	/// <summary>
	/// The 2FA
	/// </summary>
	[DataMember(Name = "2FA")]
	public string TwoFactorAuthentication { get; set; } = string.Empty;

	/// <summary>
	/// The 2FA Passkey
	/// </summary>
	[DataMember(Name = "2FA Passkey")]
	public string TwoFactorAuthenticationPasskey { get; set; } = string.Empty;
}
