using System;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{
	/// <summary>
	/// WebsiteGroup SDT creation DTO
	/// </summary>
	public class WebsiteGroupScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="websiteGroupId"></param>
		public WebsiteGroupScheduledDownTimeCreationDto(int websiteGroupId) : base(ScheduledDownTimeType.WebsiteGroup)
		  => WebsiteGroupId = websiteGroupId;

		/// <summary>
		/// The website group id
		/// </summary>
		[DataMember(Name = "websiteGroupId")]
		public int WebsiteGroupId { get; set; }
	}

	/// <summary>
	/// WebsiteGroup SDT creation DTO - deprecated
	/// </summary>
	[Obsolete("Use WebsiteGroupScheduledDownTimeCreationDto instead")]
	public class WebsiteGroupIdScheduledDownTimeCreationDto : WebsiteGroupScheduledDownTimeCreationDto
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="websiteGroupId"></param>
		public WebsiteGroupIdScheduledDownTimeCreationDto(int websiteGroupId) : base(websiteGroupId)
		{
		}
	}
}