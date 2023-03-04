namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An escalation chain period
/// </summary>
[DataContract]
public class Period
{
	/// <summary>
	/// The start time
	/// </summary>
	[DataMember(Name = "start")]
	public int StartMemberMinutes { get; set; }

	/// <summary>
	/// The end time
	/// </summary>
	[DataMember(Name = "end")]
	public int EndMemberMinutes { get; set; }

	/// <summary>
	/// the start minute of this period
	/// </summary>
	[DataMember(Name = "startMinutes")]
	public int StartMinutes { get; set; }

	/// <summary>
	/// the end minute of this period
	/// </summary>
	[DataMember(Name = "endMinutes")]
	public int EndMinutes { get; set; }

	/// <summary>
	/// the timezone for this period
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	/// the list of week day of this period
	/// </summary>
	[DataMember(Name = "weekDays")]
	public object WeekDays { get; set; } = new();
}
