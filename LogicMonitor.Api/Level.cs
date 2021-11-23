namespace LogicMonitor.Api;

/// <summary>
///    Alert level
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum Level : byte
{
	/// <summary>
	///    All levels
	/// </summary>
	[EnumMember(Value = "all")]
	All = 0,

	/// <summary>
	///    Warning level
	/// </summary>
	[EnumMember(Value = "warn")]
	Warning = 1,

	/// <summary>
	///    Error level
	/// </summary>
	[EnumMember(Value = "error")]
	Error = 2,

	/// <summary>
	///    Critical level
	/// </summary>
	[EnumMember(Value = "critical")]
	Critical = 3,

	/// <summary>
	///    Dead level
	/// </summary>
	[EnumMember(Value = "dead")]
	Dead = 4,

	/// <summary>
	///    Dead level
	/// </summary>
	[EnumMember(Value = "dead-collector")]
	DeadCollector = 5,

	/// <summary>
	///    Alive level
	/// </summary>
	[EnumMember(Value = "alive")]
	Alive = 6,

	/// <summary>
	///    Normal
	/// </summary>
	[EnumMember(Value = "normal")]
	Normal = 100
}
