namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Website SDT creation DTO
/// </summary>
public class WebsiteCheckpointScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="websiteId">The website id</param>
	/// <param name="checkpointId">The checkpoint id</param>
	public WebsiteCheckpointScheduledDownTimeCreationDto(int websiteId, int checkpointId) : base(ScheduledDownTimeType.Website)
	{
		WebsiteId = websiteId;
		CheckpointId = checkpointId;
	}

	/// <summary>
	/// The website id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int WebsiteId { get; set; }

	/// <summary>
	/// The checkpoint id
	/// </summary>
	[DataMember(Name = "checkpointId")]
	public int CheckpointId { get; set; }
}