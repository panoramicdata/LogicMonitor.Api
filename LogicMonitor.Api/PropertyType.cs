namespace LogicMonitor.Api;

/// <summary>
///    A device property type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum PropertyType : byte
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    Custom
	/// </summary>
	[EnumMember(Value = "custom")] Custom,

	/// <summary>
	///    System
	/// </summary>
	[EnumMember(Value = "system")] System,

	/// <summary>
	///    Inherit
	/// </summary>
	[EnumMember(Value = "inherit")] Inherit,

	/// <summary>
	///    Auto
	/// </summary>
	[EnumMember(Value = "auto")] Auto,

	/// <summary>
	///    Owned
	/// </summary>
	[EnumMember(Value = "owned")] Owned
}
