namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Scheduled Down Time
/// </summary>
[DataContract]
public abstract class ScheduledDownTimeCreationDto : CreationDto<ScheduledDownTime>
{
	/// <summary>
	/// Protected constructor
	/// </summary>
	/// <param name="type"></param>
	protected ScheduledDownTimeCreationDto(ScheduledDownTimeType type)
	{
		Type = type;
	}

	/// <summary>
	/// The type of SDT
	/// </summary>
	[DataMember(Name = "sdtType")]
	public ScheduledDownTimeRecurrenceType RecurrenceType { get; set; }

	/// <summary>
	/// The type resource that this SDT is for
	/// </summary>
	[DataMember(Name = "type")]
	public ScheduledDownTimeType Type { get; set; }

	/// <summary>
	/// The notes associated with the SDT
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;

	/// <summary>
	/// The epoch time, in milliseconds, that the SDT will start
	/// </summary>
	[DataMember(Name = "startDateTime")]
	public long StartDateTimeEpochMs { get; set; }

	/// <summary>
	/// The epoch time, in milliseconds, that the SDT will end
	/// </summary>
	[DataMember(Name = "endDateTime")]
	public long EndDateTimeEpochMs { get; set; }

	/// <summary>
	/// 1 | 2....| 31 The day of the month that the SDT will be active for a monthly SDT
	/// </summary>
	[DataMember(Name = "monthDay")]
	public int MonthDay { get; set; }

	/// <summary>
	/// the week day of sdt, values can be SUNDAY|MONDAY|TUESDAY|WEDNESDAY|THURSDAY|FRIDAY|SATURDAY
	/// </summary>
	[DataMember(Name = "weekDay")]
	public string WeekDay { get; set; } = string.Empty;

	/// <summary>
	/// 1 | 2....| 24 The hour that the SDT will start for a repeating SDT (daily, weekly, or monthly)
	/// </summary>
	[DataMember(Name = "hour")]
	public int StartHour { get; set; }

	/// <summary>
	/// 1 | 2....| 60 The minute of the hour that the SDT should begin for a repeating SDT
	/// </summary>
	[DataMember(Name = "minute")]
	public int StartMinute { get; set; }

	/// <summary>
	/// 1 | 2....| 24 The hour that the SDT ends for a repeating SDT
	/// </summary>
	[DataMember(Name = "endHour")]
	public int EndHour { get; set; }

	/// <summary>
	/// 1 | 2....| 60 The minute of the hour that the SDT ends for a repeating SDT
	/// </summary>
	[DataMember(Name = "endMinute")]
	public int EndMinute { get; set; }

	/// <summary>
	/// The specific timezone for SDT
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	/// The week of the month that the SDT will be active for a monthly SDT
	/// </summary>
	[DataMember(Name = "weekOfMonth")]
	public string WeekOfMonth { get; set; } = string.Empty;
}
