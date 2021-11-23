namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// The recurrence type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum ScheduledDownTimeRecurrenceType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// One-time
	/// </summary>
	[EnumMember(Value = "oneTime")]
	OneTime = 1,

	/// <summary>
	/// Daily
	/// </summary>
	[EnumMember(Value = "daily")]
	Daily,

	/// <summary>
	/// Weekly
	/// </summary>
	[EnumMember(Value = "weekly")]
	Weekly,

	/// <summary>
	/// Monthly
	/// </summary>
	[EnumMember(Value = "monthly")]
	Monthly,

	/// <summary>
	/// Monthly
	/// </summary>
	[EnumMember(Value = "monthlyByWeek")]
	MonthlyByWeek,
}
