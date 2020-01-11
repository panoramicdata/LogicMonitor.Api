using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{
	/// <summary>
	/// Website SDT creation DTO
	/// </summary>
	public class WebsiteNameScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="websiteName"></param>
		public WebsiteNameScheduledDownTimeCreationDto(string websiteName) : base(ScheduledDownTimeType.Website)
		{
			WebsiteName = websiteName;
		}

		/// <summary>
		/// The website name
		/// </summary>
		[DataMember(Name = "websiteName")]
		public string WebsiteName { get; set; }
	}
}