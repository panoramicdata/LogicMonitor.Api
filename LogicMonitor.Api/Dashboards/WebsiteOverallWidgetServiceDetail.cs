using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// The WebsitevOerallWidgetWebsiteDetail
	/// </summary>
	[DataContract]
	public class WebsiteOverallWidgetWebsiteDetail
	{
		/// <summary>
		/// The Website id
		/// </summary>
		[DataMember(Name = "id")]
		public int WebsiteId { get; set; }

		/// <summary>
		/// The Website name
		/// </summary>
		[DataMember(Name = "name")]
		public string WebsiteName { get; set; }
	}
}