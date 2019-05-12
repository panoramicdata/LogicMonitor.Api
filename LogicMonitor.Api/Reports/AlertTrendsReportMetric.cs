using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// A AlertTrendReportMetric
	/// </summary>
	[DataContract]
	public class AlertTrendsReportMetric
	{
		/// <summary>
		/// The item type
		/// </summary>
		[DataMember(Name = "itemType")]
		public string ItemType { get; set; }

		/// <summary>
		/// The item value
		/// </summary>
		[DataMember(Name = "itemVal")]
		public string ItemValue { get; set; }
	}
}