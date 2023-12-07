namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// Website SDT creation DTO
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="websiteName"></param>
public class WebsiteNameScheduledDownTimeCreationDto(string websiteName) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.Website)
{

	/// <summary>
	/// The website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; } = websiteName;
}
