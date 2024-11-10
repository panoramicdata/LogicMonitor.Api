namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets history SDTs for a Resource
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public Task<List<ScheduledDownTimeHistory>> GetResourceHistorySdtsAsync(
		int resourceId,
		CancellationToken cancellationToken)
	=> GetAllAsync<ScheduledDownTimeHistory>($"device/devices/{resourceId}/historysdts", cancellationToken);

	/// <summary>
	///     Gets history SDTs for a ResourceGroup
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup ID</param>
	/// <param name="cancellationToken">The Cancellation Token</param>
	public Task<List<ScheduledDownTimeHistory>> GetResourceGroupHistorySdtsAsync(
		int resourceGroupId,
		CancellationToken cancellationToken)
	=> GetAllAsync<ScheduledDownTimeHistory>($"device/groups/{resourceGroupId}/historysdts", cancellationToken);

	/// <summary>
	///     Gets history SDTs for a Resource Data Source
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="resourceDataSourceId">The Device Data Source ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public Task<List<ScheduledDownTimeHistory>> GetDeviceDataSourceHistorySdtsAsync(
		int resourceId,
		int resourceDataSourceId,
		CancellationToken cancellationToken)
	=> GetAllAsync<ScheduledDownTimeHistory>($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/historysdts", cancellationToken);

	/// <summary>
	///     Gets history SDTs for a Resource Data Source Instance
	/// </summary>
	/// <param name="resourceId">The Resource ID</param>
	/// <param name="resourceDataSourceId">The Resource Data Source ID</param>
	/// <param name="resourceDataSourceInstanceId">The Resource Data Source Instance ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public Task<List<ScheduledDownTimeHistory>> GetDeviceDataSourceInstanceHistorySdtsAsync(
		int resourceId,
		int resourceDataSourceId,
		int resourceDataSourceInstanceId,
		CancellationToken cancellationToken)
	=> GetAllAsync<ScheduledDownTimeHistory>($"device/devices/{resourceId}/devicedatasources/{resourceDataSourceId}/instances/{resourceDataSourceInstanceId}/historysdts", cancellationToken);

	/// <summary>
	/// Get a list of SDTs for a website group (Response may contain extra fields depending upon the type of SDT)
	/// </summary>
	/// <param name="id">The Website Group ID</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public Task<Page<ScheduledDownTime>> GetAllSdtListByWebsiteGroupIdAsync(
		int id,
		Filter<ScheduledDownTime> filter,
		CancellationToken cancellationToken)
	=> FilteredGetAsync($"website/groups/{id}/sdts", filter, cancellationToken);

	/// <summary>
	/// Get SDT histories for a website group (Response may contain extra fields depending upon the type of SDT)
	/// </summary>
	/// <param name="id">The Website Group ID</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public Task<Page<ScheduledDownTimeHistory>> GetSdtHistoryListByWebsiteGroupIdAsync(
		int id,
		Filter<ScheduledDownTimeHistory> filter,
		CancellationToken cancellationToken)
	=> FilteredGetAsync($"website/groups/{id}/historysdts", filter, cancellationToken);

	/// <summary>
	/// Get SDT history for the website (Response may contain extra fields depending upon the type of SDT)
	/// </summary>
	/// <param name="id">The Website ID</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public Task<Page<ScheduledDownTimeHistory>> GetSdtHistoryByWebsiteIdAsync(
		int id,
		Filter<ScheduledDownTimeHistory> filter,
		CancellationToken cancellationToken)
	=> FilteredGetAsync($"website/websites/{id}/historysdts", filter, cancellationToken);
}