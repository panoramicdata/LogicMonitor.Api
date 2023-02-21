namespace LogicMonitor.Api.Test;

[Collection("CollectorRelated")]
public class CollectorTests2
	: TestWithOutput
{
	public CollectorTests2(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient.GetAllAsync<CollectorGroup>(default).ConfigureAwait(false);
		collectorGroups.Should().NotBeNullOrEmpty();

		// Re-fetch each
		foreach (var collectorGroup in collectorGroups)
		{
			var refetch = await LogicMonitorClient.GetAsync<CollectorGroup>(collectorGroup.Id, default).ConfigureAwait(false);
			refetch.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetAllCollectors()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>(default).ConfigureAwait(false);
		collectors.Should().NotBeNullOrEmpty();

		// Re-fetch each
		foreach (var collector in collectors)
		{
			var refetch = await LogicMonitorClient.GetAsync<Collector>(collector.Id, default).ConfigureAwait(false);
			refetch.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task RunDebugCommand()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>(default).ConfigureAwait(false);
		var testCollector = collectors.Find(c => !c.IsDown);
		testCollector.Should().NotBeNull();
		var response = await LogicMonitorClient
			.ExecuteDebugCommandAndWaitForResultAsync(testCollector.Id, "!ping 8.8.8.8", cancellationToken: default)
			.ConfigureAwait(false);
		response.Should().NotBeNull();
		Logger.LogInformation("{Output}", response.Output);
	}

	[Fact]
	public async Task CreateCollectorDownloadAndDelete()
	{
		// Determine the latest supported version
		var collectorVersionInts = (await LogicMonitorClient.GetAllCollectorVersionsAsync(
			new Filter<CollectorVersion>
			{
				FilterItems = new List<FilterItem<CollectorVersion>>
				{
						new Eq<CollectorVersion>(nameof(CollectorVersion.IsStable), true),
				}
			}, default
			).ConfigureAwait(false))
			.Select(cv => (cv.MajorVersion * 1000) + cv.MinorVersion)
			.OrderByDescending(v => v)
			.ToList();

		collectorVersionInts.Should().NotBeNull();
		collectorVersionInts.Should().NotBeNullOrEmpty();

		var collectorVersionInt = collectorVersionInts[0];

		// Create the collector
		var collector = await LogicMonitorClient.CreateAsync(new CollectorCreationDto { Description = "UNIT TEST" }, default).ConfigureAwait(false);

		var tempFileInfo = new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString());
		try
		{
			await LogicMonitorClient.DownloadCollector(
				collector.Id,
				tempFileInfo,
				CollectorPlatformAndArchitecture.Win64,
				CollectorDownloadType.Bootstrap,
				CollectorSize.Medium,
				collectorVersionInt, default).ConfigureAwait(false);
		}
		finally
		{
			// No need to do this as the collector has not been registered
			// await DefaultPortalClient.DeleteAsync(collector).ConfigureAwait(false);
			tempFileInfo.Delete();

			// Remove the collector from the API
			await LogicMonitorClient.DeleteAsync(collector, cancellationToken: default).ConfigureAwait(false);
		}
	}
}
