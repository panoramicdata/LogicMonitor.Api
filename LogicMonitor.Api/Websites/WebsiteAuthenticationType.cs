using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// Authentication type
/// </summary>
public enum WebsiteAuthenticationType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Common
	/// </summary>
	Common = 1,
}
