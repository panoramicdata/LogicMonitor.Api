namespace LogicMonitor.Api;

/// <summary>
///     EventSource portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets the XML for an EventSource.
	/// </summary>
	/// <param name="eventSourceId">The EventSource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<string> GetEventSourceXmlAsync(int eventSourceId, CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/eventsources/{eventSourceId}?format=xml", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets a list of EventSources that apply to a ResourceGroup
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<ResourceGroupEventSource>> GetAllResourceGroupEventSourcesAsync(
		int resourceGroupId,
		CancellationToken cancellationToken)
		=> GetAllAsync<ResourceGroupEventSource>($"device/groups/{resourceGroupId}/eventsources", cancellationToken);

	/// <summary>
	/// Gets a page of device EventSources
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ResourceEventSource>> GetDeviceEventSourcesPageAsync(
		int resourceId,
		Filter<ResourceEventSource> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<ResourceEventSource>>($"device/devices/{resourceId}/deviceeventsources?{filter}", cancellationToken);

	/// <summary>
	///     Gets the deviceEventSource
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="deviceEventSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<ResourceEventSource> GetDeviceEventSourceAsync(
		int resourceId,
		int deviceEventSourceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<ResourceEventSource>($"device/devices/{resourceId}/deviceeventsources/{deviceEventSourceId}", cancellationToken);

	/// <summary>
	///     Gets a page of device EventSource groups
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="deviceEventSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<ResourceEventSourceGroup>> GetDeviceEventSourceGroupsPageAsync(
		int resourceId,
		int deviceEventSourceId,
		Filter<ResourceEventSourceGroup> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<ResourceEventSourceGroup>>($"device/devices/{resourceId}/deviceEventSources/{deviceEventSourceId}/groups?{filter}", cancellationToken);

	/// <summary>
	///     Gets a device event source
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="eventSourceId"></param>
	public async Task<ResourceEventSource> GetDeviceEventSourceByDeviceIdAndEventSourceIdAsync(
		int resourceId,
		int eventSourceId)
	{
		// TODO - Make this use a search field
		var page = await GetDeviceEventSourcesPageAsync(resourceId, new Filter<ResourceEventSource> { Skip = 0, Take = 300 }, default).ConfigureAwait(false);
		return page.Items.SingleOrDefault(deviceEventSource => deviceEventSource.EventSourceId == eventSourceId);
	}

	/// <summary>
	/// add audit version
	/// </summary>
	/// <param name="id">The id</param>
	/// <param name="body">The audit to be added</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<EventSource> AddEventSourceAuditVersionAsync(
		int id,
		Audit body,
		CancellationToken cancellationToken) => PostAsync<Audit, EventSource>(body,
			$"setting/eventsources/{id}/audit",
			cancellationToken);
}
