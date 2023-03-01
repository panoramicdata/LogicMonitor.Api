namespace LogicMonitor.Api;

/// <summary>
///     User Permission
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum UserPermissionValues
{
	/// <summary>
	///     Unknown
	/// </summary>
	[EnumMember(Value = "")]
	Unknown,

	/// <summary>
	///     None
	/// </summary>
	[EnumMember(Value = "none")]
	None,

	/// <summary>
	///     Read
	/// </summary>
	[EnumMember(Value = "read")]
	Read,

	/// <summary>
	///     Write
	/// </summary>
	[EnumMember(Value = "write")]
	Write,

	/// <summary>
	///     Visit (Read-only)
	/// </summary>
	[EnumMember(Value = "visit")]
	Visit,

	/// <summary>
	///     Exec
	/// </summary>
	[EnumMember(Value = "exec")]
	Execute
}
