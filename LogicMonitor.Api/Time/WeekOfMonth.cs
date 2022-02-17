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
	[EnumMember(Value = "First")]
	First = 1,

	/// <summary>
	/// Two
	/// </summary>
	[EnumMember(Value = "Second")]
	Second = 2,

	/// <summary>
	/// Three
	/// </summary>
	[EnumMember(Value = "Third")]
	Third = 3,

	/// <summary>
	/// Four
	/// </summary>
	[EnumMember(Value = "Forth")]
	Fourth = 4,

	/// <summary>
	/// Five
	/// </summary>
	[EnumMember(Value = "Last")]
	Last = 5
}
