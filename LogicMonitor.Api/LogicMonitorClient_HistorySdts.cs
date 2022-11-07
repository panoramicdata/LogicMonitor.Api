
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
	///     Gets history SDTs for a Website Group
	/// </summary>
	/// <param name="websiteGroupId">The Website Group ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<HistorySdtCollection> GetWebsiteGroupHistorySdts(
		int websiteGroupId,
		CancellationToken cancellationToken)
	=> await GetBySubUrlAsync<HistorySdtCollection>($"website/groups/{websiteGroupId}/historysdts", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Gets history SDTs for a Website
	/// </summary>
	/// <param name="websiteId">The Website ID</param>
	/// <param name="cancellationToken">The CancellationToken</param>
	public async Task<HistorySdtCollection> GetWebsiteHistorySdts(
		int websiteId,
		CancellationToken cancellationToken)
	=> await GetBySubUrlAsync<HistorySdtCollection>($"website/websites/{websiteId}/historysdts", cancellationToken).ConfigureAwait(false);
}
