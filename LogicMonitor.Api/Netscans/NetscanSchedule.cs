using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Netscans
{
	/// <summary>
	///    A netscan policy schedule
	/// </summary>
	[DataContract(Name = "schedule")]
	public class NetscanSchedule
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
		public List<object> Recipients { get; set; }

		/// <summary>
		///    The cron
		/// </summary>
		[DataMember(Name = "cron")]
		public string Cron { get; set; }

		/// <summary>
		///    The weekday
		/// </summary>
		[DataMember(Name = "weekday")]
		public List<string> WeekDays { get; set; }

		/// <summary>
		///    The nth week
		/// </summary>
		[DataMember(Name = "nthweek")]
		public string NthWeek { get; set; }

		/// <summary>
		///    The monthday
		/// </summary>
		[DataMember(Name = "monthday")]
		public string Monthday { get; set; }

		/// <summary>
		/// The time zone
		/// </summary>
		[DataMember(Name = "timezone")]
		public string TimeZone { get; set; }
	}
}