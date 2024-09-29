namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// WebsiteGroup SDT creation DTO - deprecated
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="websiteGroupId"></param>
[Obsolete("Use WebsiteGroupScheduledDownTimeCreationDto instead", true)]
public class WebsiteGroupIdScheduledDownTimeCreationDto(int websiteGroupId) : WebsiteGroupScheduledDownTimeCreationDto(websiteGroupId)
{
}
