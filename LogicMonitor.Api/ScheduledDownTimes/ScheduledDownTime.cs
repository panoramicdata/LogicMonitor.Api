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
	[DataMember(Name = "type", IsRequired = true)]
	public ScheduledDownTimeType Type { get; set; }

	/// <summary>
	/// true: the SDT is currently actice\nfalse: the SDT is currently inactive
	/// </summary>
	[DataMember(Name = "isEffective", IsRequired = false)]
	public bool IsEffective { get; set; }

	/// <summary>
	/// the type of sdt, values can be oneTime|weekly|monthly|daily|monthlyByWeek
	/// </summary>
	[DataMember(Name = "sdtType", IsRequired = false)]
	public string RecurrenceType { get; set; } = string.Empty;

	/// <summary>
	/// The name of the user that created the SDT
	/// </summary>
	[DataMember(Name = "admin", IsRequired = false)]
	public string Admin { get; set; } = string.Empty;

	/// <summary>
	/// The notes associated with the SDT
	/// </summary>
	[DataMember(Name = "comment", IsRequired = false)]
	public string Comment { get; set; } = string.Empty;

	#endregion

	#region Type-specific

	/// <summary>
	///    The collector description
	/// </summary>
	[DataMember(Name = "collectorDescription", IsRequired = false)]
	public string CollectorDescription { get; set; } = string.Empty;

	/// <summary>
	///    The checkpoint id
	/// </summary>
	[DataMember(Name = "checkpointId", IsRequired = false)]
	public int? CheckpointId { get; set; }

	/// <summary>
	///    The checkpoint name
	/// </summary>
	[DataMember(Name = "checkpointName", IsRequired = false)]
	public string CheckpointName { get; set; } = string.Empty;

	/// <summary>
	///    The datasource Id
	/// </summary>
	[DataMember(Name = "dataSourceId", IsRequired = false)]
	public int? DataSourceId { get; set; }
	/// <summary>
	///    DeviceDataSourceId
	/// </summary>
	[DataMember(Name = "deviceDataSourceId", IsRequired = false)]
	public int? DeviceDataSourceId { get; set; }

	/// <summary>
	///    DeviceEventSourceId
	/// </summary>
	[DataMember(Name = "deviceEventSourceId", IsRequired = false)]
	public int DeviceEventSourceId { get; set; }

	/// <summary>
	///    Device Id
	/// </summary>
	[DataMember(Name = "deviceId", IsRequired = false)]
	public int DeviceId { get; set; }

	/// <summary>
	///    Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName", IsRequired = false)]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///    DataSource name
	/// </summary>
	[DataMember(Name = "dataSourceName", IsRequired = false)]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	///    The DataSource instance name
	/// </summary>
	[DataMember(Name = "dataSourceInstanceName", IsRequired = false)]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	///    EventSourceName
	/// </summary>
	[DataMember(Name = "eventSourceName", IsRequired = false)]
	public string EventSourceName { get; set; } = string.Empty;

	/// <summary>
	///    The Website Id
	/// </summary>
	[DataMember(Name = "websiteId", IsRequired = false)]
	public int? WebsiteId { get; set; }

	/// <summary>
	///    The website name
	/// </summary>
	[DataMember(Name = "websiteName", IsRequired = false)]
	public string WebsiteName { get; set; } = string.Empty;

	/// <summary>
	///    Collector Id
	/// </summary>
	[DataMember(Name = "collectorId", IsRequired = false)]
	public int? CollectorId { get; set; }
	/// <summary>
	///    The dataSourceInstance Id
	/// </summary>
	[DataMember(Name = "dataSourceInstanceId", IsRequired = false)]
	public int? DataSourceInstanceId { get; set; }

	/// <summary>
	///    The deviceDataSourceInstanceGroup Id
	/// </summary>
	[DataMember(Name = "deviceDataSourceInstanceGroupId", IsRequired = false)]
	public int? DataSourceInstanceGroupId { get; set; }

	/// <summary>
	///    The deviceDataSourceInstanceGroup Name
	/// </summary>
	[DataMember(Name = "deviceDataSourceInstanceGroupName", IsRequired = false)]
	public string DataSourceInstanceGroupName { get; set; } = string.Empty;

	/// <summary>
	///    The Device Group Id
	/// </summary>
	[DataMember(Name = "deviceGroupId", IsRequired = false)]
	public int? DeviceGroupId { get; set; }

	/// <summary>
	///    The DeviceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath", IsRequired = false)]
	public string DeviceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///    The Website Group Id
	/// </summary>
	[DataMember(Name = "websiteGroupId", IsRequired = false)]
	public int? WebsiteGroupId { get; set; }

	/// <summary>
	///    The Website Group Name
	/// </summary>
	[DataMember(Name = "websiteGroupName", IsRequired = false)]
	public string WebsiteGroupName { get; set; } = string.Empty;

	#endregion

	#region Time-related
	/// <summary>
	/// the week day of sdt, values can be SUNDAY|MONDAY|TUESDAY|WEDNESDAY|THURSDAY|FRIDAY|SATURDAY
	/// </summary>
	[DataMember(Name = "weekDay")]
	public string WeekDay { get; set; } = string.Empty;

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
	[DataMember(Name = "endDateTimeOnLocal", IsRequired = false)]
	public string EndDateTimeLocal { get; set; } = string.Empty;

	/// <summary>
	/// The epoch time, in milliseconds, that the SDT will end
	/// </summary>
	[DataMember(Name = "endDateTime", IsRequired = false)]
	public long EndDateTimeMs { get; set; }

	/// <summary>
	/// End DateTime UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime EndDateTimeUtc => EndDateTimeMs.ToDateTimeUtcFromMs();

	/// <summary>
	/// The specific timezone for SDT
	/// </summary>
	[DataMember(Name = "timezone", IsRequired = false)]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	/// The week of the month that the SDT will be active for a monthly SDT
	/// </summary>
	[DataMember(Name = "weekOfMonth", IsRequired = false)]
	public string WeekOfMonth { get; set; } = string.Empty;

	#endregion

	/// <inheritdoc />
	public string Endpoint() => "sdt/sdts";

	/// <inheritdoc />
	public override string ToString() => $"{Type}:{Id}";
}
