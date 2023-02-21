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
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/eventsources/{eventSourceId}?format=xml", cancellationToken).ConfigureAwait(false))?.Content;

	/// <summary>
	///     Gets a list of EventSources that apply to a device group
	/// </summary>
	/// <param name="deviceGroupId">The device group Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<DeviceGroupEventSource>> GetAllDeviceGroupEventSourcesAsync(
		int deviceGroupId,
		CancellationToken cancellationToken)
		=> GetAllAsync<DeviceGroupEventSource>($"device/groups/{deviceGroupId}/eventsources", cancellationToken);

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
	/// <param name="deviceId">The Device Id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DeviceEventSource>> GetDeviceEventSourcesPageAsync(
		int deviceId,
		Filter<DeviceEventSource> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DeviceEventSource>>($"device/devices/{deviceId}/deviceeventsources?{filter}", cancellationToken);

	/// <summary>
	///     Gets the deviceEventSource
	/// </summary>
	/// <param name="deviceId">The Device Id</param>
	/// <param name="deviceEventSourceId"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<DeviceEventSource> GetDeviceEventSourceAsync(
		int deviceId,
		int deviceEventSourceId,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<DeviceEventSource>($"device/devices/{deviceId}/deviceeventsources/{deviceEventSourceId}", cancellationToken);

	/// <summary>
	///     Gets a page of device EventSource groups
	/// </summary>
	/// <param name="deviceId">The Device Id</param>
	/// <param name="deviceEventSourceId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<DeviceEventSourceGroup>> GetDeviceEventSourceGroupsPageAsync(
		int deviceId,
		int deviceEventSourceId,
		Filter<DeviceEventSourceGroup> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<DeviceEventSourceGroup>>($"device/devices/{deviceId}/deviceEventSources/{deviceEventSourceId}/groups?{filter}", cancellationToken);

	/// <summary>
	///     Gets a list of devices that a EventSource applies to
	/// </summary>
	/// <param name="deviceGroupId">The device group id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<EventSourceAppliesToCollection>> GetEventSourceAppliesToCollectionsPageAsync(
		int deviceGroupId,
		Filter<EventSourceAppliesToCollection> filter,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<EventSourceAppliesToCollection>>(
			$"device/groups/{deviceGroupId}/eventSources?{filter}",
			cancellationToken);

	/// <summary>
	///     Gets a device data source
	/// </summary>
	/// <param name="deviceId"></param>
	/// <param name="eventSourceId"></param>
	public async Task<DeviceEventSource> GetDeviceEventSourceByDeviceIdAndEventSourceIdAsync(int deviceId, int eventSourceId)
	{
		// TODO - Make this use a search field
		var page = await GetDeviceEventSourcesPageAsync(deviceId, new Filter<DeviceEventSource> { Skip = 0, Take = 300 }, CancellationToken.None).ConfigureAwait(false);
		return page.Items.SingleOrDefault(deviceEventSource => deviceEventSource.EventSourceId == eventSourceId);
	}

	/// <summary>
	/// add audit version
	/// </summary>
	/// <param name="id">The id</param>
	/// <param name="body">The audit to be added</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<EventSource> AddEventsourceAuditVersion(
		int id,
		Audit body,
		CancellationToken cancellationToken) => PostAsync<Audit, EventSource>(body,
			$"setting/eventsources/{id}/audit",
			cancellationToken);
}
