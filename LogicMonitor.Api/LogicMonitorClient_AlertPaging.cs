namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	private Task<(List<Alert> alerts, bool limitReached)> GetRestAlertPagesAsync(AlertFilter filter, bool calledFromChunked, CancellationToken cancellationToken)
		=> GetAlertPagesAsync(filter, calledFromChunked, (f, ct) => FilteredGetAsync("alert/alerts", f.GetFilter(), ct), cancellationToken);

	private Task<(List<Alert> alerts, bool limitReached)> GetEntityAlertPagesAsync(string alertsSubUrl, AlertFilter filter, bool calledFromChunked, CancellationToken cancellationToken)
	{
		// Ensure that the filter CANNOT override the entity ID set!
		filter.RemoveMonitorObjectReferences();

		return GetAlertPagesAsync(filter, calledFromChunked, (f, ct) => GetBySubUrlAsync<Page<Alert>>($"{alertsSubUrl}?{f.GetFilter()}", ct), cancellationToken);
	}

	private static async Task<(List<Alert> alerts, bool limitReached)> GetAlertPagesAsync(
		AlertFilter filter,
		bool calledFromChunked,
		Func<AlertFilter, CancellationToken, Task<Page<Alert>>> getPage,
		CancellationToken cancellationToken)
	{
		// Ensure skip is set (or 0)
		filter.Skip ??= 0;

		var maxAlertCount = NormaliseAlertTake(filter);

		var allAlerts = new List<Alert>();
		do
		{
			var page = await getPage(filter, cancellationToken).ConfigureAwait(false);

			allAlerts.AddRange([.. page.Items.Where(alert => !allAlerts.Select(aa => aa.Id).Contains(alert.Id))]);

			if (!calledFromChunked && allAlerts.Count >= 5000)
			{
				// LMREP-9012: with more than 5000 alerts (anywhere near the 10,000 limit), use the chunked method instead
				return (new List<Alert>(), true);
			}

			// The page TotalCount is negative if there are more to come, so keep fetching until a page is empty.
			if (page.Items?.Count == 0)
			{
				break;
			}

			AdvanceAlertFilter(filter, page.SearchId, maxAlertCount - allAlerts.Count);
		}
		while (filter.Take != 0);

		return (allAlerts, false);
	}

	// Returns the number of alerts wanted, and caps the filter's Take at the page size.
	private static int NormaliseAlertTake(AlertFilter filter)
	{
		if (filter.Take is null)
		{
			filter.Take = AlertsMaxTake;
			return int.MaxValue;
		}

		var maxAlertCount = (int)filter.Take;
		if (filter.Take > AlertsMaxTake)
		{
			filter.Take = AlertsMaxTake;
		}

		return maxAlertCount;
	}

	private static void AdvanceAlertFilter(AlertFilter filter, string searchId, int remainingAlertCount)
	{
		if (filter.SearchId is null && !string.IsNullOrWhiteSpace(searchId))
		{
			// We can re-use the searchId
			filter.SearchId = searchId;
		}

		filter.Skip += AlertsMaxTake;
		filter.Take = Math.Min(AlertsMaxTake, remainingAlertCount);
	}

	private static async Task<List<Alert>> GetAlertsChunkedAsync(
		AlertFilter filter,
		TimeSpan chunkSize,
		Func<AlertFilter, Task<(List<Alert> alerts, bool limitReached)>> getChunk)
	{
		var originalStartEpochIsAfter = filter.StartEpochIsAfter;
		var originalStartEpochIsBefore = filter.StartEpochIsBefore;
		var utcNow = DateTime.UtcNow;
		filter.StartEpochIsAfter ??= utcNow.AddYears(-1).SecondsSinceTheEpoch();

		filter.StartEpochIsBefore ??= utcNow.SecondsSinceTheEpoch();

		var allAlerts = new ConcurrentBag<Alert>();

		var alertFilterList = ((long)filter.StartEpochIsAfter).ToDateTimeUtc()
			.GetChunkedTimeRangeList(((long)filter.StartEpochIsBefore).ToDateTimeUtc(), chunkSize)
			.Select(t =>
			{
				var newAlertFilter = filter.Clone();
				newAlertFilter.ResetSearch();
				newAlertFilter.StartEpochIsAfter = t.Item1.SecondsSinceTheEpoch() - 1; // Take one off to include anything raised on that exact second
				newAlertFilter.StartEpochIsBefore = t.Item2.SecondsSinceTheEpoch();
				return newAlertFilter;
			});
		await Task.WhenAll(alertFilterList.Select(async individualAlertFilter =>
		{
			// Jitter, to spread the chunked requests out across the portal. Not security-sensitive.
			await Task.Delay(Random.Shared.Next(0, 2000), default).ConfigureAwait(false);
			foreach (var alert in (await getChunk(individualAlertFilter).ConfigureAwait(false)).alerts)
			{
				allAlerts.Add(alert);
			}
		})).ConfigureAwait(false);

		filter.StartEpochIsAfter = originalStartEpochIsAfter;
		filter.StartEpochIsBefore = originalStartEpochIsBefore;

		return [.. allAlerts.DistinctBy(a => a.Id).Take(filter.Take ?? int.MaxValue)];
	}
}
