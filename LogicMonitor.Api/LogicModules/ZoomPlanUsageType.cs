namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The Zoom Plan Usage Type
/// </summary>
[DataContract]
[JsonConverter(typeof(TolerantStringEnumConverter))]
public enum ZoomPlanUsageType
{
	/// <summary>
	///     Unknown
	/// </summary>
	[EnumMember(Value = "0")]
	Unknown = 0,

	/// <summary>
	///     Plan base
	/// </summary>
	[EnumMember(Value = "plan_base")]
	PlanBase = 1,
}