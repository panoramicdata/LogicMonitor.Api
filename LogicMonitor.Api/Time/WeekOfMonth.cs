namespace LogicMonitor.Api.Time;

/// <summary>
/// A day of the week
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum WeekOfMonth
{
	/// <summary>
	/// None
	/// </summary>
	[EnumMember(Value = "None")]
	None = 0,

	/// <summary>
	/// One
	/// </summary>
	[EnumMember(Value = "1")]
	One = 1,

	/// <summary>
	/// Two
	/// </summary>
	[EnumMember(Value = "2")]
	Two = 2,

	/// <summary>
	/// Three
	/// </summary>
	[EnumMember(Value = "3")]
	Three = 3,

	/// <summary>
	/// Four
	/// </summary>
	[EnumMember(Value = "4")]
	Four = 4,

	/// <summary>
	/// Five
	/// </summary>
	[EnumMember(Value = "5")]
	Five = 5
}
