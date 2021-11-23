using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings;

/// <summary>
/// An API token status
/// </summary>
[DataContract]
public enum ApiTokenStatus
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Disabled
	/// </summary>
	Disabled = 1,

	/// <summary>
	/// Enabled
	/// </summary>
	Enabled = 2
}
