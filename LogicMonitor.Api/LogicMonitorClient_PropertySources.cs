namespace LogicMonitor.Api;

/// <summary>
///    PropertySource portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets the JSON for a PropertySource (it is NOT XML!).
	/// </summary>
	/// <param name="propertySourceId">The PropertySource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<string> GetPropertySourceJsonAsync(
		int propertySourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/propertyrules/{propertySourceId}?format=xml", cancellationToken).ConfigureAwait(false))?.Content;
	// Can probably take off format=xml as this never returns XML anyway!
}
