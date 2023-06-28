namespace LogicMonitor.Api.Alerts;

/// <summary>
///     The destination type
/// </summary>
[DataContract]
public enum DestinationType
{
	/// <summary>
	///     Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///     Simple
	/// </summary>
	[EnumMember(Value = "simple")]
	Simple = 1,

	/// <summary>
	///     Single
	/// </summary>
	[EnumMember(Value = "single")]
	SingleDest = 2,

	/// <summary>
	///     Time-based
	/// </summary>
	[EnumMember(Value = "timebased")]
	TimeBased = 3,

	/// <summary>
	///     Group
	/// </summary>
	[EnumMember(Value = "GROUP")]
	Group = 4,

	/// <summary>
	///     Group
	/// </summary>
	[EnumMember(Value = "ARBITRARY")]
	Arbitrary = 5,

	/// <summary>
	///     Admin
	/// </summary>
	[EnumMember(Value = "admin")]
	Admin = 6,

	/// <summary>
	///     Admin
	/// </summary>
	[EnumMember(Value = "ADMIN")]
	AdminCapitalised = Admin
}
