namespace LogicMonitor.Api.ScheduledDownTimes;
/// <summary>
/// ScheduledDownTimeHistory
/// </summary>
[DataContract]
public class ScheduledDownTimeHistory
{
	/// <summary>
	/// The duration of the SDT, in minutes
	/// </summary>
	[DataMember(Name = "duration")]
	public int Duration { get; set; }

	/// <summary>
	/// The ID of the resource in SDT, e.g. the group or device in SDT
	/// </summary>
	[DataMember(Name = "itemId")]
	public int ItemId { get; set; }

	/// <summary>
	/// The end epoch for the SDT
	/// </summary>
	[DataMember(Name = "approximateEndEpoch", IsRequired = false)]
	public long ApproximateEndEpoch { get; set; }

	/// <summary>
	/// The user that added the SDT
	/// </summary>
	[DataMember(Name = "admin")]
	public string? Admin { get; set; }

	/// <summary>
	/// The comment associated with the SDT
	/// </summary>
	[DataMember(Name = "comment")]
	public string? Comment { get; set; }

	/// <summary>
	/// The SDT type
	/// </summary>
	[DataMember(Name = "type")]
	public string? Type { get; set; }

	/// <summary>
	/// The start epoch for the SDT
	/// </summary>
	[DataMember(Name = "approximateStartEpoch", IsRequired = false)]
	public long ApproximateStartEpoch { get; set; }
}
