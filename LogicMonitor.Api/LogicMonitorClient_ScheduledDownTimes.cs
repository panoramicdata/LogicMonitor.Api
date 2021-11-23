namespace LogicMonitor.Api;

/// <summary>
///    Scheduled Down Time portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///    Get Scheduled Down Times
	/// </summary>
	/// <param name="scheduledDownTimeFilter">The SDT filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>Existing, future and historic scheduled down times based on the provided filter.</returns>
	[Obsolete("Use GetAsync(Filter<ScheduledDownTime>) instead", true)]
	public async Task<List<ScheduledDownTime>> GetScheduledDownTimesAsync(
		ScheduledDownTimeFilter scheduledDownTimeFilter,
		CancellationToken cancellationToken = default)
		=> (await FilteredGetAsync("sdt/sdts", scheduledDownTimeFilter.GetFilter(), cancellationToken).ConfigureAwait(false)).Items;
}
