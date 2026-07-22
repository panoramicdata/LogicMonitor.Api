namespace LogicMonitor.Api.Extensions;

/// <summary>
/// DateTime Extensions
/// </summary>
internal static class DateTimeExtensions
{
	private static readonly DateTime _theEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	/// <summary>
	/// Converts a value to a UTC DateTime by adding it to the Epoch as seconds - despite the parameter
	/// name, this treats the value as seconds, not milliseconds (see <see cref="ToDateTimeUtcFromMs"/> for
	/// a milliseconds-based conversion).
	/// </summary>
	/// <param name="msSinceTheEpoch">The number of seconds since the Epoch (despite the parameter name).</param>
	/// <returns>The equivalent UTC DateTime, clamped to DateTime.MinValue/MaxValue if out of range.</returns>
	public static DateTime ToDateTimeUtc(this long msSinceTheEpoch)
	{
		try
		{
			return _theEpoch.AddSeconds(msSinceTheEpoch);
		}
		catch (ArgumentOutOfRangeException)
		{
			// MS-24854 Some sources (e.g. LogicMonitor SDTs) can return a corrupted epoch value that
			// overflows DateTime's representable range. Clamp rather than throw.
			return msSinceTheEpoch < 0 ? DateTime.MinValue : DateTime.MaxValue;
		}
	}

	/// <summary>
	/// Converts a Unix timestamp (seconds since the Epoch) to a UTC DateTime.
	/// </summary>
	/// <param name="secondsSinceTheEpoch">The number of seconds since the Epoch.</param>
	/// <returns>The equivalent UTC DateTime.</returns>
	public static DateTime ToDateTimeUtc(this int secondsSinceTheEpoch)
		=> _theEpoch.AddSeconds(secondsSinceTheEpoch);

	/// <summary>
	/// Converts a Unix timestamp (seconds since the Epoch) to a UTC DateTime, or null when the value is
	/// zero (the sentinel for "not set").
	/// </summary>
	/// <param name="secondsSinceTheEpoch">The number of seconds since the Epoch.</param>
	/// <returns>The equivalent UTC DateTime, or null if secondsSinceTheEpoch is zero.</returns>
	public static DateTime? ToNullableDateTimeUtc(this int secondsSinceTheEpoch)
		=> secondsSinceTheEpoch == 0 ? null : secondsSinceTheEpoch.ToDateTimeUtc();

	/// <summary>
	/// Converts a Unix timestamp (milliseconds since the Epoch) to a UTC DateTime.
	/// </summary>
	/// <param name="msSinceTheEpoch">The number of milliseconds since the Epoch.</param>
	/// <returns>
	/// The equivalent UTC DateTime, clamped to DateTime.MinValue/MaxValue if out of range (MS-24854 - some
	/// sources, e.g. LogicMonitor SDTs, can return a corrupted epoch value that would otherwise overflow DateTime).
	/// </returns>
	public static DateTime ToDateTimeUtcFromMs(this long msSinceTheEpoch)
	{
		try
		{
			return _theEpoch.AddMilliseconds(msSinceTheEpoch);
		}
		catch (ArgumentOutOfRangeException)
		{
			// MS-24854 Some sources (e.g. LogicMonitor SDTs) can return a corrupted epoch value that
			// overflows DateTime's representable range. Clamp rather than throw - this is used by
			// ScheduledDownTime/HistoricScheduledDownTime.StartDateTimeUtc/EndDateTimeUtc,
			// Alert.LastUpdatedOnUtc and ResourceDataSourceInstanceConfig.PollUtc.
			return msSinceTheEpoch < 0 ? DateTime.MinValue : DateTime.MaxValue;
		}
	}

	/// <summary>
	/// Converts a Unix timestamp (seconds since the Epoch) to a UTC DateTime, or null when the value is
	/// zero (the sentinel for "not set").
	/// </summary>
	/// <param name="secondsSinceTheEpoch">The number of seconds since the Epoch.</param>
	/// <returns>The equivalent UTC DateTime, or null if secondsSinceTheEpoch is zero.</returns>
	public static DateTime? ToNullableDateTimeUtc(this long secondsSinceTheEpoch)
		=> secondsSinceTheEpoch == 0 ? null : secondsSinceTheEpoch.ToDateTimeUtc();

	/// <summary>
	/// The number of seconds that this DateTime is past midnight on 1st January 1970.
	/// </summary>
	/// <param name="dateTime">The DateTime.</param>
	/// <returns>The number of seconds since the Epoch.</returns>
	public static int SecondsSinceTheEpoch(this DateTime dateTime)
		=> (int)(dateTime - _theEpoch).TotalSeconds;

	/// <summary>
	/// The number of milliseconds that this DateTime is past midnight on 1st January 1970. Note this
	/// truncates to whole-second precision first (via SecondsSinceTheEpoch), so any sub-second component is lost.
	/// </summary>
	/// <param name="dateTime">The DateTime.</param>
	/// <returns>The number of milliseconds since the Epoch, to whole-second precision.</returns>
	public static long MillisecondsSinceTheEpoch(this DateTime dateTime)
		=> (long)dateTime.SecondsSinceTheEpoch() * 1000;

	private static DateTime FirstDayOfThisMonth(this DateTime dateTime)
		=> new(dateTime.Year, dateTime.Month, 1);

	/// <summary>
	/// Midnight on the first day of the month prior to this DateTime.
	/// </summary>
	/// <param name="dateTime">The DateTime.</param>
	/// <returns>Midnight on the first day of the previous month.</returns>
	public static DateTime FirstDayOfLastMonth(this DateTime dateTime)
		=> dateTime.FirstDayOfThisMonth().AddMonths(-1);

	/// <summary>
	/// Midnight on the last day of the month prior to this DateTime's month.
	/// </summary>
	/// <param name="dateTime">The DateTime.</param>
	/// <returns>Midnight on the last day of the previous month.</returns>
	public static DateTime LastDayOfLastMonth(this DateTime dateTime)
		=> dateTime.FirstDayOfThisMonth().AddDays(-1);

	/// <summary>
	/// Splits a date/time range into consecutive chunks of at most chunkSize each.
	/// </summary>
	/// <param name="startDateTime">The start of the overall range.</param>
	/// <param name="endDateTime">The end of the overall range.</param>
	/// <param name="chunkSize">The maximum size of each returned chunk.</param>
	/// <returns>A list of (start, end) tuples covering the overall range.</returns>
	/// <exception cref="ArgumentException">endDateTime is before startDateTime, or the range spans more than 10,000 hours.</exception>
	public static List<Tuple<DateTime, DateTime>> GetChunkedTimeRangeList(this DateTime startDateTime, DateTime endDateTime, TimeSpan chunkSize)
	{
		if (endDateTime < startDateTime)
		{
			throw new ArgumentException("endDateTime is before startDateTime", nameof(endDateTime));
		}

		// Handle a little more than a year
		const int maxHourCount = 10000;
		if ((endDateTime - startDateTime).TotalHours > maxHourCount)
		{
			throw new ArgumentException($"endDateTime is over {maxHourCount} hours more than startDateTime", nameof(endDateTime));
		}

		var chunkList = new List<Tuple<DateTime, DateTime>>();
		while (true)
		{
			var nextDateTime = startDateTime + chunkSize;
			if (nextDateTime >= endDateTime)
			{
				chunkList.Add(new Tuple<DateTime, DateTime>(startDateTime, endDateTime));
				return chunkList;
			}

			chunkList.Add(new Tuple<DateTime, DateTime>(startDateTime, nextDateTime));
			startDateTime = nextDateTime;
		}
	}
}
