namespace LogicMonitor.Api.Settings;

/// <summary>
/// API Token
/// </summary>
[DataContract]
public class ApiToken : IdentifiedItem
{
	/// <summary>
	/// The access Id associated with the API Tokens
	/// </summary>
	[DataMember(Name = "accessId")]
	public string? AccessId { get; set; }

	/// <summary>
	/// The secret key associated with the API Tokens
	/// </summary>
	[DataMember(Name = "accessKey")]
	public string? AccessKey { get; set; }

	/// <summary>
	/// The id of the user associated with the API Tokens
	/// </summary>
	[DataMember(Name = "adminId")]
	public int UserId { get; set; }

	/// <summary>
	/// The name of the user associated with the API Tokens
	/// </summary>
	[DataMember(Name = "adminName")]
	public string? UserName { get; set; }

	/// <summary>
	/// The roles assigned to the user that is associated with the API Tokens
	/// </summary>
	[DataMember(Name = "roles")]
	public List<string>? Roles { get; set; }

	/// <summary>
	/// 1 | 2 - Whether or not the API Tokens are enabled, where 2 \u003d enabled
	/// </summary>
	[DataMember(Name = "status")]
	public ApiTokenStatus Status { get; set; }

	/// <summary>
	/// The type.  If null, V1
	/// </summary>
	[DataMember(Name = "type")]
	public ApiTokenType? Type { get; set; }

	/// <summary>
	/// The note associated with the API Tokens
	/// </summary>
	[DataMember(Name = "note")]
	public string? Note { get; set; }

	/// <summary>
	/// The user who is the API Tokens created by
	/// </summary>
	[DataMember(Name = "createdBy")]
	public string? CreatedBy { get; set; }

	/// <summary>
	/// The epoch at which the API Tokens were created
	/// </summary>
	[DataMember(Name = "createdOn")]
	public long CreatedOnSeconds { get; set; }

	/// <summary>
	/// The epoch at which the API Tokens were last used
	/// </summary>
	[DataMember(Name = "lastUsedOn")]
	public long LastUsedOnSeconds { get; set; }

	/// <summary>
	/// The permission of current apiToken with the admin. values can be write|read|none
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermissionValues? UserPermission { get; set; }

	/// <summary>
	///    The DateTime the user accepted the EULA UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime CreatedOnUtc => CreatedOnSeconds.ToDateTimeUtc();

	/// <summary>
	///    The last user action time in UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime? LastActionOnUtc => LastUsedOnSeconds.ToNullableDateTimeUtc();
}
