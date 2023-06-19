namespace LogicMonitor.Api.Websites;

/// <summary>
/// The Post data edit type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum PostDataEditType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Raw
	/// </summary>
	[EnumMember(Value = "raw")]
	Raw,

	/// <summary>
	/// x-www-form-urlencoded
	/// </summary>
	[EnumMember(Value = "x-www-form-urlencoded")]
	XWwwFormUrlEncoded,

	/// <summary>
	/// JSON
	/// </summary>
	[EnumMember(Value = "json")]
	Json
}
