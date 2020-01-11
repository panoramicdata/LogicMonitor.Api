using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{
	/// <summary>
	/// Website SDT creation DTO
	/// </summary>
	public class WebsiteScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="websiteId"></param>
		public WebsiteScheduledDownTimeCreationDto(int websiteId) : base(ScheduledDownTimeType.Website)
		{
			WebsiteId = websiteId;
		}

		/// <summary>
		/// The website id
		/// </summary>
		[DataMember(Name = "websiteId")]
		public int WebsiteId { get; set; }
	}
}