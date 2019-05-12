using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// An Website SLA report metric
	/// </summary>
	[DataContract]
	public class WebsiteSlaReportMetric
	{
		/// <summary>
		/// The website group name (or * for all)
		/// </summary>
		[DataMember(Name = "groupName")]
		public string GroupName { get; set; }

		/// <summary>
		/// The Website Name (or * for all)
		/// </summary>
		[DataMember(Name = "websiteName")]
		public string WebsiteName { get; set; }

		/// <summary>
		/// Whether to exclude alerts occurring in SDT
		/// </summary>
		[DataMember(Name = "excludeSDT")]
		public bool ExcludeSDT { get; set; }
	}
}