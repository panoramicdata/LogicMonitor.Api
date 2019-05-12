using System.Runtime.Serialization;

namespace LogicMonitor.Api.OpsNotes
{
	/// <summary>
	/// A Website Ops Note Scope creation DTO
	/// </summary>
	[DataContract]
	public class WebsiteOpsNoteScopeCreationDto : OpsNoteScopeCreationDto
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public WebsiteOpsNoteScopeCreationDto() => Type = "website";

		/// <summary>
		/// The website Id
		/// </summary>
		[DataMember(Name = "websiteId")]
		public int WebsitesId { get; set; }
	}
}