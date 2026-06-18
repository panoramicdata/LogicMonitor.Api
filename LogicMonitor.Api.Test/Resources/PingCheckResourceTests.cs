using System.Text.Json;

namespace LogicMonitor.Api.Test.Resources;

public class PingCheckResourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task CrudPingCheckResource()
	{
		const string resourceDisplayName = "IntegrationTest-Ping-Check";
		const string targetHost = "8.8.8.8";
		const string initialDescription = "Integration test Ping check resource";
		const string updatedDescription = "Integration test Ping check resource - updated";

		// Clean up any leftover test resource from a prior run
		var existingResource = await LogicMonitorClient
			.GetResourceByDisplayNameAsync(resourceDisplayName, CancellationToken);
		if (existingResource is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingResource, cancellationToken: CancellationToken);
		}

		// Build the service parameters blob that LM uses to configure the ping check synthetics.
		// Per the "Adding Uptime Devices" docs, an internal check must set all=false when specific
		// collectorIds are supplied (and smgIds must be empty).
		var testLocationParams = new TestLocationParameters
		{
			All = false,
			CollectorIds = [CollectorId],
			SmgIds = []
		};
		var testLocationJson = JsonSerializer.Serialize(testLocationParams);

		var serviceParameters = new PingCheckServiceParameters
		{
			PercentPktsNotReceiveInTime = "80",
			TestLocation = testLocationJson,
			OverallAlertLevel = "critical",
			PollingInterval = "5",
			Dns = targetHost,
			Count = "5",
			IndividualSmAlertEnable = "true",
			TimeoutInMSPktsNotReceive = "500",
			Transition = "1",
			GlobalSmAlertCond = "0",
			IsInternal = "true",
			IndividualAlertLevel = "warn"
		};
		var serviceParametersJson = JsonSerializer.Serialize(serviceParameters);

		// Create
		var creationDto = new ResourceCreationDto
		{
			Name = resourceDisplayName,
			DisplayName = resourceDisplayName,
			Description = initialDescription,
			ResourceGroupIds = "1",
			PreferredCollectorId = CollectorId,
			DisableAlerting = true,
			ResourceType = ResourceType.Ping,
			TestLocation = new WebsiteLocation
			{
				All = false,
				CollectorIds = [CollectorId],
				SmgIds = []
			},
			SyntheticsCollectorIds = [CollectorId],
			CustomProperties =
			[
				new EntityProperty { Name = "system.categories", Value = "pingcheckdevice" },
				new EntityProperty { Name = "uptime.hostname", Value = targetHost },
				new EntityProperty { Name = "uptime.pollingInterval", Value = "5" },
				new EntityProperty { Name = "uptime.usedefaultalertsetting", Value = "false" },
				new EntityProperty { Name = "uptime.usedefaultlocationsetting", Value = "false" },
				new EntityProperty { Name = "website.private.serviceParameters", Value = serviceParametersJson }
			]
		};

		var resource = await LogicMonitorClient
			.CreateAsync(creationDto, CancellationToken);

		resource.Should().NotBeNull();

		var resourceRefetched = await LogicMonitorClient
			.GetAsync<Resource>(resource.Id, CancellationToken);

		// We should be able to retrieve the resource and it should have the same properties as when we created it, using the strongly-typed PingCheckResource class.
		var pingCheckResource = await LogicMonitorClient
			.GetAsync<PingCheckResource>(resource.Id, CancellationToken);

		try
		{
			resource.DisplayName.Should().Be(resourceDisplayName);
			resource.ResourceType.Should().Be(ResourceType.Ping);

			// Fetch back and verify
			var fetched = await LogicMonitorClient
				.GetAsync<Resource>(resource.Id, CancellationToken);
			fetched.Should().NotBeNull();
			fetched.ResourceType.Should().Be(ResourceType.Ping);
			fetched.Description.Should().Be(initialDescription);

			// Update description (skip if Uptime feature not enabled)
			try
			{
				fetched.Description = updatedDescription;
				await LogicMonitorClient
					.PutAsync(fetched, CancellationToken);

				// Verify update
				var updated = await LogicMonitorClient
					.GetAsync<Resource>(resource.Id, CancellationToken);
				updated.Description.Should().Be(updatedDescription);
				updated.ResourceType.Should().Be(ResourceType.Ping);
				((int)updated.ResourceType).Should().Be(19);
			}
			catch (LogicMonitorApiException ex) when (ex.Message.Contains("Uptime feature is not enabled"))
			{
				// Skip update test if Uptime feature is not enabled in this portal
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
