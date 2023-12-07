namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Website SDT creation DTO
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="websiteId">The website id</param>
/// <param name="checkpointId">The checkpoint id</param>
public class WebsiteCheckpointScheduledDownTimeCreationDto(int websiteId, int checkpointId) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.Website)
{

	/// <summary>
	/// The website id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int WebsiteId { get; set; } = websiteId;

	/// <summary>
	/// The checkpoint id
	/// </summary>
	[DataMember(Name = "checkpointId")]
	public int CheckpointId { get; set; } = checkpointId;
}