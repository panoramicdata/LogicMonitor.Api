namespace LogicMonitor.Api.Users;

/// <summary>
/// The users status
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum UserStatus
{
	/// <summary>
	///    Active
	/// </summary>
	[EnumMember(Value = "active")]
	Active,

	/// <summary>
	///    Inactive
	/// </summary>
	[EnumMember(Value = "suspended")]
	Suspended
}