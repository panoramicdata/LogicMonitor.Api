using System;

namespace LogicMonitor.Api.Logs
{
	/// <summary>
	/// An access log filter
	/// </summary>
	public class LogFilter
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="skip">The number of log entries to skip</param>
		/// <param name="take">The number of log entries to take</param>
		/// <param name="startDateTimeUtc">The UTC start date/time</param>
		/// <param name="endDateTimeUtc">The UTC end date/time</param>
		/// <param name="logFilterSortOrder">The Log file sort order</param>
		public LogFilter(int skip, int take, DateTime startDateTimeUtc, DateTime endDateTimeUtc, LogFilterSortOrder logFilterSortOrder)
		{
			Skip = skip;
			Take = take;
			StartDateTimeUtc = startDateTimeUtc;
			EndDateTimeUtc = endDateTimeUtc;
			LogFilterSortOrder = logFilterSortOrder;
		}

		/// <summary>
		/// The Log file sort order
		/// </summary>
		public LogFilterSortOrder LogFilterSortOrder { get; set; }

		/// <summary>
		/// The end DateTime in UTC
		/// </summary>
		public DateTime EndDateTimeUtc { get; set; }

		/// <summary>
		/// The start DateTime in UTC
		/// </summary>
		public DateTime StartDateTimeUtc { get; set; }

		/// <summary>
		/// The number of records to skip
		/// </summary>
		public int? Skip { get; set; }

		/// <summary>
		/// The number of records to take
		/// </summary>
		public int? Take { get; set; }
	}
}