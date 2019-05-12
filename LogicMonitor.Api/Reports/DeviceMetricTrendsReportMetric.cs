using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// A HostMetricTrendsReportMetric
	/// </summary>
	[DataContract]
	public class DeviceMetricTrendsReportMetric
	{
		/// <summary>
		/// The dataSourceId
		/// </summary>
		[DataMember(Name = "dataSourceId")]
		public int DataSourceId { get; set; }

		/// <summary>
		/// The dataSourceFullName
		/// </summary>
		[DataMember(Name = "DataSourceFullName")]
		public string DataSourceFullName { get; set; }

		/// <summary>
		/// The instances
		/// </summary>
		[DataMember(Name = "Instances")]
		public string Instances { get; set; }

		/// <summary>
		/// The metric
		/// </summary>
		[DataMember(Name = "Metric")]
		public string Metric { get; set; }
	}
}