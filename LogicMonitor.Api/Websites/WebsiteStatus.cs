using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// The website status
/// </summary>
public enum WebsiteStatus
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Alive
	/// </summary>
	[EnumMember(Value = "alive")]
	Alive = 1,

	/// <summary>
	/// Alive
	/// </summary>
	[EnumMember(Value = "dead")]
	Dead = 2,
}
