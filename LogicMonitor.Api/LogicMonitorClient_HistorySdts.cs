﻿
namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets history SDTs for a Device
	/// </summary>
	/// <param name="deviceId">The Device ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<HistorySdtCollection> GetDeviceHistorySdts(
		int deviceId,
		CancellationToken cancellationToken)
	=> await GetBySubUrlAsync<HistorySdtCollection>($"device/devices/{deviceId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Gets history SDTs for a Device Group
	/// </summary>
	/// <param name="deviceGroupId">The Device Group ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<HistorySdtCollection> GetDeviceGroupHistorySdts(
		int deviceGroupId,
		CancellationToken cancellationToken)
	=> await GetBySubUrlAsync<HistorySdtCollection>($"device/groups/{deviceGroupId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Gets history SDTs for a Device Data Source
	/// </summary>
	/// <param name="deviceId">The Device ID</param>
	/// <param name="deviceDataSourceId">The Device Data Source ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<HistorySdtCollection> GetDeviceDataSourceHistorySdts(
		int deviceId,
		int deviceDataSourceId,
		CancellationToken cancellationToken)
	=> await GetBySubUrlAsync<HistorySdtCollection>($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Gets history SDTs for a Device Data Source Instance
	/// </summary>
	/// <param name="deviceId">The Device ID</param>
	/// <param name="deviceDataSourceId">The Device Data Source ID</param>
	/// <param name="deviceDataSourceInstanceId">The Device Data Source Instance ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<HistorySdtCollection> GetDeviceDataSourceInstanceHistorySdts(
		int deviceId,
		int deviceDataSourceId,
		int deviceDataSourceInstanceId,
		CancellationToken cancellationToken)
	=> await GetBySubUrlAsync<HistorySdtCollection>($"device/devices/{deviceId}/devicedatasources/{deviceDataSourceId}/instances/{deviceDataSourceInstanceId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// get a list of SDTs for a website group (Response may contain extra fields depending upon the type of SDT)
	/// </summary>
	/// <param name="id">The Website Group ID</param>
	/// <param name="fields"></param>
	/// <param name="size"></param>
	/// <param name="offset"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<SDTPaginationResponse> GetAllSDTListByWebsiteGroupIdAsync(
		int id,
		CancellationToken cancellationToken,
		string? fields = null,
		int size = 50,
		int offset = 0,
		string? filter = null)
	=> await GetBySubUrlAsync<SDTPaginationResponse>($"website/groups/{id}/sdts?fields={fields}&size={size}&offset={offset}&filter={filter}", cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// get SDT history for the website (Response may contain extra fields depending upon the type of SDT)
	/// </summary>
	/// <param name="id">The Website ID</param>
	/// <param name="fields">Fields</param>
	/// <param name="size"></param>
	/// <param name="offset"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<WebsiteSDTHistoryPaginationResponse> GetSDTHistoryByWebsiteIdAsync(
		int id,
		CancellationToken cancellationToken,
		string? fields = null,
		int size = 50,
		int offset = 0,
		string? filter = null)
	=> await GetBySubUrlAsync<WebsiteSDTHistoryPaginationResponse>($"website/websites/{id}/historysdts?fields={fields}&size={size}&offset={offset}&filter={filter}", cancellationToken).ConfigureAwait(false);
}
