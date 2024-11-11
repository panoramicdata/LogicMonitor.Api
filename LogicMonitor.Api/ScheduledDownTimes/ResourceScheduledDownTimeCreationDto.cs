namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Resource SDT creation DTO
/// </summary>
/// <remarks>
///    Resource
/// </remarks>
/// <param name="resourceId"></param>
public class ResourceScheduledDownTimeCreationDto(int resourceId) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.Resource)
{
	/// <summary>
	/// The id of the Resource that the SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; } = resourceId;

	/// <summary>
	/// The name of the Resource that this SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceName { get; set; } = string.Empty;
}
