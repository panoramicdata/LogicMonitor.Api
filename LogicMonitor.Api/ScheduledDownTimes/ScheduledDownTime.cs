namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Scheduled Down Time
/// </summary>
[DataContract]
public class ScheduledDownTime : StringIdentifiedItem, IHasEndpoint
{
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
	///    The EventSource Name
	/// </summary>
	[DataMember(Name = "eventSourceName")]
	public string? EventSourceName { get; set; }

	/// <summary>
	///    ScheduledDownTime type
	/// </summary>
	[DataMember(Name = "sdtType")]
	public ScheduledDownTimeRecurrenceType RecurrenceType { get; set; }

	/// <summary>
	///    Admin
	/// </summary>
	[DataMember(Name = "admin")]
	public string Admin { get; set; } = string.Empty;

	/// <summary>
	///    Comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;

	/// <summary>
	///    The collector description
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string? CollectorDescription { get; set; }

	/// <summary>
	///    The Device Group Id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int? DeviceGroupId { get; set; }

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

	/// <summary>
	///    The DeviceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string? DeviceGroupFullPath { get; set; }

	/// <summary>
	///    Weekday
	/// </summary>
	[DataMember(Name = "weekDay")]
	public WeekDay? WeekDay { get; set; }

	/// <summary>
	///    Month day
	/// </summary>
	[DataMember(Name = "monthDay")]
	public int? MonthDay { get; set; }

	/// <summary>
	///    Hour
	/// </summary>
	[DataMember(Name = "hour")]
	public int Hour { get; set; }

	/// <summary>
	///    Minute
	/// </summary>
	[DataMember(Name = "minute")]
	public int Minute { get; set; }

	/// <summary>
	///    End Hour
	/// </summary>
	[DataMember(Name = "endHour")]
	public int EndHour { get; set; }

	/// <summary>
	///    End Minute
	/// </summary>
	[DataMember(Name = "endMinute")]
	public int? EndMinute { get; set; }

	/// <summary>
	///    Duration in minutes
	/// </summary>
	[DataMember(Name = "duration")]
	public int? DurationMinutes { get; set; }

	/// <summary>
	///    Start date time local
	/// </summary>
	[DataMember(Name = "startDateTimeOnLocal")]
	public string StartDateTimeLocal { get; set; } = string.Empty;

	/// <summary>
	///    Start date time local
	/// </summary>
	[DataMember(Name = "startDateTime")]
	public long? StartDateTimeMs { get; set; }

	/// <summary>
	/// Start DateTime UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime? StartDateTimeUtc => StartDateTimeMs?.ToDateTimeUtcFromMs();

	/// <summary>
	///    End date time local
	/// </summary>
	[DataMember(Name = "endDateTimeOnLocal")]
	public string? EndDateTimeLocal { get; set; }

	/// <summary>
	///    End date time local
	/// </summary>
	[DataMember(Name = "endDateTime")]
	public long? EndDateTimeMs { get; set; }

	/// <summary>
	/// End DateTime UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime? EndDateTimeUtc => EndDateTimeMs?.ToDateTimeUtcFromMs();

	/// <summary>
	///    Weekday
	/// </summary>
	[DataMember(Name = "isEffective")]
	public bool IsEffective { get; set; }

	/// <summary>
	/// The time zone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	///    Type
	/// </summary>
	[DataMember(Name = "type")]
	public ScheduledDownTimeType Type { get; set; }

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
	///    Week of Month
	/// </summary>
	[DataMember(Name = "weekOfMonth")]
	public string? WeekOfMonth { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "sdt/sdts";

	/// <inheritdoc />
	public override string ToString() => $"{Type}:{Id}";
}
