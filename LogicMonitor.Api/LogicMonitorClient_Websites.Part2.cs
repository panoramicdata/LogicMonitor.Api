namespace LogicMonitor.Api;

/// <summary>
///    Websites Portal interaction
/// </summary>
public partial class LogicMonitorClient
{

	/// <summary>
	///     Gets website Group properties
	/// </summary>
	/// <param name="websiteGroupId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<EntityProperty>> GetWebsiteGroupPropertiesAsync(int websiteGroupId, CancellationToken cancellationToken)
		=> GetAllAsync<EntityProperty>($"website/groups/{websiteGroupId}/properties", cancellationToken);

	/// <summary>
	/// Get Alerts for a Website by ID
	/// </summary>
	/// <param name="websiteId">The Website ID</param>
	/// <param name="filter">The Alert Filter</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	/// <returns>A List of Alerts</returns>
	public async Task<List<Alert>> GetWebsiteAlertsByIdAsync(
		int websiteId,
		AlertFilter filter,
		CancellationToken cancellationToken)
	{
		// Ensure that the filter CANNOT override the website ID set!
		filter.RemoveMonitorObjectReferences();

		var (alerts, limitReached) = await
			GetWebsiteAlertsByIdNormalAsync(websiteId, filter, false, cancellationToken)
			.ConfigureAwait(false);

		if (limitReached)
		{
			// Fall back to the chunked method
			alerts = await GetWebsiteAlertsByIdChunkedAsync(websiteId, filter, TimeSpan.FromHours(24)).ConfigureAwait(false);
		}

		if (filter?.IsCleared == true)
		{
			return [.. alerts.Where(a => a.IsCleared)];
		}

		return filter?.IsCleared == false ? [.. alerts.Where(a => !a.IsCleared)] : alerts;
	}

	/// <summary>
	///     This version of the call requests hourly chunks
	/// </summary>
	/// <param name="websiteId">The Website ID</param>
	/// <param name="filter"></param>
	/// <param name="chunkSize">The chunk size (TimeSpan)</param>
	internal Task<List<Alert>> GetWebsiteAlertsByIdChunkedAsync(
		int websiteId,
		AlertFilter filter,
		TimeSpan chunkSize)
	{
		// Ensure that the filter CANNOT override the website ID set!
		filter.RemoveMonitorObjectReferences();

		return GetAlertsChunkedAsync(filter, chunkSize, f => GetWebsiteAlertsByIdNormalAsync(websiteId, f, true, default));
	}

	/// <summary>
	///		Get website alerts with an alert filter (not all properties in the filter are used)
	/// </summary>
	/// <param name="websiteId">The Website ID</param>
	/// <param name="filter">An AlertFilter</param>
	/// <param name="calledFromChunked"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	/// <returns>A List of Alerts and whether the limit was reached</returns>
	internal Task<(List<Alert> alerts, bool limitReached)> GetWebsiteAlertsByIdNormalAsync(
		int websiteId,
		AlertFilter filter,
		bool calledFromChunked,
		CancellationToken cancellationToken)
		=> GetEntityAlertPagesAsync($"website/websites/{websiteId}/alerts", filter, calledFromChunked, cancellationToken);

	/// <summary>
	/// Get a list of SDTs for a Website
	/// </summary>
	public Task<Page<ScheduledDownTime>> GetWebsiteSdtListAsync(
		int websiteId,
		Filter<ScheduledDownTime> filter,
		CancellationToken cancellationToken)
		=> FilteredGetAsync($"website/websites/{websiteId}/sdts", filter, cancellationToken);
}
