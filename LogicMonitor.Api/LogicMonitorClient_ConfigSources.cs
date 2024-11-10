namespace LogicMonitor.Api;

/// <summary>
///    ConfigSource portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets the XML for a ConfigSource.
	/// </summary>
	/// <param name="configSourceId">The ConfigSource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<string> GetConfigSourceXmlAsync(
		int configSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/configsources/{configSourceId}?format=xml", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///	add audit version
	/// </summary>
	/// <param name="id">The ConfigSource id</param>
	/// <param name="body">The body</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task AddConfigSourceAuditVersionAsync(
		int id,
		Audit body,
		CancellationToken cancellationToken
		) => PostAsync<Audit, ConfigSource>(
			body,
			$"setting/configsources/{id}/audit", cancellationToken);
}
