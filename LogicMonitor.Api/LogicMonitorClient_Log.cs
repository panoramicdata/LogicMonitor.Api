namespace LogicMonitor.Api;

/// <summary>
///     LogItems
/// </summary>
public partial class LogicMonitorClient
{
	private const int LogItemsMaxTake = 49;

	/// <summary>
	/// Gets LogItems
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<List<LogItem>> GetLogItemsAsync(CancellationToken cancellationToken)
		=> GetLogItemsAsync(null, cancellationToken);


	/// <summary>
	/// Gets LogItems using a filter
	/// </summary>
	/// <param name="logFilter"></param>
	/// <param name="cancellationToken"></param>
	public async Task<List<LogItem>> GetLogItemsAsync(LogFilter? logFilter, CancellationToken cancellationToken)
	{
		// If take is specified, do only that chunk.
		logFilter ??= new LogFilter(
				0,
				LogItemsMaxTake,
				DateTime.Parse("1970-01-01", CultureInfo.InvariantCulture),
				DateTime.UtcNow,
				LogFilterSortOrder.HappenedOnAsc);

		logFilter.Skip ??= 0;

		var maxLogItemCount = NormaliseLogItemTake(logFilter);

		var allLogItems = new List<LogItem>();
		do
		{
			var logItems = (await GetBySubUrlAsync<Page<LogItem>>(
				"setting/accesslogs" +
				$"?sort={EnumHelper.ToEnumString(logFilter.LogFilterSortOrder)}" +
				$"&offset={logFilter.Skip}" +
				$"&size={logFilter.Take}" +
				$"&filter={BuildLogItemFilter(logFilter)}",
				cancellationToken
				).ConfigureAwait(false)).Items;
			allLogItems.AddRange([.. logItems.Where(item => !allLogItems.Select(li => li.Id).Contains(item.Id))]);
			if (logItems.Count == 0)
			{
				break;
			}

			logFilter.Skip += LogItemsMaxTake;
			logFilter.Take = Math.Min(LogItemsMaxTake, maxLogItemCount - allLogItems.Count);
		}
		while (logFilter.Take != 0);

		return allLogItems;
	}

	// Returns the number of items wanted, and caps the filter's Take at the page size.
	private static int NormaliseLogItemTake(LogFilter logFilter)
	{
		if (logFilter.Take is null)
		{
			logFilter.Take = LogItemsMaxTake;
			return int.MaxValue;
		}

		var maxLogItemCount = (int)logFilter.Take;
		if (logFilter.Take > LogItemsMaxTake)
		{
			logFilter.Take = LogItemsMaxTake;
		}

		return maxLogItemCount;
	}

	private static string BuildLogItemFilter(LogFilter logFilter)
	{
		// Always start with the date range requested
		var filter = $"happenedOn%3E%3A{logFilter.StartDateTimeUtc.SecondsSinceTheEpoch()}%2ChappenedOn%3C%3A{logFilter.EndDateTimeUtc.SecondsSinceTheEpoch()}";
		if (!string.IsNullOrWhiteSpace(logFilter.UsernameFilter))
		{
			// Need to UrlEncode before adding to the filter
			filter += HttpUtility.UrlEncode($",username:{logFilter.UsernameFilter}");
		}

		if (!string.IsNullOrWhiteSpace(logFilter.TextFilter))
		{
			filter += HttpUtility.UrlEncode($",_all~{logFilter.TextFilter}");
		}

		return filter;
	}
}
