using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	/// <summary>
	///     LogItems
	/// </summary>
	public partial class PortalClient
	{
		private const int LogItemsMaxTake = 49;

		/// <summary>
		///     Gets the LogItems
		/// </summary>
		/// <param name="logFilter"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		// public List<LogItem> GetLogItemsAsync(LogFilter logFilter) => Get<LogItemCollection>(ApiMethod.Do, $"onesetting?func=getAccessLog&needTotal=true&orderBy=happenedOn&orderDirection=desc&start={logFilter.Skip}&results={logFilter.Take}").AccessLogItems;
		public async Task<List<LogItem>> GetLogItemsAsync(LogFilter logFilter = null, CancellationToken cancellationToken = default)
		{
			// If take is specified, do only that chunk.
			if (logFilter == null)
			{
				logFilter = new LogFilter(
					0,
					LogItemsMaxTake,
					DateTime.Parse("1970-01-01"),
					DateTime.UtcNow,
					LogFilterSortOrder.HappenedOnAsc);
			}

			if (logFilter.Skip == null)
			{
				logFilter.Skip = 0;
			}
			int maxLogItemCount;
			if (logFilter.Take != null)
			{
				if (logFilter.Take > LogItemsMaxTake)
				{
					maxLogItemCount = (int)logFilter.Take;
					logFilter.Take = LogItemsMaxTake;
				}
				else
				{
					maxLogItemCount = (int)logFilter.Take;
				}
			}
			else
			{
				maxLogItemCount = int.MaxValue;
				logFilter.Take = LogItemsMaxTake;
			}

			var allLogItems = new List<LogItem>();
			do
			{
				var logItems = (await GetBySubUrlAsync<Page<LogItem>>($"setting/accesslogs?sort={EnumHelper.ToEnumString(logFilter.LogFilterSortOrder)}&offset={logFilter.Skip}&size={logFilter.Take}&filter=happenedOn%3E%3A{logFilter.StartDateTimeUtc.SecondsSinceTheEpoch()}%2ChappenedOn%3C%3A{logFilter.EndDateTimeUtc.SecondsSinceTheEpoch()}", cancellationToken).ConfigureAwait(false)).Items;
				allLogItems.AddRange(logItems.Where(item => !allLogItems.Select(li => li.Id).Contains(item.Id)).ToList());
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
}