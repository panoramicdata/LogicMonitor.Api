using System.Data;

namespace LogicMonitor.Api.Test.Settings;

[Collection("CollectorRelated")]
public class CollectorTests2(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAllCollectorGroups()
	{
		var collectorGroups = await LogicMonitorClient
			.GetAllAsync<CollectorGroup>(default)
			.ConfigureAwait(true);
		collectorGroups.Should().NotBeNullOrEmpty();

		// Re-fetch each
		foreach (var collectorGroup in collectorGroups)
		{
			var refetch = await LogicMonitorClient
				.GetAsync<CollectorGroup>(collectorGroup.Id, default)
				.ConfigureAwait(true);
			refetch.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task GetCollectorsFromGroup()
	{
		var collectorGroups = await LogicMonitorClient
			.GetAllAsync<CollectorGroup>(default)
			.ConfigureAwait(true);

		var fullGroup = new CollectorGroup();
		var i = 0;
		while (fullGroup.Id == 0)
		{
			var collectorGroup = collectorGroups[i];
			if (collectorGroup.CollectorCount > 0)
			{
				fullGroup = collectorGroup;
			}

			i++;
		}

		var collectors = await LogicMonitorClient
			.GetAllCollectorsByCollectorGroupIdAsync(fullGroup.Id, default)
			.ConfigureAwait(true);

		collectors.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllCollectors()
	{
		var collectors = await LogicMonitorClient
			.GetAllAsync<Collector>(default)
			.ConfigureAwait(true);
		collectors.Should().NotBeNullOrEmpty();

		// Re-fetch each
		foreach (var collector in collectors)
		{
			var refetch = await LogicMonitorClient
				.GetAsync<Collector>(collector.Id, default)
				.ConfigureAwait(true);
			refetch.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task RunDebugCommand()
	{
		var collectors = await LogicMonitorClient
			.GetAllAsync<Collector>(default)
			.ConfigureAwait(true);
		var testCollector = collectors.Find(c => !c.IsDown);
		testCollector.Should().NotBeNull();
		var response = await LogicMonitorClient
			.ExecuteDebugCommandAndWaitForResultAsync(
				testCollector.Id,
				"!ping 8.8.8.8",
				10_000,
				500,
				default)
			.ConfigureAwait(true);
		response.Should().NotBeNull();
		response ??= new();
		Logger.LogInformation("{Output}", response.Output);
	}

	[Fact]
	public async Task RunGroovyDebugCommand()
	{
		var collectors = await LogicMonitorClient
			.GetAllAsync<Collector>(default)
			.ConfigureAwait(true);
		var testCollector = collectors.Find(c => !c.IsDown);
		testCollector.Should().NotBeNull();
		var response = await LogicMonitorClient
			.ExecuteDebugCommandAndWaitForResultAsync(
				testCollector.Id,
				$"!groovy hostId={WindowsDeviceId}\nprintln \"Hello World\"",
				10_000,
				500,
				default)
			.ConfigureAwait(true);
		response.Should().NotBeNull();
		response ??= new();
		Logger.LogInformation("{Output}", response.Output);
	}

	[Fact]
	public async Task UpdateCollectorGroup()
	{
		var collectorGroup = (await LogicMonitorClient
			.GetAllAsync<CollectorGroup>(default)
			.ConfigureAwait(true))[2];

		var defaultDesc = collectorGroup.Description;
		collectorGroup.Description = "test desc";

		await LogicMonitorClient
			.UpdateCollectorGroupByIdAsync(collectorGroup.Id, collectorGroup, default)
			.ConfigureAwait(true);

		var updatedGroup = (await LogicMonitorClient
			.GetAllAsync<CollectorGroup>(default)
			.ConfigureAwait(true))[1];
		updatedGroup.Should().NotBeNull();

		var updatedDesc = collectorGroup.Description;
		collectorGroup.Description = defaultDesc;

		await LogicMonitorClient
			.UpdateCollectorGroupByIdAsync(collectorGroup.Id, collectorGroup, default)
			.ConfigureAwait(true);

		updatedDesc.Should().Be("test desc");
	}

	[Fact]
	public async Task UpdateCollector()
	{
		var collector = (await LogicMonitorClient
			.GetAllAsync<Collector>(default))
			[0];

		var collectorId = collector.Id;
		var initialDescription = collector.Description;

		var testPropertyName = "test-" + Guid.NewGuid().ToString();
		var testDescription = "desc-" + Guid.NewGuid().ToString();

		collector.Description = testDescription;
		collector.CustomProperties.Add(new EntityProperty { Name = testPropertyName, Value = "test" });

		// Update the collector
		await LogicMonitorClient
			.UpdateCollectorByIdAsync(collectorId, collector, default)
			.ConfigureAwait(true);

		var updatedCollector = await LogicMonitorClient
			.GetAsync<Collector>(collectorId, default);
		updatedCollector.Should().NotBeNull();

		collector.Description.Should().Be(testDescription);
		collector.CustomProperties.Should().Contain(cp => cp.Name == testPropertyName);

		// Return to previous values
		collector.Description = initialDescription;
		collector.CustomProperties.Remove(collector.CustomProperties.Single(cp => cp.Name == testPropertyName));

		await LogicMonitorClient
			.UpdateCollectorByIdAsync(collector.Id, collector, default)
			.ConfigureAwait(true);

		// Check
		collector = (await LogicMonitorClient
			.GetAllAsync<Collector>(default)
			.ConfigureAwait(true))[0];

		collector.Description.Should().Be(initialDescription);
		collector.CustomProperties.Should().NotContain(cp => cp.Name == testPropertyName);
	}

	[Fact]
	public async Task UpdateCollector_WithUpdate()
	{
		var collector = (await LogicMonitorClient
			.GetAllAsync<Collector>(default))
			[0];

		var collectorId = collector.Id;
		var initialDescription = collector.Description;

		var testPropertyName = "test-" + Guid.NewGuid().ToString();
		var testDescription = "desc-" + Guid.NewGuid().ToString();

		collector.Description = testDescription;
		collector.CustomProperties.Add(new EntityProperty { Name = testPropertyName, Value = "test" });

		// Update the collector
		try
		{
			await LogicMonitorClient
				.PutAsync(collector, default)
				.ConfigureAwait(true);

			var updatedCollector = await LogicMonitorClient
				.GetAsync<Collector>(collectorId, default);
			updatedCollector.Should().NotBeNull();

			collector.Description.Should().Be(testDescription);
			collector.CustomProperties.Should().Contain(cp => cp.Name == testPropertyName);
		}
		catch (Exception e)
		{
			Logger.LogError(e, "Error updating collector");
			e.Should().BeNull(because: "We should not be here");
		}

		// Return to previous values
		collector.Description = initialDescription;
		collector.CustomProperties.Remove(collector.CustomProperties.Single(cp => cp.Name == testPropertyName));

		await LogicMonitorClient
			.UpdateCollectorByIdAsync(collector.Id, collector, default)
			.ConfigureAwait(true);

		// Check
		collector = (await LogicMonitorClient
			.GetAllAsync<Collector>(default)
			.ConfigureAwait(true))[0];

		collector.Description.Should().Be(initialDescription);
		collector.CustomProperties.Should().NotContain(cp => cp.Name == testPropertyName);
	}

	[Fact]
	public async Task CreateCollectorDownloadAndDelete()
	{
		// Determine the latest supported version
		var collectorVersionInts = (await LogicMonitorClient.GetAllCollectorVersionsAsync(
			new Filter<CollectorVersion>
			{
				FilterItems =
				[
					new Eq<CollectorVersion>(nameof(CollectorVersion.IsStable), true),
				]
			}, default
			).ConfigureAwait(true))
			.Select(cv => cv.MajorVersion * 1000 + cv.MinorVersion)
			.OrderByDescending(v => v)
			.ToList();

		collectorVersionInts.Should().NotBeNull();
		collectorVersionInts.Should().NotBeNullOrEmpty();

		var collectorVersionInt = collectorVersionInts[0];

		// Create the collector
		var collector = await LogicMonitorClient
			.CreateAsync(new CollectorCreationDto { Description = "UNIT TEST" }, default)
			.ConfigureAwait(true);

		var tempFileInfo = new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString());
		try
		{
			await LogicMonitorClient.DownloadCollectorAsync(
				collector.Id,
				tempFileInfo,
				CollectorPlatformAndArchitecture.Win64,
				CollectorDownloadType.Bootstrap,
				CollectorSize.Medium,
				collectorVersionInt)
				.ConfigureAwait(true);
		}
		finally
		{
			// No need to do this as the collector has not been registered
			// await DefaultPortalClient.DeleteAsync(collector).ConfigureAwait(true);
			tempFileInfo.Delete();

			// Remove the collector from the API
			await LogicMonitorClient
				.DeleteAsync(collector, default)
				.ConfigureAwait(true);
		}
	}
}
