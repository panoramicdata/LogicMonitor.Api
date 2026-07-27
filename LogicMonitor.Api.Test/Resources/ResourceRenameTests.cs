namespace LogicMonitor.Api.Test.Resources;

/// <summary>
/// Integration tests (live portal) proving:
///  1. A <see cref="ResourceGroup" /> can be renamed via a full-object <see cref="LogicMonitorClient.PutAsync{T}" />.
///  2. An Uptime ping-check <see cref="Resource" /> can be renamed via PutAsync - and that BOTH Name and
///     DisplayName must be set (a DisplayName-only change silently no-ops on uptimepingcheck devices), while
///     the ping target (Host) is preserved.
///  3. A hidden/secret custom property (snmp.community) can be written safely via
///     <see cref="LogicMonitorClient.SetCustomPropertyAsync(EntityPropertyWrite, SetPropertyMode, CancellationToken)" />
///     (one field at a time) - the admin-gated, clobber-free alternative to round-tripping a whole object
///     whose secret fields come back masked as ********.
/// </summary>
public class ResourceRenameTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string TargetHost = "8.8.8.8";

	[Fact]
	public async Task PutAsync_RenamesResourceGroup()
	{
		const string originalName = "IntegrationTest-Rename-Group-Before";
		const string renamedName = "IntegrationTest-Rename-Group-After";

		await DeleteResourceGroupIfExistsAsync(originalName);
		await DeleteResourceGroupIfExistsAsync(renamedName);

		var group = await LogicMonitorClient.CreateAsync(
			new ResourceGroupCreationDto { ParentId = "1", Name = originalName },
			CancellationToken);

		try
		{
			// Fetch the full object, change the Name, and PUT it back.
			var fetched = await LogicMonitorClient.GetAsync<ResourceGroup>(group.Id, CancellationToken);
			fetched.Name = renamedName;
			await LogicMonitorClient.PutAsync(fetched, CancellationToken);

			var reloaded = await LogicMonitorClient.GetAsync<ResourceGroup>(group.Id, CancellationToken);
			reloaded.Name.Should().Be(renamedName);
		}
		finally
		{
			await LogicMonitorClient.DeleteAsync(group, true, CancellationToken);
		}
	}

	[Fact]
	public async Task PutAsync_RenamesUptimePingCheckResource_SettingNameAndDisplayName()
	{
		const string originalName = "IntegrationTest-Rename-Ping-Before";
		const string renamedName = "IntegrationTest-Rename-Ping-After";

		await DeleteResourceIfExistsAsync(originalName);
		await DeleteResourceIfExistsAsync(renamedName);

		var collectorId = await GetLiveCollectorIdAsync();

		PingCheckResource resource;
		try
		{
			resource = await LogicMonitorClient.CreateAsync(
				new PingCheckResourceCreationDto
				{
					Name = originalName,
					DisplayName = originalName,
					Description = "Integration test rename ping check",
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
					TestLocation = new UptimeTestLocation { All = true, CollectorIds = [collectorId], SmgIds = [] },
					Alerting = new UptimeAlertSettings
					{
						OverallAlertLevel = Level.Critical,
						IndividualAlertLevel = Level.Warning,
						IndividualCheckpointAlertsEnabled = true,
						FailedCheckCountBeforeAlerting = 1,
						AlertCondition = SiteMonitorAlertCondition.AllLocations
					}
				},
				CancellationToken);
		}
		catch (LogicMonitorApiException ex) when (IsUptimeUnavailable(ex))
		{
			Assert.Skip($"Portal does not have LM Uptime enabled: {ex.Message}");
			return;
		}

		try
		{
			var fetched = await LogicMonitorClient.GetAsync<PingCheckResource>(resource.Id, CancellationToken);
			var originalHost = fetched.HostName;

			// A rename on a uptimepingcheck device must set BOTH Name and DisplayName; a DisplayName-only
			// change silently no-ops on these devices. For these devices Name is a label, not the ping
			// target (that is HostName), so changing Name is safe.
			fetched.Name = renamedName;
			fetched.DisplayName = renamedName;
			await LogicMonitorClient.PutAsync(fetched, CancellationToken);

			var reloaded = await LogicMonitorClient.GetAsync<PingCheckResource>(resource.Id, CancellationToken);
			reloaded.Name.Should().Be(renamedName);
			reloaded.DisplayName.Should().Be(renamedName);
			reloaded.HostName.Should().Be(originalHost, "the ping target must be preserved across a rename");
			reloaded.ResourceType.Should().Be(ResourceType.Ping);
		}
		finally
		{
			if (resource.Id > 0)
			{
				await LogicMonitorClient.DeleteAsync(resource, cancellationToken: CancellationToken);
			}
		}
	}

	[Fact]
	public async Task SetCustomPropertyAsync_WritesHiddenField_ViaEntityPropertyWrite()
	{
		const string groupName = "IntegrationTest-HiddenField-Group";
		const string secretPropertyName = "snmp.community";

		await DeleteResourceGroupIfExistsAsync(groupName);

		var group = await LogicMonitorClient.CreateAsync(
			new ResourceGroupCreationDto { ParentId = "1", Name = groupName },
			CancellationToken);

		try
		{
			// Write the hidden field one property at a time - never a full-object PUT - so the masked
			// ******** value is never sent and the real value cannot be clobbered. This is the admin-gated
			// config-as-code shape: [ { "type": "resourceGroup", "id": <id>, "name": "snmp.community", "value": "public" } ]
			var write = new EntityPropertyWrite
			{
				Type = EntityPropertyWriteTargetType.ResourceGroup,
				Id = group.Id,
				Name = secretPropertyName,
				Value = "public"
			};
			await LogicMonitorClient.SetCustomPropertyAsync(write, SetPropertyMode.Create, CancellationToken);

			var properties = await LogicMonitorClient.GetResourceGroupPropertiesAsync(group.Id, CancellationToken);
			properties.Should().ContainSingle(p => p.Name == secretPropertyName,
				"the hidden field must be written (its value is masked as ******** by LogicMonitor on read)");

			// The batch entry point applies the same write as a config list.
			await LogicMonitorClient.SetCustomPropertiesAsync([write], SetPropertyMode.Update, CancellationToken);

			properties = await LogicMonitorClient.GetResourceGroupPropertiesAsync(group.Id, CancellationToken);
			properties.Should().ContainSingle(p => p.Name == secretPropertyName);
		}
		finally
		{
			await LogicMonitorClient.DeleteAsync(group, true, CancellationToken);
		}
	}

	/// <summary>
	/// True when a creation error indicates the portal cannot host LM Uptime checks (feature disabled or the
	/// Ping_Check LogicModules are not imported) - in which case the test should be skipped, not failed.
	/// </summary>
	private static bool IsUptimeUnavailable(LogicMonitorApiException ex)
		=> ex.Message.Contains("Uptime feature is not enabled", StringComparison.OrdinalIgnoreCase)
			|| ex.Message.Contains("datasources not found", StringComparison.OrdinalIgnoreCase);

	private async Task<int> GetLiveCollectorIdAsync()
	{
		var collectors = await LogicMonitorClient.GetAllAsync<Collector>(CancellationToken);
		var collector = collectors.FirstOrDefault(c => c.Id == CollectorId)
			?? collectors.FirstOrDefault(c => !c.IsDown)
			?? collectors.FirstOrDefault();
		collector.Should().NotBeNull("the portal must have at least one Collector for an internal Uptime check");
		return collector!.Id;
	}

	private async Task DeleteResourceGroupIfExistsAsync(string fullPath)
	{
		var existing = await LogicMonitorClient.GetResourceGroupByFullPathAsync(fullPath, CancellationToken);
		if (existing is not null)
		{
			await LogicMonitorClient.DeleteAsync(existing, true, CancellationToken);
		}
	}

	private async Task DeleteResourceIfExistsAsync(string displayName)
	{
		var existing = await LogicMonitorClient.GetResourceByDisplayNameAsync(displayName, CancellationToken);
		if (existing is not null)
		{
			await LogicMonitorClient.DeleteAsync(existing, cancellationToken: CancellationToken);
		}
	}
}
