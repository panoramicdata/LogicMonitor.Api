using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// The mtachType
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
	Regular = 3
}
