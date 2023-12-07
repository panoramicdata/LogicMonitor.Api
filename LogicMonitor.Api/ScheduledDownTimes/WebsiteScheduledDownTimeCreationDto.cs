namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Website SDT creation DTO
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="websiteId"></param>
public class WebsiteScheduledDownTimeCreationDto(int websiteId) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.Website)
{

	/// <summary>
	/// The website id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int WebsiteId { get; set; } = websiteId;
}
