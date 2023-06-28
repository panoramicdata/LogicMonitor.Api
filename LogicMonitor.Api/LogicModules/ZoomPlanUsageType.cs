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

	/// <summary>
	/// PlanLargeMeeting
	/// </summary>
	[EnumMember(Value = "plan_large_meeting")]
	PlanLargeMeeting = 2,

	/// <summary>
	/// PlanZoomRooms
	/// </summary>
	[EnumMember(Value = "plan_zoom_rooms")]
	PlanZoomRooms = 3,

	/// <summary>
	/// PlanWebinar
	/// </summary>
	[EnumMember(Value = "plan_webinar")]
	PlanWebinar = 4,

	/// <summary>
	/// Subaccount
	/// </summary>
	[EnumMember(Value = "subaccount")]
	Subaccount = 5
}