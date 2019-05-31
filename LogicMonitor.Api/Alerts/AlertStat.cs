using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// The total alert stats for the logged-in user
	/// </summary>
	[DataContract]
	public class AlertStat : IHasSingletonEndpoint
	{
		/// <summary>
		/// The warning count
		/// </summary>
		[DataMember(Name = "warns")]
		public int Warnings { get; set; }

		/// <summary>
		/// The error count
		/// </summary>
		[DataMember(Name = "errors")]
		public int Errors { get; set; }

		/// <summary>
		/// The critical count
		/// </summary>
		[DataMember(Name = "criticals")]
		public int Criticals { get; set; }

		/// <summary>
		/// The Website warning count
		/// </summary>
		[DataMember(Name = "websiteWarns")]
		public int WebsiteWarnings { get; set; }

		/// <summary>
		/// The Website error count
		/// </summary>
		[DataMember(Name = "websiteErrors")]
		public int WebsiteErrors { get; set; }

		/// <summary>
		/// The Website critical count
		/// </summary>
		[DataMember(Name = "websiteCriticals")]
		public int WebsiteCriticals { get; set; }

		/// <summary>
		/// The dead host count
		/// </summary>
		[DataMember(Name = "deadhosts")]
		public int DeadHosts { get; set; }

		/// <summary>
		/// The count of alerts in scheduled down time
		/// </summary>
		[DataMember(Name = "sdtAlerts")]
		public int SdtAlerts { get; set; }

		/// <summary>
		/// The total count of alerts
		/// </summary>
		[DataMember(Name = "totalAlerts")]
		public int TotalAlerts { get; set; }

		/// <summary>
		/// The total count of alerts
		/// </summary>
		[DataMember(Name = "ackAlerts")]
		public int AckAlerts { get; set; }

		/// <summary>
		///    The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "alert/stat";
	}
}