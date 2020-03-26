using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{

	/// <summary>
	/// A website alert SDT
	/// </summary>
	[DataContract]
	public class WebsiteAlertSdt : AlertSdt
	{
		/// <summary>
		/// The website id
		/// </summary>
		[DataMember(Name = "websiteId")]
		public int DeviceId { get; set; }
	}
}