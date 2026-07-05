namespace LogicMonitor.Api.Test.UptimeResources;

public class UptimeWebCheckResourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TargetDomain = "www.google.com";

	[Fact]
	public async Task CrudInternalWebCheckResource()
	{
		const string resourceDisplayName = "IntegrationTest-Uptime-Web-Internal";

		var collectorId = await GetLiveCollectorIdAsync();

		var creationDto = new WebCheckResourceCreationDto
		{
			Name = resourceDisplayName,
			DisplayName = resourceDisplayName,
			Description = "Integration test internal Uptime web check",
			ResourceGroupIds = "1",
			PreferredCollectorId = collectorId,
			DisableAlerting = true,
			IsInternal = true,
			HostName = TargetDomain,
			Domain = TargetDomain,
			Scheme = UptimeHttpScheme.Https,
			IgnoreSsl = true,
			PollingIntervalMinutes = 5,
			PageLoadAlertTimeMs = 30000,
			SyntheticsCollectorIds = [collectorId],
			TestLocation = new UptimeTestLocation { All = false, CollectorIds = [collectorId], SmgIds = [] },
			Alerting = new UptimeAlertSettings
			{
				OverallAlertLevel = Level.Warning,
				IndividualAlertLevel = Level.Warning,
				IndividualCheckpointAlertsEnabled = true,
				FailedCheckCountBeforeAlerting = 1,
				AlertCondition = SiteMonitorAlertCondition.AllLocations
			},
			Steps = [new UptimeWebCheckStep { Name = "__step0", Url = TargetDomain, HttpMethod = "GET", StatusCode = "200" }]
		};

		await AssertWebCrudAsync(creationDto, resourceDisplayName);
	}

	[Fact]
	public Task CrudExternalWebCheckResource()
	{
		const string resourceDisplayName = "IntegrationTest-Uptime-Web-External";

		var creationDto = new WebCheckResourceCreationDto
		{
			Name = resourceDisplayName,
			DisplayName = resourceDisplayName,
			Description = "Integration test external Uptime web check",
			ResourceGroupIds = "1",
			DisableAlerting = true,
			IsInternal = false,
			HostName = TargetDomain,
			Domain = TargetDomain,
			Scheme = UptimeHttpScheme.Https,
			IgnoreSsl = true,
			PollingIntervalMinutes = 5,
			PageLoadAlertTimeMs = 30000,
			// External checks run from Site Monitor Group locations (e.g. 2 = US - Washington DC)
			TestLocation = new UptimeTestLocation { All = false, CollectorIds = [], SmgIds = [2] },
			Alerting = new UptimeAlertSettings
			{
				OverallAlertLevel = Level.Warning,
				IndividualAlertLevel = Level.Warning,
				IndividualCheckpointAlertsEnabled = true,
				FailedCheckCountBeforeAlerting = 1,
				AlertCondition = SiteMonitorAlertCondition.AllLocations
			},
			Steps = [new UptimeWebCheckStep { Name = "__step0", Url = TargetDomain, HttpMethod = "GET", StatusCode = "200" }]
		};

		return AssertWebCrudAsync(creationDto, resourceDisplayName);
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

	private async Task AssertWebCrudAsync(WebCheckResourceCreationDto creationDto, string resourceDisplayName)
	{
		// Clean up any leftover test resource from a prior run
		var existingResource = await LogicMonitorClient
			.GetResourceByDisplayNameAsync(resourceDisplayName, CancellationToken);
		if (existingResource is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingResource, cancellationToken: CancellationToken);
		}

		// LM Uptime web checks require the feature and the Web_Check LogicModules; skip (not fail) where absent.
		WebCheckResource resource;
		try
		{
			resource = await LogicMonitorClient.CreateAsync(creationDto, CancellationToken);
		}
		catch (LogicMonitorApiException ex) when (UptimePingCheckResourceTests.IsUptimeUnavailable(ex))
		{
			Assert.Skip($"Portal does not have LM Uptime web checks enabled: {ex.Message}");
			return;
		}

		resource.Should().NotBeNull();

		try
		{
			resource.DisplayName.Should().Be(resourceDisplayName);
			resource.ResourceType.Should().Be(ResourceType.Web);

			// Fetch back and verify the typed members round-trip
			var fetched = await LogicMonitorClient
				.GetAsync<WebCheckResource>(resource.Id, CancellationToken);
			fetched.Should().NotBeNull();
			fetched.ResourceType.Should().Be(ResourceType.Web);
			fetched.Description.Should().Be(creationDto.Description);
			fetched.Domain.Should().Be(creationDto.Domain);
			fetched.Scheme.Should().Be(creationDto.Scheme);

			// Internal web checks must be provisioned as an Uptime resource: the server applies the Web_Check
			// DataSources and starts collecting, driven by the v3 structured creation body.
			if (creationDto.IsInternal)
			{
				await AssertWebDataSourcesAppliedAsync(resource.Id);
			}

			// Update (skip if Uptime feature not enabled)
			try
			{
				var updatedDescription = creationDto.Description + " - updated";
				fetched.Description = updatedDescription;
				await LogicMonitorClient
					.PutAsync(fetched, CancellationToken);

				var updated = await LogicMonitorClient
					.GetAsync<WebCheckResource>(resource.Id, CancellationToken);
				updated.Description.Should().Be(updatedDescription);
				updated.ResourceType.Should().Be(ResourceType.Web);
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

	private async Task AssertWebDataSourcesAppliedAsync(int resourceId)
	{
		List<ResourceDataSource> appliedDataSources = [];

		for (var attempt = 0; attempt < 40; attempt++)
		{
			appliedDataSources = await LogicMonitorClient
				.GetAllResourceDataSourcesAsync(resourceId, null, CancellationToken);

			if (appliedDataSources.Any(ds => ds.DataSourceName == "Web_Check_Individual")
				&& appliedDataSources.Any(ds => ds.DataSourceName == "Web_Check_Overall"))
			{
				break;
			}

			await Task.Delay(3000, CancellationToken);
		}

		appliedDataSources
			.Should().Contain(ds => ds.DataSourceName == "Web_Check_Individual",
				"Web_Check_Individual must be applied automatically when creating an internal web check");
		appliedDataSources
			.Should().Contain(ds => ds.DataSourceName == "Web_Check_Overall",
				"Web_Check_Overall must be applied automatically when creating an internal web check");
	}
}

