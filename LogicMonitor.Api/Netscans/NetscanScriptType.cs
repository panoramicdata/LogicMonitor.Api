namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A netscan policy script type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum NetscanScriptType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    Embedded
	/// </summary>
	[EnumMember(Value = "embeded")] // Typo intentional
	Embedded = 1,

	/// <summary>
	///    External
	/// </summary>
	[EnumMember(Value = "external")]
	External = 2
}
