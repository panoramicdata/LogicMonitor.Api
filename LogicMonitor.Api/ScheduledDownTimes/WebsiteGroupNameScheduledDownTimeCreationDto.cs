namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Website Group SDT creation DTO
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="websiteGroupName"></param>
public class WebsiteGroupNameScheduledDownTimeCreationDto(string websiteGroupName) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.WebsiteGroup)
{

	/// <summary>
	/// The website group name
	/// </summary>
	[DataMember(Name = "websiteGroupName")]
	public string WebsiteGroupName { get; set; } = websiteGroupName;
}
