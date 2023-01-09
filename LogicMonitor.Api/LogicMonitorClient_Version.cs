namespace LogicMonitor.Api;

/// <summary>
///    Websites Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///    Gets the current portal's version.
	/// </summary>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public Task<PortalVersion> GetVersionAsync(CancellationToken cancellationToken)
		=> GetVersionAsync(AccountName, cancellationToken);

	/// <summary>
	///    Gets the current portal's version.
	/// </summary>
	/// <param name="accountName"></param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public async Task<PortalVersion> GetVersionAsync(string accountName, CancellationToken cancellationToken)
	{
		// The actual Get method
		using var _httpClient = new HttpClient();
		using var request = new HttpRequestMessage(HttpMethod.Get, $"https://{accountName}.logicmonitor.com/santaba/rest/version");
		using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
		UpdateSummary(request, response);
		var portalResponse = new PortalResponse<PortalVersion>(response);
		return portalResponse.GetObject();
	}
}
