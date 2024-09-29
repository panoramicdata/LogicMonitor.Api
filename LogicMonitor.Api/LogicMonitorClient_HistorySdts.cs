﻿namespace LogicMonitor.Api;

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
	///	Obsolete
	///	</summary>
	[Obsolete("Use GetResourceHistorySdtsAsync instead", true)]
	public Task<List<ScheduledDownTimeHistory>> GetDeviceHistorySdtsAsync(
		int resourceId,
		CancellationToken cancellationToken)
		=> GetResourceHistorySdtsAsync(resourceId, cancellationToken);

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
	/// Obsolete
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetResourceGroupHistorySdtsAsync instead", true)]
	public Task<List<ScheduledDownTimeHistory>> GetDeviceGroupHistorySdtsAsync(
		int resourceGroupId,
		CancellationToken cancellationToken)
		=> GetResourceGroupHistorySdtsAsync(resourceGroupId, cancellationToken);

	/// <summary>
	///     Gets history SDTs for a Device Data Source
	/// </summary>
	/// <param name="resourceId">The Device ID</param>
	/// <param name="deviceDataSourceId">The Device Data Source ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public Task<List<ScheduledDownTimeHistory>> GetDeviceDataSourceHistorySdtsAsync(
		int resourceId,
		int deviceDataSourceId,
		CancellationToken cancellationToken)
	=> GetAllAsync<ScheduledDownTimeHistory>($"device/devices/{resourceId}/devicedatasources/{deviceDataSourceId}/historysdts", cancellationToken);

	/// <summary>
	///     Gets history SDTs for a Device Data Source Instance
	/// </summary>
	/// <param name="deviceId">The Device ID</param>
	/// <param name="deviceDataSourceId">The Device Data Source ID</param>
	/// <param name="deviceDataSourceInstanceId">The Device Data Source Instance ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public Task<List<ScheduledDownTimeHistory>> GetDeviceDataSourceInstanceHistorySdtsAsync(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceId,
		CancellationToken cancellationToken)
	=> GetAllAsync<ScheduledDownTimeHistory>($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/historysdts", cancellationToken);

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