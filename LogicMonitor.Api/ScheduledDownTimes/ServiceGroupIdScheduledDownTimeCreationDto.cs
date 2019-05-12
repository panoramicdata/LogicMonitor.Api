using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{
	/// <summary>
	/// Website Group SDT creation DTO
	/// </summary>
	public class WebsiteGroupIdScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="websiteGroupId"></param>
		public WebsiteGroupIdScheduledDownTimeCreationDto(int websiteGroupId) : base(ScheduledDownTimeType.WebsiteGroup)
		  => WebsiteGroupId = websiteGroupId;

		/// <summary>
		/// The website group id
		/// </summary>
		[DataMember(Name = "websiteGroupId")]
		public int WebsiteGroupId { get; set; }
	}
}