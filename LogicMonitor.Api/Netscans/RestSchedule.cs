namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A netscan policy schedule
/// </summary>
[DataContract(Name = "schedule")]
public class RestSchedule
{
	/// <summary>
	///    The notify
	/// </summary>
	[DataMember(Name = "notify")]
	public bool Notify { get; set; }

	/// <summary>
	///    The Type
	/// </summary>
	[DataMember(Name = "type")]
	public NetscanScheduleType Type { get; set; }

	/// <summary>
	///    The recipients
	/// </summary>
	[DataMember(Name = "recipients")]
	public List<object> Recipients { get; set; } = new();

	/// <summary>
	///    The cron
	/// </summary>
	[DataMember(Name = "cron")]
	public string Cron { get; set; } = string.Empty;

	/// <summary>
	///    The weekday
	/// </summary>
	[DataMember(Name = "weekday")]
	public List<string> WeekDays { get; set; } = new();

	/// <summary>
	///    The nth week
	/// </summary>
	[DataMember(Name = "nthweek")]
	public string NthWeek { get; set; } = string.Empty;

	/// <summary>
	///    The monthday
	/// </summary>
	[DataMember(Name = "monthday")]
	public string Monthday { get; set; } = string.Empty;

	/// <summary>
	/// The time zone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;
}
