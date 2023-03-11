namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Historic Scheduled Down Time
/// </summary>
[DataContract]
public class HistoricScheduledDownTime : StringIdentifiedItem
{
	/// <summary>
	/// Comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;

	/// <summary>
	/// Start date time UTC
	/// </summary>
	[DataMember(Name = "startEpoch")]
	public long StartDateTimeMs { get; set; }

	/// <summary>
	/// End date time UTC
	/// </summary>
	[DataMember(Name = "endEpoch")]
	public long EndDateTimeMs { get; set; }

	/// <summary>
	/// Item Type
	/// </summary>
	[DataMember(Name = "type")]
	public string ItemType { get; set; } = string.Empty;

	/// <summary>
	/// Item Type
	/// </summary>
	[DataMember(Name = "itemId")]
	public int ItemId { get; set; }

	/// <summary>
	/// The user that configured it
	/// </summary>
	[DataMember(Name = "admin")]
	public string Admin { get; set; } = string.Empty;

	/// <summary>
	/// Duration
	/// </summary>
	[DataMember(Name = "duration")]
	public int DurationMinutes { get; set; }

	/// <summary>
	/// Start DateTime UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime StartDateTimeUtc => StartDateTimeMs.ToDateTimeUtcFromMs();

	/// <summary>
	/// End DateTime UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime EndDateTimeUtc => EndDateTimeMs.ToDateTimeUtcFromMs();
}
