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


		int maxLogItemCount;
		// Was a Take provided?
		if (logFilter.Take is null)
		{
			// NO - set the maxLogItemCount to the count requested, set the request page size to LogItemsMaxTake
			maxLogItemCount = int.MaxValue;
			logFilter.Take = LogItemsMaxTake;
		}
		else
		{
			// YES - Are we being asked for more than the max defined request page size?
			if (logFilter.Take > LogItemsMaxTake)
			{
				// YES - set the maxLogItemCount to our LogItemsMaxTake, restricting the request page size to LogItemsMaxTake
				maxLogItemCount = (int)logFilter.Take;
				logFilter.Take = LogItemsMaxTake;
			}
			else
			{
				// NO - The request page size can stay as desired and the max set to this
				maxLogItemCount = (int)logFilter.Take;
			}
		}

		var allLogItems = new List<LogItem>();
		do
		{
			// Build the filter, always start with the date range requested
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

			var logItems = (await GetBySubUrlAsync<Page<LogItem>>(
				"setting/accesslogs" +
				$"?sort={EnumHelper.ToEnumString(logFilter.LogFilterSortOrder)}" +
				$"&offset={logFilter.Skip}" +
				$"&size={logFilter.Take}" +
				$"&filter={filter}",
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

		// Return the lot
		return allLogItems;
	}
}
