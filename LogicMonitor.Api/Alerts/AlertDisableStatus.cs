namespace LogicMonitor.Api.Alerts;

/// <summary>
/// Alert Status
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum AlertDisableStatus
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// None
	/// </summary>
	[EnumMember(Value = "none-none-none")]
	NoneNoneNone,

	/// <summary>
	/// None / none / disable
	/// </summary>
	[EnumMember(Value = "none-none-disable")]
	NoneNoneDisable,

	/// <summary>
	/// None / disable / none
	/// </summary>
	[EnumMember(Value = "none-disable-none")]
	NoneDisableNone,

	/// <summary>
	/// Disable / none / none
	/// </summary>
	[EnumMember(Value = "disable-none-none")]
	DisableNoneNone,

	/// <summary>
	/// None / disable / disable
	/// </summary>
	[EnumMember(Value = "none-disable-disable")]
	NoneDisableDisable,

	/// <summary>
	/// Disable / disable / none
	/// </summary>
	[EnumMember(Value = "disable-disable-none")]
	DisableDisableNone,

	/// <summary>
	/// Disable / none / disable
	/// </summary>
	[EnumMember(Value = "disable-none-disable")]
	DisableNoneDisable,

	/// <summary>
	/// Disable / disable / disable
	/// </summary>
	[EnumMember(Value = "disable-disable-disable")]
	DisableDisableDisable,
}
