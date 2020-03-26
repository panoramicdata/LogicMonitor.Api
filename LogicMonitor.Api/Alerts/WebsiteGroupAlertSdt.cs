using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{

	/// <summary>
	/// A website group alert SDT
	/// </summary>
	[DataContract]
	public class WebsiteGroupAlertSdt : AlertSdt
	{
		/// <summary>
		/// The website group id
		/// </summary>
		[DataMember(Name = "websiteGroupId")]
		public int WebsiteGroupId { get; set; }
	}
}