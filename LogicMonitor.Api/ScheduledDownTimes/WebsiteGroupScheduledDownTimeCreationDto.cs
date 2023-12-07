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

/// <summary>
/// WebsiteGroup SDT creation DTO - deprecated
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="websiteGroupId"></param>
[Obsolete("Use WebsiteGroupScheduledDownTimeCreationDto instead")]
public class WebsiteGroupIdScheduledDownTimeCreationDto(int websiteGroupId) : WebsiteGroupScheduledDownTimeCreationDto(websiteGroupId)
{
}
