using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// A Collector SDT
	/// </summary>
	[DataContract]
	public class CollectorAlertSdt : AlertSdt
	{
		/// <summary>
		/// The Collector description
		/// </summary>
		[DataMember(Name = "collectorDescription")]
		public string CollectorDescription { get; set; }

		/// <summary>
		/// The Collector description
		/// </summary>
		[DataMember(Name = "collectorId")]
		public int CollectorId { get; set; }
	}
}
