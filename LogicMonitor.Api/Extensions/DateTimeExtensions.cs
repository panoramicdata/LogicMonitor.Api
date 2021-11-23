namespace LogicMonitor.Api.Extensions;

/// <summary>
/// DateTime Extensions
/// </summary>
internal static class DateTimeExtensions
{
	private static readonly DateTime TheEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	/// <summary>
	/// The number of seconds that this DateTime is past midnight on 1st January 1970
	/// </summary>
	/// <param name="secondsSinceTheEpoch">the number of seconds since the Epoch</param>
	public static DateTime ToDateTimeUtc(this long secondsSinceTheEpoch)
		=> TheEpoch.AddSeconds(secondsSinceTheEpoch);

	/// <summary>
	/// The number of seconds that this DateTime is past midnight on 1st January 1970
	/// </summary>
	/// <param name="secondsSinceTheEpoch">the number of seconds since the Epoch</param>
	public static DateTime ToDateTimeUtc(this int secondsSinceTheEpoch)
		=> TheEpoch.AddSeconds(secondsSinceTheEpoch);

	/// <summary>
	/// The number of seconds that this DateTime is past midnight on 1st January 1970 or null if zero
	/// </summary>
	/// <param name="secondsSinceTheEpoch">the number of seconds since the Epoch</param>
	public static DateTime? ToNullableDateTimeUtc(this int secondsSinceTheEpoch)
		=> secondsSinceTheEpoch == 0 ? (DateTime?)null : secondsSinceTheEpoch.ToDateTimeUtc();

	/// <summary>
	/// The number of seconds that this DateTime is past midnight on 1st January 1970
	/// </summary>
	/// <param name="msSinceTheEpoch">the number of seconds since the Epoch</param>
	public static DateTime ToDateTimeUtcFromMs(this long msSinceTheEpoch)
		=> TheEpoch.AddMilliseconds(msSinceTheEpoch);

	/// <summary>
	/// The number of seconds that this DateTime is past midnight on 1st January 1970 or null if zero
	/// </summary>
	/// <param name="secondsSinceTheEpoch">the number of seconds since the Epoch</param>
	public static DateTime? ToNullableDateTimeUtc(this long secondsSinceTheEpoch)
		=> secondsSinceTheEpoch == 0 ? (DateTime?)null : secondsSinceTheEpoch.ToDateTimeUtc();

	/// <summary>
	/// The number of seconds that this DateTime is past midnight on 1st January 1970
	/// </summary>
	/// <param name="dateTime">The DateTime</param>
	public static int SecondsSinceTheEpoch(this DateTime dateTime)
		=> (int)(dateTime - TheEpoch).TotalSeconds;

	/// <summary>
	/// The number of seconds that this DateTime is past midnight on 1st January 1970
	/// </summary>
	/// <param name="dateTime">The DateTime</param>
	public static DateTime ToSecondLevel(this DateTime dateTime)
		=> new(dateTime.Ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond);

	/// <summary>
	/// The number of milliseconds that this DateTime is past midnight on 1st January 1970
	/// </summary>
	/// <param name="dateTime">The DateTime</param>
	public static long MillisecondsSinceTheEpoch(this DateTime dateTime)
		=> (long)dateTime.SecondsSinceTheEpoch() * 1000;

	private static DateTime FirstDayOfThisMonth(this DateTime dateTime)
		=> new(dateTime.Year, dateTime.Month, 1);

	/// <summary>
	/// Midnight on the first day of the month prior to this DateTime
	/// </summary>
	/// <param name="dateTime"></param>
	public static DateTime FirstDayOfLastMonth(this DateTime dateTime)
		=> dateTime.FirstDayOfThisMonth().AddMonths(-1);

	/// <summary>
	/// Midnight on the first day this DateTime's month
	/// </summary>
	/// <param name="dateTime"></param>
	public static DateTime LastDayOfLastMonth(this DateTime dateTime)
		=> dateTime.FirstDayOfThisMonth().AddDays(-1);

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
