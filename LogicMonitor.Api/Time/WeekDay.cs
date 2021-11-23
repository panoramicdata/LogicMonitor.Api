using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Time;

/// <summary>
/// A day of the week
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum WeekDay
{
	/// <summary>
	/// Sunday
	/// </summary>
	[EnumMember(Value = "Sunday")]
	Sunday = 1,

	/// <summary>
	/// Monday
	/// </summary>
	[EnumMember(Value = "Monday")]
	Monday = 2,

	/// <summary>
	/// Tuesday
	/// </summary>
	[EnumMember(Value = "Tuesday")]
	Tuesday = 3,

	/// <summary>
	/// Wednesday
	/// </summary>
	[EnumMember(Value = "Wednesday")]
	Wednesday = 4,

	/// <summary>
	/// Thursday
	/// </summary>
	[EnumMember(Value = "Thursday")]
	Thursday = 5,

	/// <summary>
	/// Friday
	/// </summary>
	[EnumMember(Value = "Friday")]
	Friday = 6,

	/// <summary>
	/// Saturday
	/// </summary>
	[EnumMember(Value = "Saturday")]
	Saturday = 7,
}
