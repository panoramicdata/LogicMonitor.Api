using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{
	/// <summary>
	/// Website Group SDT creation DTO
	/// </summary>
	public class WebsiteGroupNameScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="websiteGroupName"></param>
		public WebsiteGroupNameScheduledDownTimeCreationDto(string websiteGroupName) : base(ScheduledDownTimeType.WebsiteGroup) => WebsiteGroupName = websiteGroupName;

		/// <summary>
		/// The website group name
		/// </summary>
		[DataMember(Name = "websiteGroupName")]
		public string WebsiteGroupName { get; set; }
	}
}