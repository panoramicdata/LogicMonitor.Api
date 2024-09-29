namespace LogicMonitor.Api;

/// <summary>
///     EventSource portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets a list of all EventSources.
	/// </summary>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetAllAsync(Filter<EventSource>) instead.")]
	public Task<List<EventSource>> GetEventSourcesAsync(Filter<EventSource> filter, CancellationToken cancellationToken) => throw new NotSupportedException();

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
	/// Obsolete
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Obsolete("Use GetAllResourceGroupEventSourcesAsync instead", true)]
	public Task<List<ResourceGroupEventSource>> GetAllDeviceGroupEventSourcesAsync(
		int resourceGroupId,
		CancellationToken cancellationToken)
		=> GetAllResourceGroupEventSourcesAsync(resourceGroupId, cancellationToken);

	/// <summary>
	///     Gets a EventSource by name
	/// </summary>
	/// <param name="eventSourceName"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetByNameAsync<EventSource> instead", true)]
	public Task<EventSource> GetEventSourceByNameAsync(
		string eventSourceName,
		CancellationToken cancellationToken)
		=> throw new NotSupportedException();

	/// <summary>
	/// Gets a page of device EventSources
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DeviceEventSource>> GetDeviceEventSourcesPageAsync(
		int resourceId,
		Filter<DeviceEventSource> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DeviceEventSource>>($"device/devices/{resourceId}/deviceeventsources?{filter}", cancellationToken);

	/// <summary>
	///     Gets the deviceEventSource
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="deviceEventSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DeviceEventSource> GetDeviceEventSourceAsync(
		int resourceId,
		int deviceEventSourceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DeviceEventSource>($"device/devices/{resourceId}/deviceeventsources/{deviceEventSourceId}", cancellationToken);

	/// <summary>
	///     Gets a page of device EventSource groups
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="deviceEventSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DeviceEventSourceGroup>> GetDeviceEventSourceGroupsPageAsync(
		int resourceId,
		int deviceEventSourceId,
		Filter<DeviceEventSourceGroup> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DeviceEventSourceGroup>>($"device/devices/{resourceId}/deviceEventSources/{deviceEventSourceId}/groups?{filter}", cancellationToken);

	/// <summary>
	///     Gets a device event source
	/// </summary>
	/// <param name="resourceId"></param>
	/// <param name="eventSourceId"></param>
	public async Task<DeviceEventSource> GetDeviceEventSourceByDeviceIdAndEventSourceIdAsync(
		int resourceId,
		int eventSourceId)
	{
		// TODO - Make this use a search field
		var page = await GetDeviceEventSourcesPageAsync(resourceId, new Filter<DeviceEventSource> { Skip = 0, Take = 300 }, default).ConfigureAwait(false);
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
