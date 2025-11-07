using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.LogicModules;

public class EventSourceTests2(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetXml()
	{
		var eventSource = await LogicMonitorClient
			.GetByNameAsync<EventSource>("DNS A Record Check", CancellationToken);
		eventSource ??= new();
		var xml = await LogicMonitorClient
			.GetEventSourceXmlAsync(eventSource.Id, CancellationToken);
		xml.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllEventSources()
	{
		var eventSourcePage = await LogicMonitorClient
			.GetPageAsync(new Filter<EventSource> { Skip = 0, Take = 300 }, CancellationToken);

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
			.GetDataSourceOverviewGraphsPageAsync(eventSource.Id, new Filter<DataSourceGraph>(), CancellationToken);

			var testGraphs = await LogicMonitorClient
				.GetDataSourceGraphsAsync(eventSource.Id, CancellationToken);

			testGraphs.Should().NotBeNull();
		}

		Logger.LogInformation("{Message}", eventSourcesString);
	}

	[Fact]
	public async Task GetEventSourceByName()
	{
		var stopwatch = Stopwatch.StartNew();
		var eventSource = await LogicMonitorClient
			.GetByNameAsync<EventSource>("Windows System Event Log", CancellationToken);

		// Make sure that some are returned
		eventSource.Should().NotBeNull();
		eventSource ??= new();
		eventSource.AppliesTo.Should().NotBeNullOrWhiteSpace();
		// The whole thing should take less than 10 seconds
		stopwatch.ElapsedMilliseconds.Should().BeLessThan(20000);
	}

	[Fact]
	public async Task GetDeviceEventSources()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var deviceEventSources = await LogicMonitorClient
			.GetResourceEventSourcesPageAsync(
				device.Id,
				new Filter<ResourceEventSource> { Skip = 0, Take = 300 },
				CancellationToken);

		// Make sure that we have groups and they are not null
		deviceEventSources.Should().NotBeNull();

		foreach (var deviceEventSource in deviceEventSources.Items)
		{
			// Refetch
			var deviceDataSourceRefetch = await LogicMonitorClient
				.GetResourceEventSourceAsync(device.Id, deviceEventSource.Id, CancellationToken);

			// Make sure they are the same
			deviceDataSourceRefetch.ResourceId.Should().Be(device.Id);
		}
	}

	[Fact]
	public async Task GetDeviceEventSourceByIdAsync()
	{
		var eventsources = await LogicMonitorClient
			.GetResourceEventSourcesPageAsync(WindowsDeviceId, new Filter<ResourceEventSource>(), CancellationToken);

		var specificEventSource = eventsources.Items[0];

		var refetchedEventSource = await LogicMonitorClient
			.GetResourceEventSourceByResourceIdAndEventSourceIdAsync(WindowsDeviceId, specificEventSource.EventSourceId);

		refetchedEventSource.Should().Be(specificEventSource);
	}

	[Fact]
	public async Task GetFilteredEventSources()
	{
		const string groupName = "Integrator";
		var eventSources = await LogicMonitorClient.GetAllAsync(new Filter<EventSource>
		{
			FilterItems =
			[
				new Eq<EventSource>(nameof(EventSource.Group), groupName)
			]
		}, CancellationToken);

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
			.GetResourceEventSourcesPageAsync(WindowsDeviceId, new Filter<ResourceEventSource>(), CancellationToken);

		eventSource.Items.Should().NotBeNullOrEmpty();

		var groups = await LogicMonitorClient
			.GetResourceEventSourceGroupsPageAsync(WindowsDeviceId, eventSource.Items[0].EventSourceId, new Filter<ResourceEventSourceGroup>(), CancellationToken);
		groups.Should().NotBeNull();
	}
}
