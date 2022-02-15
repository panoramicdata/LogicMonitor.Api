namespace LogicMonitor.Api.Test;

[Collection("CollectorRelated")]
public class CollectorTests
	: TestWithOutput
{
	public CollectorTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAllCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient.GetAllAsync<CollectorGroup>().ConfigureAwait(false);
		Assert.NotNull(collectorGroups);
		Assert.True(collectorGroups.Count > 0);

		// Re-fetch each
		foreach (var collectorGroup in collectorGroups)
		{
			var refetch = await LogicMonitorClient.GetAsync<CollectorGroup>(collectorGroup.Id).ConfigureAwait(false);
			Assert.NotNull(refetch);
		}
	}

	[Fact]
	public async void GetAllCollectors()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>().ConfigureAwait(false);
		Assert.NotNull(collectors);
		Assert.True(collectors.Count > 0);

		// Re-fetch each
		foreach (var collector in collectors)
		{
			var refetch = await LogicMonitorClient.GetAsync<Collector>(collector.Id).ConfigureAwait(false);
			Assert.NotNull(refetch);
		}
	}

	[Fact]
	public async void RunDebugCommand()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>().ConfigureAwait(false);
		var testCollector = collectors.Find(c => !c.IsDown);
		Assert.NotNull(testCollector);
		var response = await LogicMonitorClient.ExecuteDebugCommandAndWaitForResultAsync(testCollector.Id, "!ping 8.8.8.8").ConfigureAwait(false);
		Assert.NotNull(response);
		Logger.LogInformation("{Output}", response.Output);
	}

	[Fact]
	public async void CreateCollectorDownloadAndDelete()
	{
		// Determine the latest supported version
		var collectorVersionInts = (await LogicMonitorClient.GetAllCollectorVersionsAsync(
			new Filter<CollectorVersion>
			{
				FilterItems = new List<FilterItem<CollectorVersion>>
				{
						new Eq<CollectorVersion>(nameof(CollectorVersion.IsStable), true),
				}
			}
			).ConfigureAwait(false))
			.Select(cv => (cv.MajorVersion * 1000) + cv.MinorVersion)
			.OrderByDescending(v => v)
			.ToList();

		Assert.NotNull(collectorVersionInts);
		Assert.NotEmpty(collectorVersionInts);

		var collectorVersionInt = collectorVersionInts[0];

		// Create the collector
		var collector = await LogicMonitorClient.CreateAsync(new CollectorCreationDto { Description = "UNIT TEST" }).ConfigureAwait(false);

		var tempFileInfo = new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString());
		try
		{
			await LogicMonitorClient.DownloadCollector(
				collector.Id,
				tempFileInfo,
				CollectorPlatformAndArchitecture.Win64,
				CollectorDownloadType.Bootstrap,
				CollectorSize.Medium,
				collectorVersionInt).ConfigureAwait(false);
		}
		finally
		{
			// No need to do this as the collector has not been registered
			// await DefaultPortalClient.DeleteAsync(collector).ConfigureAwait(false);
			tempFileInfo.Delete();

			// Remove the collector from the API
			await LogicMonitorClient.DeleteAsync(collector).ConfigureAwait(false);
		}
	}
}
