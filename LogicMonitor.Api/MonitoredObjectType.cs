namespace LogicMonitor.Api;

/// <summary>
/// The monitored object type
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
[DataContract]
public enum MonitoredObjectType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// A Resource
	/// </summary>
	[EnumMember(Value = "device")]
	Resource = 1,

	/// <summary>
	/// A website
	/// </summary>
	[EnumMember(Value = "website")]
	Website = 2,

	/// <summary>
	/// A service
	/// </summary>
	[EnumMember(Value = "bizService")]
	Service = 3,

	/// <summary>
	/// A group
	/// </summary>
	[EnumMember(Value = "group")]
	Group = 4,

	/// <summary>
	/// A property rule
	/// </summary>
	[EnumMember(Value = "propertyRule")]
	PropertyRule = 5,

	/// <summary>
	/// LM Logs
	/// </summary>
	[EnumMember(Value = "lmlogs")]
	LmLogs = 6
}
