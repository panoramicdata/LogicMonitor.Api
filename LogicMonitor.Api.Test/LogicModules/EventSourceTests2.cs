using LogicMonitor.Api.LogicModules;
using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test;

public class EventSourceTests2 : TestWithOutput
{
	public EventSourceTests2(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetXml()
	{
		var eventSource = await LogicMonitorClient.GetByNameAsync<EventSource>("DNS A Record Check", default).ConfigureAwait(false);
		eventSource ??= new();
		var xml = await LogicMonitorClient.GetEventSourceXmlAsync(eventSource.Id, default).ConfigureAwait(false);
		xml.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllEventSources()
	{
		var eventSourcePage = await LogicMonitorClient.GetPageAsync(new Filter<EventSource> { Skip = 0, Take = 300 }, default).ConfigureAwait(false);

		// Make sure that some are returned
		eventSourcePage.Items.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		eventSourcePage.Items.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Check each one
		var eventSourcesString = string.Empty;
		foreach (var eventSource in eventSourcePage.Items)
		{
			eventSourcesString += $"{eventSource.Name}\r\n";

			var overviewGraphs = await LogicMonitorClient
			.GetDataSourceOverviewGraphsPageAsync(eventSource.Id, new Filter<DataSourceOverviewGraph>(), default)
			.ConfigureAwait(false);

			var testGraphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(eventSource.Id, default)
				.ConfigureAwait(false);

			testGraphs.Should().NotBeNull();
		}

		Logger.LogInformation("{Message}", eventSourcesString);
	}

	[Fact]
	public async Task GetEventSourceByName()
	{
		var stopwatch = Stopwatch.StartNew();
		var eventSource = await LogicMonitorClient.GetByNameAsync<EventSource>("Windows System Event Log", default).ConfigureAwait(false);

		// Make sure that some are returned
		eventSource.Should().NotBeNull();
		eventSource ??= new();
		string.IsNullOrWhiteSpace(eventSource.AppliesTo).Should().BeFalse();
		// The whole thing should take less than 10 seconds
		stopwatch.ElapsedMilliseconds.Should().BeLessThan(20000);
	}

	[Fact]
	public async Task GetDeviceEventSources()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(false);
		var deviceEventSources = await LogicMonitorClient
			.GetDeviceEventSourcesPageAsync(
				device.Id,
				new Filter<DeviceEventSource> { Skip = 0, Take = 300 },
				default)
			.ConfigureAwait(false);

		// Make sure that we have groups and they are not null
		deviceEventSources.Should().NotBeNull();

		foreach (var deviceEventSource in deviceEventSources.Items)
		{
			// Refetch
			var deviceDataSourceRefetch = await LogicMonitorClient.GetDeviceEventSourceAsync(device.Id, deviceEventSource.Id, default).ConfigureAwait(false);

			// Make sure they are the same
			deviceDataSourceRefetch.DeviceId.Should().Be(device.Id);
		}
	}

	[Fact]
	public async Task GetDeviceEventSourceByIdAsync()
	{
		var eventsources = await LogicMonitorClient
			.GetDeviceEventSourcesPageAsync(WindowsDeviceId, new Filter<DeviceEventSource>(), default)
			.ConfigureAwait(false);

		var specificEventSource = eventsources.Items[0];

		var refetchedEventSource = await LogicMonitorClient
			.GetDeviceEventSourceByDeviceIdAndEventSourceIdAsync(WindowsDeviceId, specificEventSource.EventSourceId)
			.ConfigureAwait(false);

		refetchedEventSource.Should().Be(specificEventSource);
	}

	[Fact]
	public async Task GetFilteredEventSources()
	{
		const string groupName = "Integrator";
		var eventSources = await LogicMonitorClient.GetAllAsync(new Filter<EventSource>
		{
			FilterItems = new List<FilterItem<EventSource>>
				{
					new Eq<EventSource>(nameof(EventSource.Group), groupName)
				}
		}, default).ConfigureAwait(false);

		// Make sure that some are returned
		eventSources.Should().NotBeNull();
		eventSources.Should().NotBeNullOrEmpty();
		eventSources.Should().HaveCountLessThan(20);

		// Make sure that they match the expected group
		eventSources.Should().AllSatisfy(item => item.Group.Should().Be(groupName));

		// The whole thing should take less than 5 seconds
		AssertIsFast(5);
	}

	[Fact]
	public async Task GetEventSourceGroupsAsync()
	{
		var eventSource = await LogicMonitorClient
			.GetDeviceEventSourcesPageAsync(WindowsDeviceId, new Filter<DeviceEventSource>(), default)
			.ConfigureAwait(false);

		var groups = await LogicMonitorClient
			.GetDeviceEventSourceGroupsPageAsync(WindowsDeviceId, eventSource.Items[0].EventSourceId, new Filter<DeviceEventSourceGroup>(), default)
			.ConfigureAwait(false);
	}
}
