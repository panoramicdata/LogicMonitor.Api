namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An escalation chain period
/// </summary>
[DataContract]
public class EscalationChainPeriod
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
	/// The number of minutes into a day.
	/// e.g. midnight is 0
	/// e.g. 1 a.m. is 60
	/// </summary>
	[DataMember(Name = "startMinutes")]
	public int StartMinutes { get; set; }

	/// <summary>
	/// The number of minutes into a day.
	/// e.g. midnight is 0
	/// e.g. 1 a.m. is 60
	/// </summary>
	[DataMember(Name = "endMinutes")]
	public int EndMinutes { get; set; }

	///// <summary>
	///// The number of minutes into a day.
	///// e.g. midnight is 0
	///// e.g. 1 a.m. is 60
	///// </summary>
	//[DataMember(Name = "weekDays")]
	//public string WeekDays { get; set; }

	/// <summary>
	/// The time zone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; }

	/// <summary>
	/// The weekdays.  Sometimes, an integer array, sometimes a string in the form "1|2|3|4|5|6|7"
	/// </summary>
	[DataMember(Name = "weekDays")]
	public object WeekDays { get; set; }
}
