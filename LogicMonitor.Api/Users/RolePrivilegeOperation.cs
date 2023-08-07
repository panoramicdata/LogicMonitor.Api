namespace LogicMonitor.Api.Users;

/// <summary>
/// A role privilege operation
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum RolePrivilegeOperation
{
	/// <summary>
	/// Read
	/// </summary>
	[EnumMember(Value = "read")]
	Read,

	/// <summary>
	/// Write
	/// </summary>
	[EnumMember(Value = "write")]
	Write,

	/// <summary>
	/// Ack
	/// </summary>
	[EnumMember(Value = "ack")]
	Ack,

	/// <summary>
	/// None
	/// </summary>
	[EnumMember(Value = "none")]
	None,

	/// <summary>
	/// SDT
	/// </summary>
	[EnumMember(Value = "sdt")]
	Sdt,

	/// <summary>
	/// Threshold
	/// </summary>
	[EnumMember(Value = "threshold")]
	Threshold,

	/// <summary>
	/// Publish
	/// </summary>
	[EnumMember(Value = "publish")]
	Publish,

	/// <summary>
	/// NoPrivilege
	/// </summary>
	[EnumMember(Value = "noprivilege")]
	NoPrivilege
}
