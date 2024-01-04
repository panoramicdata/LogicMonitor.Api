namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets history SDTs for a Device
	/// </summary>
	/// <param name="deviceId">The Device ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<List<ScheduledDownTimeHistory>> GetDeviceHistorySdtsAsync(
		int deviceId,
		CancellationToken cancellationToken)
	=> await GetAllAsync<ScheduledDownTimeHistory>($"device/devices/{deviceId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Gets history SDTs for a Device Group
	/// </summary>
	/// <param name="deviceGroupId">The Device Group ID</param>
	/// <param name="cancellationToken">The Cancellation Token</param>
	public async Task<List<ScheduledDownTimeHistory>> GetDeviceGroupHistorySdtsAsync(
		int deviceGroupId,
		CancellationToken cancellationToken)
	=> await GetAllAsync<ScheduledDownTimeHistory>($"device/groups/{deviceGroupId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Gets history SDTs for a Device Data Source
	/// </summary>
	/// <param name="deviceId">The Device ID</param>
	/// <param name="deviceDataSourceId">The Device Data Source ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<List<ScheduledDownTimeHistory>> GetDeviceDataSourceHistorySdtsAsync(
		int deviceId,
		int deviceDataSourceId,
		CancellationToken cancellationToken)
	=> await GetAllAsync<ScheduledDownTimeHistory>($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Gets history SDTs for a Device Data Source Instance
	/// </summary>
	/// <param name="deviceId">The Device ID</param>
	/// <param name="deviceDataSourceId">The Device Data Source ID</param>
	/// <param name="deviceDataSourceInstanceId">The Device Data Source Instance ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<List<ScheduledDownTimeHistory>> GetDeviceDataSourceInstanceHistorySdtsAsync(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceId,
		CancellationToken cancellationToken)
	=> await GetAllAsync<ScheduledDownTimeHistory>($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// Get a list of SDTs for a website group (Response may contain extra fields depending upon the type of SDT)
	/// </summary>
	/// <param name="id">The Website Group ID</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<Page<ScheduledDownTime>> GetAllSdtListByWebsiteGroupIdAsync(
		int id,
		Filter<ScheduledDownTime> filter,
		CancellationToken cancellationToken)
	=> await FilteredGetAsync($"website/groups/{id}/sdts", filter, cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// Get SDT histories for a website group (Response may contain extra fields depending upon the type of SDT)
	/// </summary>
	/// <param name="id">The Website Group ID</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<Page<ScheduledDownTimeHistory>> GetSdtHistoryListByWebsiteGroupIdAsync(
		int id,
		Filter<ScheduledDownTimeHistory> filter,
		CancellationToken cancellationToken)
	=> await FilteredGetAsync($"website/groups/{id}/historysdts", filter, cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// Get SDT history for the website (Response may contain extra fields depending upon the type of SDT)
	/// </summary>
	/// <param name="id">The Website ID</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<Page<ScheduledDownTimeHistory>> GetSdtHistoryByWebsiteIdAsync(
		int id,
		Filter<ScheduledDownTimeHistory> filter,
		CancellationToken cancellationToken)
	=> await FilteredGetAsync($"website/websites/{id}/historysdts", filter, cancellationToken).ConfigureAwait(false);
}