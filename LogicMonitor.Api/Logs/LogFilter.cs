namespace LogicMonitor.Api.Logs;

/// <summary>
/// An access log filter
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="skip">The number of log entries to skip</param>
/// <param name="take">The number of log entries to take</param>
/// <param name="startDateTimeUtc">The UTC start date/time</param>
/// <param name="endDateTimeUtc">The UTC end date/time</param>
/// <param name="logFilterSortOrder">The Log file sort order</param>
public class LogFilter(int skip, int take, DateTime startDateTimeUtc, DateTime endDateTimeUtc, LogFilterSortOrder logFilterSortOrder)
{

	/// <summary>
	/// The Log file sort order
	/// </summary>
	public LogFilterSortOrder LogFilterSortOrder { get; set; } = logFilterSortOrder;

	/// <summary>
	/// The end DateTime in UTC
	/// </summary>
	public DateTime EndDateTimeUtc { get; set; } = endDateTimeUtc;

	/// <summary>
	/// The start DateTime in UTC
	/// </summary>
	public DateTime StartDateTimeUtc { get; set; } = startDateTimeUtc;

	/// <summary>
	/// The number of records to skip
	/// </summary>
	public int? Skip { get; set; } = skip;

	/// <summary>
	/// The number of records to take
	/// </summary>
	public int? Take { get; set; } = take;

	/// <summary>
	/// The username filter as per LogicMonitor documentation. The filtered values must be Url Encoded, see example below
	/// Example: to allow both "\"System:ActiveDiscovery\"" and "\"System:AppliesTo\"" the filter should be "\"System%3AActiveDiscovery\"|\"System%3AAppliesTo\""
	/// </summary>
	public string UsernameFilter { get; set; } = string.Empty;

	/// <summary>
	/// This filter is globbed and applied to the "_all" filter
	/// Example: 
	///		"\"*health*\"" will include everything with the text 'health'
	///		"\"* AND NOT *health*\"" will exclude everything with the text 'health'
	/// </summary>
	public string TextFilter { get; set; } = string.Empty;

}
