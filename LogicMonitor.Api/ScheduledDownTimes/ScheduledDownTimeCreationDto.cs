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
	///    Recurrence type
	/// </summary>
	[DataMember(Name = "sdtType")]
	public ScheduledDownTimeRecurrenceType RecurrenceType { get; set; }

	/// <summary>
	///    Type
	/// </summary>
	[DataMember(Name = "type")]
	public ScheduledDownTimeType Type { get; set; }

	/// <summary>
	///    Comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; }

	/// <summary>
	///    Start date time milliseconds since the Epoch
	/// </summary>
	[DataMember(Name = "startDateTime")]
	public long StartDateTimeEpochMs { get; set; }

	/// <summary>
	///    End date time milliseconds since the Epoch
	/// </summary>
	[DataMember(Name = "endDateTime")]
	public long EndDateTimeEpochMs { get; set; }

	/// <summary>
	///    The month day (used for monthly)
	/// </summary>
	[DataMember(Name = "monthDay")]
	public int MonthDay { get; set; }

	/// <summary>
	///    The week day (used for monthlyByWeek)
	/// </summary>
	[DataMember(Name = "weekDay")]
	public WeekDay WeekDay { get; set; }

	/// <summary>
	///    The start hour
	/// </summary>
	[DataMember(Name = "hour")]
	public int StartHour { get; set; }

	/// <summary>
	///    The start minute
	/// </summary>
	[DataMember(Name = "minute")]
	public int StartMinute { get; set; }

	/// <summary>
	///    The end hour
	/// </summary>
	[DataMember(Name = "endHour")]
	public int EndHour { get; set; }

	/// <summary>
	///    The end minute
	/// </summary>
	[DataMember(Name = "endMinute")]
	public int EndMinute { get; set; }

	/// <summary>
	///    The time zone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; }

	/// <summary>
	///    The week of month (used for monthlyByWeek)
	/// </summary>
	[DataMember(Name = "weekOfMonth")]
	public string WeekOfMonth { get; set; }
}
