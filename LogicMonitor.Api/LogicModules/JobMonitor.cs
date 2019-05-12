using LogicMonitor.Api.Alerts;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A JobMonitor
	/// </summary>
	[DataContract]
	public class JobMonitor : NamedItem, IHasEndpoint
	{
		/// <summary>
		/// Whether it is active monitoring
		/// </summary>
		[DataMember(Name = "activeMonitoring")]
		public bool ActiveMonitoring { get; set; }

		/// <summary>
		/// The alert message's body
		/// </summary>
		[DataMember(Name = "alertBody")]
		public string AlertBody { get; set; }

		/// <summary>
		/// The Alert effective interval
		/// </summary>
		[DataMember(Name = "alertEffectiveIval")]
		public int AlertEffectiveInterval { get; set; }

		/// <summary>
		/// The alert level
		/// </summary>
		[DataMember(Name = "alertLevel")]
		public AlertLevel AlertLevel { get; set; }

		/// <summary>
		/// The alert message's body
		/// </summary>
		[DataMember(Name = "alertSubject")]
		public string AlertSubject { get; set; }

		/// <summary>
		/// What this applies to
		/// </summary>
		[DataMember(Name = "appliesTo")]
		public string AppliesTo { get; set; }

		/// <summary>
		/// The cron schedule
		/// </summary>
		[DataMember(Name = "cronSchedule")]
		public string CronSchedule { get; set; }

		/// <summary>
		/// The time zone of the cron schedule
		/// </summary>
		[DataMember(Name = "cronTimeZone")]
		public string CronTimeZone { get; set; }

		/// <summary>
		/// The Group name
		/// </summary>
		[DataMember(Name = "group")]
		public string Group { get; set; }

		/// <summary>
		/// Longest run time in minutes
		/// </summary>
		[DataMember(Name = "longestRunTimeInMinute")]
		public string LongestRunTimeMinutes { get; set; }

		/// <summary>
		/// Published
		/// </summary>
		[DataMember(Name = "published")]
		public int Published { get; set; }

		/// <summary>
		/// The maximum relative time interval
		/// </summary>
		[DataMember(Name = "startMrtie")]
		public int MaximumRelativeTimeIntervalError { get; set; }

		/// <summary>
		/// Tags
		/// </summary>
		[DataMember(Name = "tags")]
		public string Tags { get; set; }

		/// <summary>
		/// Technology
		/// </summary>
		[DataMember(Name = "technology")]
		public string Technology { get; set; }

		/// <summary>
		/// ToString override
		/// </summary>
		/// <returns>'Id : Name'</returns>
		public override string ToString() => $"{Id} : {Name}";

		/// <summary>
		///    The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "setting/batchjobs";
	}
}