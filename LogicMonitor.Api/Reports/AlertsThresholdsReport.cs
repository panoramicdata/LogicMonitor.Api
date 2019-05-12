using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// An alert threshold report
	/// </summary>
	[DataContract]
	public class AlertsThresholdsReport : Report
	{
		/// <summary>
		/// The group full path
		/// </summary>
		[DataMember(Name = "groupFullPath")]
		public string GroupFullPath { get; set; }

		/// <summary>
		/// The device display name
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DeviceDisplayName { get; set; }

		/// <summary>
		/// The dataSourceInstanceName
		/// </summary>
		[DataMember(Name = "dataSourceInstanceName")]
		public string DataSourceInstanceName { get; set; }

		/// <summary>
		/// The dataPoint
		/// </summary>
		[DataMember(Name = "dataPoint")]
		public string DataPoint { get; set; }

		/// <summary>
		/// The columns to sort by
		/// </summary>
		[DataMember(Name = "sortedBy")]
		public string SortedBy { get; set; }

		/// <summary>
		/// Whether to exclude global
		/// </summary>
		[DataMember(Name = "excludeGlobal")]
		public bool IncludePreexist { get; set; }

		/// <summary>
		/// The columns
		/// </summary>
		[DataMember(Name = "columns")]
		public List<ReportColumn> Columns { get; set; }
	}
}