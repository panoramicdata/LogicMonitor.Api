using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// A ServiceLevelAgreementReportMetric
	/// </summary>
	[DataContract]
	public class SlaReportMetric
	{
		/// <summary>
		/// The group name
		/// </summary>
		[DataMember(Name = "groupName")]
		public string GroupName { get; set; }

		/// <summary>
		/// The device name
		/// </summary>
		[DataMember(Name = "deviceName")]
		public string DeviceName { get; set; }

		/// <summary>
		/// The data source id
		/// </summary>
		[DataMember(Name = "dataSourceId")]
		public int DataSourceId { get; set; }

		/// <summary>
		/// The datasource
		/// </summary>
		[DataMember(Name = "dataSourceFullName")]
		public string DataSourceFullName { get; set; }

		/// <summary>
		/// The instances
		/// </summary>
		[DataMember(Name = "instances")]
		public string Instances { get; set; }

		/// <summary>
		/// The metric
		/// </summary>
		[DataMember(Name = "metric")]
		public string Metric { get; set; }

		/// <summary>
		/// The threshold
		/// </summary>
		[DataMember(Name = "threshold")]
		public string Threshold { get; set; }

		/// <summary>
		/// The exclusion SDT type
		/// </summary>
		[DataMember(Name = "exclusionSDTType")]
		public string ExclusionSDTType { get; set; }
	}
}