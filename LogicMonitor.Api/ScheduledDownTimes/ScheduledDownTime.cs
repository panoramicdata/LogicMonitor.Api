namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Scheduled Down Time
/// </summary>
[DataContract]
public class ScheduledDownTime : StringIdentifiedItem, IHasEndpoint
{
	#region Basics

	/// <summary>
	/// The type resource that this SDT is for
	/// </summary>
	[DataMember(Name = "type")]
	public ScheduledDownTimeType Type { get; set; }

	/// <summary>
	/// true: the SDT is currently actice\nfalse: the SDT is currently inactive
	/// </summary>
	[DataMember(Name = "isEffective")]
	public bool IsEffective { get; set; }

	/// <summary>
	/// the type of sdt, values can be oneTime|weekly|monthly|daily|monthlyByWeek
	/// </summary>
	[DataMember(Name = "sdtType")]
	public ScheduledDownTimeRecurrenceType RecurrenceType { get; set; }

	/// <summary>
	/// The name of the user that created the SDT
	/// </summary>
	[DataMember(Name = "admin")]
	public string Admin { get; set; } = string.Empty;

	/// <summary>
	/// The notes associated with the SDT
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;

	#endregion

	#region Type-specific

	/// <summary>
	///    The collector description
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; } = string.Empty;

	/// <summary>
	///    The checkpoint id
	/// </summary>
	[DataMember(Name = "checkpointId")]
	public int? CheckpointId { get; set; }

	/// <summary>
	///    The checkpoint name
	/// </summary>
	[DataMember(Name = "checkpointName")]
	public string? CheckpointName { get; set; }

	/// <summary>
	///    The datasource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int? DataSourceId { get; set; }
	/// <summary>
	///    DeviceDataSourceId
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int? DeviceDataSourceId { get; set; }

	/// <summary>
	///    DeviceEventSourceId
	/// </summary>
	[DataMember(Name = "deviceEventSourceId")]
	public int? DeviceEventSourceId { get; set; }

	/// <summary>
	///    Device Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int? DeviceId { get; set; }

	/// <summary>
	///    Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string? DeviceDisplayName { get; set; }

	/// <summary>
	///    DataSource name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string? DataSourceName { get; set; }

	/// <summary>
	///    The DataSource instance name
	/// </summary>
	[DataMember(Name = "dataSourceInstanceName")]
	public string? DataSourceInstanceName { get; set; }

	/// <summary>
	///    EventSourceName
	/// </summary>
	[DataMember(Name = "eventSourceName")]
	public string? EventSourceName { get; set; }

	/// <summary>
	///    The Website Id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int? WebsiteId { get; set; }

	/// <summary>
	///    The website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string? WebsiteName { get; set; }

	/// <summary>
	///    Collector Id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int? CollectorId { get; set; }

	/// <summary>
	///    The dataSourceInstance Id
	/// </summary>
	[DataMember(Name = "dataSourceInstanceId")]
	public int? DataSourceInstanceId { get; set; }

	/// <summary>
	///    The deviceDataSourceInstanceGroup Id
	/// </summary>
	[DataMember(Name = "deviceDataSourceInstanceGroupId")]
	public int? DataSourceInstanceGroupId { get; set; }

	/// <summary>
	///    The deviceDataSourceInstanceGroup Name
	/// </summary>
	[DataMember(Name = "deviceDataSourceInstanceGroupName")]
	public string? DataSourceInstanceGroupName { get; set; }

	/// <summary>
	///    The Device Group Id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int? DeviceGroupId { get; set; }

	/// <summary>
	///    The DeviceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string? DeviceGroupFullPath { get; set; }

	/// <summary>
	///    The Website Group Id
	/// </summary>
	[DataMember(Name = "websiteGroupId")]
	public int? WebsiteGroupId { get; set; }

	/// <summary>
	///    The Website Group Name
	/// </summary>
	[DataMember(Name = "websiteGroupName")]
	public string? WebsiteGroupName { get; set; }

	#endregion

	#region Time-related
	/// <summary>
	/// the week day of sdt, values can be SUNDAY|MONDAY|TUESDAY|WEDNESDAY|THURSDAY|FRIDAY|SATURDAY
	/// </summary>
	[DataMember(Name = "weekDay")]
	public WeekDay WeekDay { get; set; }

	/// <summary>
	/// 1 | 2....| 31 The day of the month that the SDT will be active for a monthly SDT
	/// </summary>
	[DataMember(Name = "monthDay")]
	public int MonthDay { get; set; }

	/// <summary>
	/// 1 | 2....| 24 The hour that the SDT will start for a repeating SDT (daily, weekly, or monthly)
	/// </summary>
	[DataMember(Name = "hour")]
	public int Hour { get; set; }

	/// <summary>
	/// 1 | 2....| 60 The minute of the hour that the SDT should begin for a repeating SDT
	/// </summary>
	[DataMember(Name = "minute")]
	public int Minute { get; set; }

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
	/// The duration of the SDT in minutes
	/// </summary>
	[DataMember(Name = "duration")]
	public int DurationMinutes { get; set; }

	/// <summary>
	/// The date, time and time zone that the SDT will end at
	/// </summary>
	[DataMember(Name = "startDateTimeOnLocal")]
	public string StartDateTimeLocal { get; set; } = string.Empty;

	/// <summary>
	/// The epoch time, in milliseconds, that the SDT will start
	/// </summary>
	[DataMember(Name = "startDateTime")]
	public long StartDateTimeMs { get; set; }

	/// <summary>
	/// Start DateTime UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime StartDateTimeUtc => StartDateTimeMs.ToDateTimeUtcFromMs();

	/// <summary>
	/// The date, time and time zone that the SDT will end at
	/// </summary>
	[DataMember(Name = "endDateTimeOnLocal")]
	public string EndDateTimeLocal { get; set; } = string.Empty;

	/// <summary>
	/// The epoch time, in milliseconds, that the SDT will end
	/// </summary>
	[DataMember(Name = "endDateTime")]
	public long EndDateTimeMs { get; set; }

	/// <summary>
	/// End DateTime UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime EndDateTimeUtc => EndDateTimeMs.ToDateTimeUtcFromMs();

	/// <summary>
	/// The specific timezone for SDT
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	/// The week of the month that the SDT will be active for a monthly SDT
	/// </summary>
	[DataMember(Name = "weekOfMonth")]
	public WeekOfMonth WeekOfMonth { get; set; }

	#endregion

	/// <inheritdoc />
	public string Endpoint() => "sdt/sdts";

	/// <inheritdoc />
	public override string ToString() => $"{Type}:{Id}";
}
