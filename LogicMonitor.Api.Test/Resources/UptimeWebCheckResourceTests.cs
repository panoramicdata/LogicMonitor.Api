namespace LogicMonitor.Api.Test.Resources;

public class UptimeWebCheckResourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TargetDomain = "www.google.com";

	[Fact]
	public Task CrudInternalWebCheckResource()
	{
		const string resourceDisplayName = "IntegrationTest-Uptime-Web-Internal";

		var creationDto = new WebCheckResourceCreationDto
		{
			Name = resourceDisplayName,
			DisplayName = resourceDisplayName,
			Description = "Integration test internal Uptime web check",
			ResourceGroupIds = "1",
			PreferredCollectorId = CollectorId,
			DisableAlerting = true,
			IsInternal = true,
			HostName = TargetDomain,
			Domain = TargetDomain,
			Scheme = UptimeHttpScheme.Https,
			IgnoreSsl = true,
			PollingIntervalMinutes = 5,
			PageLoadAlertTimeMs = 30000,
			SyntheticsCollectorIds = [CollectorId],
			TestLocation = new UptimeTestLocation { All = false, CollectorIds = [CollectorId], SmgIds = [] },
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

		var resource = await LogicMonitorClient
			.CreateAsync(creationDto, CancellationToken);

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
}
