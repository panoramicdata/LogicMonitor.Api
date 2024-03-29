namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A JobMonitor
/// </summary>
[DataContract]
public class JobMonitor : LogicModule, IHasEndpoint
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
	public string AlertBody { get; set; } = string.Empty;

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
	public string AlertSubject { get; set; } = string.Empty;

	/// <summary>
	/// What this applies to
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The cron schedule
	/// </summary>
	[DataMember(Name = "cronSchedule")]
	public string CronSchedule { get; set; } = string.Empty;

	/// <summary>
	/// The time zone of the cron schedule
	/// </summary>
	[DataMember(Name = "cronTimeZone")]
	public string CronTimeZone { get; set; } = string.Empty;

	/// <summary>
	/// The Group name
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// Longest run time in minutes
	/// </summary>
	[DataMember(Name = "longestRunTimeInMinute")]
	public string LongestRunTimeMinutes { get; set; } = string.Empty;

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
	public string Tags { get; set; } = string.Empty;

	/// <summary>
	/// Technology
	/// </summary>
	[DataMember(Name = "technology")]
	public string Technology { get; set; } = string.Empty;

	/// <summary>
	/// ToString override
	/// </summary>
	/// <returns>'Id : Name'</returns>
	public override string ToString() => $"{Id} : {Name}";

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/batchjobs";
}
