using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// A date range report
	/// </summary>
	public abstract class DateRangeReport : Report
	{
		/// <summary>
		/// The date range
		/// </summary>
		[DataMember(Name = "dateRange")]
		public string DateRange { get; set; }
	}
}