using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts;

/// <summary>
/// From http://www.logicmonitor.com/support/rpc-api-developers-guide/manage-alerts/get-alerts/
/// If timing=start, only alerts that started between startEpoch and endEpoch will be returned.
/// If timing=overlap, any alert that was active during startEpoch and endEpoch will be returned.
/// Note that it is not necessary to specify both startEpoch and endEpoch.
/// </summary>
public enum Timing
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// any alert that was active during startEpoch and endEpoch will be returned
	/// </summary>
	Overlap = 1,

	/// <summary>
	/// only alerts that started between startEpoch and endEpoch will be returned
	/// </summary>
	Start = 2,
}
