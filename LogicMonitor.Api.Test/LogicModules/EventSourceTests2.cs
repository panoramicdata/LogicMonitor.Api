using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test;

public class EventSourceTests2 : TestWithOutput
{
	public EventSourceTests2(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetXml()
	{
		var eventSource = await LogicMonitorClient.GetByNameAsync<EventSource>("DNS A Record Check").ConfigureAwait(false);
		var xml = await LogicMonitorClient.GetEventSourceXmlAsync(eventSource.Id).ConfigureAwait(false);
		xml.Should().NotBeNull();
	}

	[Fact]
	public async void GetAllEventSources()
	{
		var eventSourcePage = await LogicMonitorClient.GetPageAsync(new Filter<EventSource> { Skip = 0, Take = 300 }).ConfigureAwait(false);

		// Make sure that some are returned
		eventSourcePage.Items.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		eventSourcePage.Items.Select(c => c.Id).HasDuplicates().Should().BeFalse();

		// Check each one
		var eventSourcesString = string.Empty;
		foreach (var eventSource in eventSourcePage.Items)
		{
			// TODO
			//dataSourcesString += $"{dataSource.Name} / {dataSource.DisplayedAs}\r\n";
			//TestOverviewGraphs(dataSource);
			//TestGraphs(dataSource);
		}

		Logger.LogInformation("{Message}", eventSourcesString);
	}

	[Fact]
	public async void GetEventSourceByName()
	{
		var stopwatch = Stopwatch.StartNew();
		var eventSource = await LogicMonitorClient.GetByNameAsync<EventSource>("Windows System Event Log").ConfigureAwait(false);

		// Make sure that some are returned
		eventSource.Should().NotBeNull();
		string.IsNullOrWhiteSpace(eventSource.AppliesTo).Should().BeFalse();
		// The whole thing should take less than 10 seconds
		stopwatch.ElapsedMilliseconds.Should().BeLessThan(20000);
	}

	[Fact]
	public async void GetDeviceEventSources()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var deviceEventSources = await LogicMonitorClient.GetDeviceEventSourcesPageAsync(device.Id, new Filter<DeviceEventSource> { Skip = 0, Take = 300 }).ConfigureAwait(false);

		// Make sure that we have groups and they are not null
		deviceEventSources.Should().NotBeNull();

		foreach (var deviceEventSource in deviceEventSources.Items)
		{
			// Refetch
			var deviceDataSourceRefetch = await LogicMonitorClient.GetDeviceEventSourceAsync(device.Id, deviceEventSource.Id).ConfigureAwait(false);

			// Make sure they are the same
			deviceDataSourceRefetch.DeviceId.Should().Be(device.Id);
		}
	}

	[Fact]
	public async void GetFilteredEventSources()
	{
		const string groupName = "Integrator";
		var eventSources = await LogicMonitorClient.GetAllAsync(new Filter<EventSource>
		{
			FilterItems = new List<FilterItem<EventSource>>
				{
					new Eq<EventSource>(nameof(EventSource.Group), groupName)
				}
		}).ConfigureAwait(false);

		// Make sure that some are returned
		eventSources.Should().NotBeNull();
		eventSources.Should().NotBeNullOrEmpty();
		eventSources.Should().HaveCountLessThan(20);

		// Make sure that they match the expected group
		eventSources.Should().AllSatisfy(item => item.Group.Should().Be(groupName));

		// The whole thing should take less than 5 seconds
		AssertIsFast(5);
	}
}
