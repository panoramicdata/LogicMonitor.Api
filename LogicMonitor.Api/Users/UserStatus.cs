namespace LogicMonitor.Api.Users;

/// <summary>
/// The users status
/// </summary>
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