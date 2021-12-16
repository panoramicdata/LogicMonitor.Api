namespace LogicMonitor.Api.Test;

public class EventSourceTests : TestWithOutput
{
	public EventSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetXml()
	{
		var eventSource = await LogicMonitorClient.GetByNameAsync<EventSource>("DNS A Record Check").ConfigureAwait(false);
		var xml = await LogicMonitorClient.GetEventSourceXmlAsync(eventSource.Id).ConfigureAwait(false);
		Assert.NotNull(xml);
	}

	[Fact]
	public async void GetAllEventSources()
	{
		var eventSourcePage = await LogicMonitorClient.GetPageAsync(new Filter<EventSource> { Skip = 0, Take = 300 }).ConfigureAwait(false);

		// Make sure that some are returned
		Assert.True(eventSourcePage.Items.Count > 0);

		// Make sure that all have Unique Ids
		Assert.False(eventSourcePage.Items.Select(c => c.Id).HasDuplicates());

		// Check each one
		var eventSourcesString = string.Empty;
		foreach (var eventSource in eventSourcePage.Items)
		{
			// TODO
			//dataSourcesString += $"{dataSource.Name} / {dataSource.DisplayedAs}\r\n";
			//TestOverviewGraphs(dataSource);
			//TestGraphs(dataSource);
		}

		Logger.LogInformation(eventSourcesString);
	}

	[Fact]
	public async void GetEventSourceByName()
	{
		var stopwatch = Stopwatch.StartNew();
		var eventSource = await LogicMonitorClient.GetByNameAsync<EventSource>("Windows System Event Log").ConfigureAwait(false);

		// Make sure that some are returned
		Assert.NotNull(eventSource);
		Assert.False(string.IsNullOrWhiteSpace(eventSource.AppliesTo));
		// The whole thing should take less than 10 seconds
		Assert.InRange(stopwatch.ElapsedMilliseconds, 0, 20000);
	}

	[Fact]
	public async void GetDeviceEventSources()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var deviceEventSources = await LogicMonitorClient.GetDeviceEventSourcesPageAsync(device.Id, new Filter<DeviceEventSource> { Skip = 0, Take = 300 }).ConfigureAwait(false);

		// Make sure that we have groups and they are not null
		Assert.NotNull(deviceEventSources);

		foreach (var deviceEventSource in deviceEventSources.Items)
		{
			// Refetch
			var deviceDataSourceRefetch = await LogicMonitorClient.GetDeviceEventSourceAsync(device.Id, deviceEventSource.Id).ConfigureAwait(false);

			// Make sure they are the same
			Assert.Equal(device.Id, deviceDataSourceRefetch.DeviceId);
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
		Assert.NotNull(eventSources);
		Assert.NotEmpty(eventSources);
		Assert.True(eventSources.Count < 20);

		// Make sure that they match the expected group
		Assert.True(eventSources.All(item => item.Group == groupName));

		// The whole thing should take less than 5 seconds
		AssertIsFast(5);
	}
}
