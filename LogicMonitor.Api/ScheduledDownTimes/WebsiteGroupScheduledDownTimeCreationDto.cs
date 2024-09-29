namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// WebsiteGroup SDT creation DTO
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="websiteGroupId"></param>
public class WebsiteGroupScheduledDownTimeCreationDto(int websiteGroupId) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.WebsiteGroup)
{

	/// <summary>
	/// The website group id
	/// </summary>
	[DataMember(Name = "websiteGroupId")]
	public int WebsiteGroupId { get; set; } = websiteGroupId;
}