namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Consolidate function
/// </summary>
public enum ConsolidateFunction
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Mean
	/// </summary>
	[EnumMember(Value = "average")]
	Average = 1,

	/// <summary>
	/// Minimum
	/// </summary>
	[EnumMember(Value = "min")]
	Minimum,

	/// <summary>
	/// Maximum
	/// </summary>
	[EnumMember(Value = "max")]
	Maximum
}
