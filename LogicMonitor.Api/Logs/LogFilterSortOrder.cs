using System.Runtime.Serialization;

namespace LogicMonitor.Api.Logs;

/// <summary>
/// A log filter sort order
/// </summary>
[DataContract]
public enum LogFilterSortOrder
{
	/// <summary>
	/// Unknown / not set
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// In ascending order by when it happened
	/// </summary>
	[EnumMember(Value = "happenedOn")]
	HappenedOnAsc = 1,

	/// <summary>
	/// In descending order by when it happened
	/// </summary>
	[EnumMember(Value = "-happenedOn")]
	HappenedOnDesc = 2,
}
