namespace LogicMonitor.Api;

/// <summary>
/// Status
/// </summary>
[DataContract]
public enum Status
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Alive
	/// </summary>
	Alive = 1,

	/// <summary>
	/// Dead
	/// </summary>
	Dead = 2,

	/// <summary>
	/// Unacknowledged Alert
	/// </summary>
	[EnumMember(Value = "alert-noconfirmed")]
	AlertNoConfirmed = 3,

	/// <summary>
	/// Acknowledged Alert
	/// </summary>
	[EnumMember(Value = "alert-confirmed")]
	AlertConfirmed = 4,
}
