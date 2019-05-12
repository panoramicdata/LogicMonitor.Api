using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{
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
		protected ScheduledDownTimeCreationDto(ScheduledDownTimeType type) => Type = type;

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
		///    The week day (used for monthlyByWeek)
		/// </summary>
		[DataMember(Name = "weekDay")]
		public string WeekDay { get; set; }

		/// <summary>
		///    The week of month (used for monthlyByWeek)
		/// </summary>
		[DataMember(Name = "weekOfMonth")]
		public string WeekOfMonth { get; set; }
	}
}