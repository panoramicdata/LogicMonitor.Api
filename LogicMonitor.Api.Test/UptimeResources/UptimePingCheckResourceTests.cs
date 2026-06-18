namespace LogicMonitor.Api.Test.UptimeResources;

public class UptimePingCheckResourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TargetHost = "8.8.8.8";

	[Fact]
	public async Task CrudInternalPingCheckResource()
	{
		const string resourceDisplayName = "IntegrationTest-Uptime-Ping-Internal";

		var collectorId = await GetLiveCollectorIdAsync();

		var creationDto = new PingCheckResourceCreationDto
		{
			Name = resourceDisplayName,
			DisplayName = resourceDisplayName,
			Description = "Integration test internal Uptime ping check",
			ResourceGroupIds = "1",
			PreferredCollectorId = collectorId,
			DisableAlerting = true,
			IsInternal = true,
			HostName = TargetHost,
			PollingIntervalMinutes = 5,
			PacketCount = 5,
			TimeoutMs = 500,
			PercentPacketsNotReceivedInTime = 80,
			SyntheticsCollectorIds = [collectorId],
			TestLocation = new UptimeTestLocation { All = false, CollectorIds = [collectorId], SmgIds = [] },
			Alerting = new UptimeAlertSettings
			{
				OverallAlertLevel = Level.Critical,
				IndividualAlertLevel = Level.Warning,
				IndividualCheckpointAlertsEnabled = true,
				FailedCheckCountBeforeAlerting = 1,
				AlertCondition = SiteMonitorAlertCondition.AllLocations
			}
		};

		await AssertPingCrudAsync(creationDto, resourceDisplayName);
	}

	[Fact]
	public Task CrudExternalPingCheckResource()
	{
		const string resourceDisplayName = "IntegrationTest-Uptime-Ping-External";

		var creationDto = new PingCheckResourceCreationDto
		{
			Name = resourceDisplayName,
			DisplayName = resourceDisplayName,
			Description = "Integration test external Uptime ping check",
			ResourceGroupIds = "1",
			DisableAlerting = true,
			IsInternal = false,
			HostName = TargetHost,
			PollingIntervalMinutes = 5,
			PacketCount = 5,
			TimeoutMs = 500,
			PercentPacketsNotReceivedInTime = 80,
			// External checks run from Site Monitor Group locations (e.g. 2 = US - Washington DC)
			TestLocation = new UptimeTestLocation { All = false, CollectorIds = [], SmgIds = [2] },
			Alerting = new UptimeAlertSettings
			{
				OverallAlertLevel = Level.Critical,
				IndividualAlertLevel = Level.Warning,
				IndividualCheckpointAlertsEnabled = true,
				FailedCheckCountBeforeAlerting = 1,
				AlertCondition = SiteMonitorAlertCondition.AllLocations
			}
		};

		return AssertPingCrudAsync(creationDto, resourceDisplayName);
	}

	/// <summary>
	/// Resolves a usable Collector id from the portal, preferring the configured <see cref="TestWithOutput.CollectorId"/>
	/// when it still exists, otherwise the first non-down Collector. Internal Uptime checks must reference a real Collector.
	/// </summary>
	private async Task<int> GetLiveCollectorIdAsync()
	{
		var collectors = await LogicMonitorClient
			.GetAllAsync<Collector>(CancellationToken);

		var collector = collectors.FirstOrDefault(c => c.Id == CollectorId)
			?? collectors.FirstOrDefault(c => !c.IsDown)
			?? collectors.FirstOrDefault();

		collector.Should().NotBeNull("the portal must have at least one Collector for an internal Uptime check");
		return collector!.Id;
	}

	private async Task AssertPingCrudAsync(PingCheckResourceCreationDto creationDto, string resourceDisplayName)
	{
		// Clean up any leftover test resource from a prior run
		var existingResource = await LogicMonitorClient
			.GetResourceByDisplayNameAsync(resourceDisplayName, CancellationToken);
		if (existingResource is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingResource, cancellationToken: CancellationToken);
		}

		var resource = await LogicMonitorClient
			.CreateAsync(creationDto, CancellationToken);

		resource.Should().NotBeNull();

		try
		{
			resource.DisplayName.Should().Be(resourceDisplayName);
			resource.ResourceType.Should().Be(ResourceType.Ping);

			// Fetch back and verify the typed members round-trip
			var fetched = await LogicMonitorClient
				.GetAsync<PingCheckResource>(resource.Id, CancellationToken);
			fetched.Should().NotBeNull();
			fetched.ResourceType.Should().Be(ResourceType.Ping);
			fetched.Description.Should().Be(creationDto.Description);
			fetched.HostName.Should().Be(creationDto.HostName);
			fetched.PacketCount.Should().Be(creationDto.PacketCount);
			fetched.TimeoutMs.Should().Be(creationDto.TimeoutMs);
			fetched.PercentPacketsNotReceivedInTime.Should().Be(creationDto.PercentPacketsNotReceivedInTime);
			fetched.PollingIntervalMinutes.Should().Be(creationDto.PollingIntervalMinutes);

			// Update (skip if Uptime feature not enabled)
			try
			{
				var updatedDescription = creationDto.Description + " - updated";
				fetched.Description = updatedDescription;
				await LogicMonitorClient
					.PutAsync(fetched, CancellationToken);

				var updated = await LogicMonitorClient
					.GetAsync<PingCheckResource>(resource.Id, CancellationToken);
				updated.Description.Should().Be(updatedDescription);
				updated.ResourceType.Should().Be(ResourceType.Ping);
			}
			catch (LogicMonitorApiException ex) when (ex.Message.Contains("Uptime feature is not enabled"))
			{
				Logger.LogInformation("Skipping update test - Uptime feature is not enabled in this portal");
			}
		}
		finally
		{
			// Delete - ensure cleanup even if test fails
			if (resource?.Id > 0)
			{
				await LogicMonitorClient
					.DeleteAsync(resource, cancellationToken: CancellationToken);
			}
		}
	}
}

