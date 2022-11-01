namespace LogicMonitor.Api.Websites;

/// <summary>
/// The matchType
/// </summary>
public enum MatchType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Plain
	/// </summary>
	[EnumMember(Value = "plain")]
	Plain = 1,

	/// <summary>
	/// JSON
	/// </summary>
	[EnumMember(Value = "json")]
	Json = 2,

	/// <summary>
	/// Regular
	/// </summary>
	[EnumMember(Value = "regular")]
	Regular = 3,

	/// <summary>
	/// Wildcard
	/// </summary>
	[EnumMember(Value = "wildcard")]
	Wildcard = 4,

	/// <summary>
	/// XPath
	/// </summary>
	[EnumMember(Value = "xpath")]
	Xpath = 5
}
