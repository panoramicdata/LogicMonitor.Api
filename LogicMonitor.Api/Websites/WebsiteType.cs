namespace LogicMonitor.Api.Websites;

/// <summary>
/// A website type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum WebsiteType
{
	/// <summary>
	/// Unknown website type
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Web check website type
	/// </summary>
	[EnumMember(Value = "webcheck")]
	WebCheck,

	/// <summary>
	/// Ping website type
	/// </summary>
	[EnumMember(Value = "pingcheck")]
	Ping,

	/// <summary>
	/// Synthetic website type
	/// </summary>
	[EnumMember(Value = "synthetic")]
	Synthetic,
}
