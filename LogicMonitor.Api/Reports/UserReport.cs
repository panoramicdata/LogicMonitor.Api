using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// A user report
	/// </summary>
	[DataContract]
	public class UserReport : Report
	{
		/// <summary>
		/// The user filter
		/// </summary>
		[DataMember(Name = "userFilter")]
		public UserReportUserFilter UserFilter { get; set; }

		/// <summary>
		/// The sorted-by column
		/// </summary>
		[DataMember(Name = "sortedBy")]
		public string SortedBy { get; set; }

		/// <summary>
		/// The columns
		/// </summary>
		[DataMember(Name = "columns")]
		public List<ReportColumn> Columns { get; set; }
	}
}