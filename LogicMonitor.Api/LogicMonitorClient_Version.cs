using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
	public Task<PortalVersion> GetVersionAsync(CancellationToken cancellationToken = default)
		=> GetVersionAsync(AccountName, cancellationToken);

	/// <summary>
	///    Gets the current portal's version.
	/// </summary>
	/// <param name="accountName"></param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public static async Task<PortalVersion> GetVersionAsync(string accountName, CancellationToken cancellationToken = default)
	{
		// The actual Get method
		using var _httpClient = new HttpClient();
		using var result = await _httpClient.GetAsync($"https://{accountName}.logicmonitor.com/santaba/rest/version", cancellationToken).ConfigureAwait(false);
		var portalResponse = new PortalResponse<PortalVersion>(result);
		return portalResponse.GetObject();
	}
}
