using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts;

/// <summary>
/// The SDT Status
/// </summary>
public enum SdtStatus
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
	/// None / none / SDT
	/// </summary>
	[EnumMember(Value = "none-none-SDT")]
	NoneNoneSdt,

	/// <summary>
	/// None / SDT / none
	/// </summary>
	[EnumMember(Value = "none-SDT-none")]
	NoneSdtNone,

	/// <summary>
	/// SDT / none / none
	/// </summary>
	[EnumMember(Value = "SDT-none-none")]
	SdtNoneNone,

	/// <summary>
	/// None / SDT / SDT
	/// </summary>
	[EnumMember(Value = "none-SDT-SDT")]
	NoneSdtSdt,

	/// <summary>
	/// Sdt / SDT / none
	/// </summary>
	[EnumMember(Value = "SDT-SDT-none")]
	SdtSdtNone,

	/// <summary>
	/// Sdt / none / SDT
	/// </summary>
	[EnumMember(Value = "SDT-none-SDT")]
	SdtNoneSdt,

	/// <summary>
	/// SDT / SDT / SDT
	/// </summary>
	[EnumMember(Value = "SDT-SDT-SDT")]
	SdtSdtSdt,
}
