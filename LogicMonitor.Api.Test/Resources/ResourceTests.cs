using System.Globalization;

namespace LogicMonitor.Api.Test.Resources;

public partial class ResourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string CreatedResourceDisplayName = "dns.google.com";
	private const string CustomPropertyName = "blah";
	private const string CustomPropertyValue1 = "test1";
	private const string CustomPropertyValue2 = "test2";

	[Fact]
	[Trait("Category", "Long")]
	public async Task CreateAndRemoveResourceAndResourceGroup()
	{
		var logicMonitorClient = LogicMonitorClient;

		// ResourceGroup properties
		const string deviceGroupName = "UnitTest CreateAndRemoveDeviceAndDeviceGroup";
		const string deviceGroupDescription = "UnitTest CreateAndRemoveDeviceAndDeviceGroup Description";
		const int deviceGroupParentId = 1;

		// Delete device if it already exists
		var deviceForDeletion = await LogicMonitorClient
			.GetResourceByDisplayNameAsync(CreatedResourceDisplayName, CancellationToken);
		if (deviceForDeletion is not null)
		{
			await logicMonitorClient
				.DeleteAsync(deviceForDeletion, cancellationToken: CancellationToken);
		}

		// Delete ResourceGroup if it already exists
		var deviceGroupForDeletion = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(deviceGroupName, CancellationToken);
		if (deviceGroupForDeletion is not null)
		{
			await logicMonitorClient
				.DeleteAsync(deviceGroupForDeletion, cancellationToken: CancellationToken);
		}

		// Get an active collector
		var collectorId = (await logicMonitorClient
			.GetAllAsync<Collector>(CancellationToken)
			)
		.OrderBy(c => c.Id)
		.FirstOrDefault(c => !c.IsDown)?.Id;
		collectorId.Should().NotBeNull();
		collectorId ??= new();

		// Create ResourceGroup
		var deviceGroup = await logicMonitorClient.CreateAsync(new ResourceGroupCreationDto
		{
			Name = deviceGroupName,
			Description = deviceGroupDescription,
			ParentId = deviceGroupParentId.ToString(CultureInfo.InvariantCulture)
		}, CancellationToken);

		foreach (var finalHardDelete in new[] { false, true })
		{
			var deviceFromCreation = await CreateResourceAndAssertAsync(logicMonitorClient, deviceGroup, collectorId.Value);
			var recycleBinItem = await SoftDeleteResourceAndAssertInRecycleBinAsync(logicMonitorClient, deviceFromCreation);
			await FinishResourceDeletionAsync(logicMonitorClient, deviceFromCreation, recycleBinItem, finalHardDelete);
		}

		// Delete ResourceGroup
		await logicMonitorClient
			.DeleteAsync(deviceGroup, cancellationToken: CancellationToken);
	}

	private static async Task<Resource> CreateResourceAndAssertAsync(LogicMonitorClient logicMonitorClient, ResourceGroup deviceGroup, int collectorId)
	{
		// Device properties
		const string resourceName = "127.199.199.199";
		const string deviceDisplayName = CreatedResourceDisplayName;
		const string deviceLink = "https://www.google.co.uk/";
		const string devicePropertyName = "Woo";
		const string devicePropertyValue = "Yay";
		const string deviceDescription = "Description";
		const bool deviceDisableAlerting = true;
		const bool deviceEnableNetflow = false;
		var deviceProperty = new EntityProperty
		{
			Name = devicePropertyName,
			Value = devicePropertyValue
		};

		// Create device
		var deviceCreationDto = new ResourceCreationDto
		{
			Name = resourceName,
			DisplayName = deviceDisplayName,
			Link = deviceLink,
			ResourceGroupIds = $"{deviceGroup.Id}",
			CustomProperties =
				[
					deviceProperty
				],
			Description = deviceDescription,
			DisableAlerting = deviceDisableAlerting,
			EnableNetflow = deviceEnableNetflow,
			PreferredCollectorId = collectorId
		};
		var deviceFromCreation = await logicMonitorClient
			.CreateAsync(
				deviceCreationDto,
				CancellationToken);

		foreach (var device in new[] { deviceFromCreation, await logicMonitorClient.GetAsync<Resource>(deviceFromCreation.Id, CancellationToken) })
		{
			device.Name.Should().Be(resourceName);
			device.DisplayName.Should().Be(deviceDisplayName);
			device.Description.Should().Be(deviceDescription);
			device.Link.Should().Be(deviceLink);
			device.CustomProperties.Should().NotBeNull();
			device.CustomProperties.Count(p => p.Name == devicePropertyName && p.Value == devicePropertyValue).Should().Be(1);
			device.CustomProperties[0].Name.Should().Be(devicePropertyName);
			device.CustomProperties[0].Value.Should().Be(devicePropertyValue);
			device.IsAlertingDisabled.Should().Be(deviceDisableAlerting);
			device.EnableNetflow.Should().Be(deviceEnableNetflow);
		}

		return deviceFromCreation;
	}

	private static async Task<RecycleBinItem> SoftDeleteResourceAndAssertInRecycleBinAsync(LogicMonitorClient logicMonitorClient, Resource deviceFromCreation)
	{
		// Soft delete device
		await logicMonitorClient
			.DeleteAsync(deviceFromCreation, false, cancellationToken: CancellationToken);

		// Get recycle bin items
		var recycleBinItems = await logicMonitorClient
			.GetAllAsync<RecycleBinItem>(CancellationToken);

		// Assert that the device is in the recycle bin
		recycleBinItems.Select(i => i.ResourceId).Should().Contain(deviceFromCreation.Id);

		// Get the recycle bin item
		var recycleBinItem = recycleBinItems.SingleOrDefault(i => i.ResourceId == deviceFromCreation.Id);
		recycleBinItem.Should().NotBeNull();
		recycleBinItem ??= new();
		deviceFromCreation.DisplayName.Should().Be(recycleBinItem.ResourceName);
		recycleBinItem.ResourceType.Should().Be("device");

		return recycleBinItem;
	}

	private static async Task FinishResourceDeletionAsync(LogicMonitorClient logicMonitorClient, Resource deviceFromCreation, RecycleBinItem recycleBinItem, bool finalHardDelete)
	{
		// Hard delete it?
		if (finalHardDelete)
		{
			// Restore from recycle bin
			await logicMonitorClient
				.RecycleBinRestoreAsync([recycleBinItem.Id], CancellationToken);

			// Make sure that it's no longer in the recycle bin
			var recycleBinItems = await logicMonitorClient
				.GetAllAsync<RecycleBinItem>(CancellationToken);
			var remainingItem = recycleBinItems
				.SingleOrDefault(i => i.ResourceId == deviceFromCreation.Id);
			remainingItem.Should().BeNull();

			// Get the resource again by Id
			var restoredResource = await logicMonitorClient
				.GetAsync<Resource>(deviceFromCreation.Id, CancellationToken);

			// Check that it's the same
			restoredResource.Should().NotBeNull();
			restoredResource.DisplayName.Should().Be(deviceFromCreation.DisplayName);

			// Do a regular hard delete
			await logicMonitorClient
				.DeleteAsync(deviceFromCreation, cancellationToken: CancellationToken);
		}
		else
		{
			// Remove from the recycle bin
			await logicMonitorClient
				.RecycleBinDeleteAsync([recycleBinItem.Id], CancellationToken);
		}
	}

	[Fact]
	public async Task SetResourceCustomProperty()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		await SetAndVerifyCustomPropertyValuesAsync(device);
		await AssertInvalidCustomPropertyWritesThrowAsync(device);
		await CreateUpdateAndDeleteCustomPropertyWithoutCheckingAsync(device);
	}

	private async Task SetAndVerifyCustomPropertyValuesAsync(Resource device)
	{
		// Set it to an expected value
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(device.Id, CustomPropertyName, CustomPropertyValue1, CancellationToken);
		var deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		var actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue1);
		actual.Should().Be(1);

		// Set it to a different value
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(device.Id, CustomPropertyName, CustomPropertyValue2, CancellationToken);
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue2);
		actual.Should().Be(1);

		// Set it to null (delete it)
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(device.Id, CustomPropertyName, null, cancellationToken: CancellationToken);
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName);
		actual.Should().Be(0);
	}

	private async Task AssertInvalidCustomPropertyWritesThrowAsync(Resource device)
	{
		// This should fail as there is nothing to delete
		var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient
				.SetResourceCustomPropertyAsync(
					device.Id,
					CustomPropertyName,
					null,
					SetPropertyMode.Delete,
					CancellationToken)
				);
		deletionException.Should().BeOfType<LogicMonitorApiException>();

		var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				CustomPropertyName,
				null,
				SetPropertyMode.Update,
				CancellationToken)
			);
		updateException.Should().BeOfType<InvalidOperationException>();

		var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				CustomPropertyName,
				null,
				SetPropertyMode.Create,
				CancellationToken)
				);
		createNullException.Should().BeOfType<InvalidOperationException>();
	}

	private async Task CreateUpdateAndDeleteCustomPropertyWithoutCheckingAsync(Resource device)
	{
		// Create one without checking
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				CustomPropertyName,
				CustomPropertyValue1,
				SetPropertyMode.Create,
				cancellationToken: CancellationToken);
		var deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		var actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue1);
		actual.Should().Be(1);

		// Update one without checking
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				CustomPropertyName,
				CustomPropertyValue2,
				SetPropertyMode.Update,
				cancellationToken: CancellationToken);
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName && dp.Value == CustomPropertyValue2);
		actual.Should().Be(1);

		// Delete one without checking
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				CustomPropertyName,
				null,
				SetPropertyMode.Delete,
				cancellationToken: CancellationToken);
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == CustomPropertyName);
		actual.Should().Be(0);
	}

	[Fact]
	public async Task GetResourcesByResourceGroupRecursive()
	{
		var devices = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken);

		devices.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllScheduledDownTimes()
	{
		var scheduledDownTimes = await LogicMonitorClient
			.GetAllAsync<ScheduledDownTime>(CancellationToken);

		scheduledDownTimes.Should().NotBeNull();
	}

	[Fact]
	public async Task GetResourcePage()
	{
		const int MaxCount = 50;
		var devicesPage = await LogicMonitorClient
			.GetResourcesPageAsync(new Filter<Resource> { Skip = 0, Take = MaxCount }, CancellationToken);

		devicesPage.Should().NotBeNull();
		(devicesPage.Items.Count <= MaxCount).Should().BeTrue();
	}

	[Fact]
	public async Task GetAllResourceInstances()
	{
		var deviceInstances = await LogicMonitorClient
			.GetResourceInstanceListAsync(WindowsDeviceId, new Filter<ResourceDataSourceInstance>
			{
				Properties =
				[
						nameof(ResourceDataSourceInstance.Id),
						nameof(ResourceDataSourceInstance.Name),
						nameof(ResourceDataSourceInstance.DataSourceName),
				]
			}, CancellationToken);
		(deviceInstances.Items.Count > 10).Should().BeTrue();
	}

	[Fact]
	public async Task GetResourceByDeviceId()
	{
		var device = await LogicMonitorClient
			.GetAsync<Resource>(77, CancellationToken);
		device.Should().NotBeNull();
	}

	/// <summary>
	/// This should fail as the device does not exist
	/// </summary>
	/// <returns></returns>
	[Fact]
	public Task GetResourceByNonExistentDeviceIdShouldFail() =>
		LogicMonitorClient
			.Invoking(async x => await LogicMonitorClient
				.GetAsync<Resource>(12345678, CancellationToken))
			.Should()
			.ThrowAsync<LogicMonitorApiException>();

	[Fact]
	public async Task GetResourceByDisplayNameAsync()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var device2 = await LogicMonitorClient.GetResourceByDisplayNameAsync(device.DisplayName, CancellationToken);
		device2.Should().NotBeNull();
	}

	[Fact]
	public async Task GetResourceByHostName()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var devices = await LogicMonitorClient
			.GetResourcesByHostNameAsync(device.Name, 100, CancellationToken);

		// Possibly all 127.0.0.1
		devices.Should().NotBeEmpty();
	}

	[Fact]
	public async Task DateTimeSetCorrectly()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var refresh = await LogicMonitorClient
			.GetAsync<Resource>(device.Id, CancellationToken);
		refresh.CreatedOnSeconds.Should().BePositive();
		refresh.CreatedOnUtc.Should().NotBeNull();
		refresh.CreatedOnUtc.Should().BeAfter(DateTime.Parse("2011-02-11", CultureInfo.InvariantCulture));
	}

	[Fact]
	public async Task SerialisationIgnoredProperties()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		device.Should().NotBeNull();
		var jObject = JObject.FromObject(device);
		int.Parse(jObject["id"]!.ToString(), CultureInfo.InvariantCulture).Should().Be(device.Id);
		jObject.Properties().Should().AllSatisfy(p => p.Name.Should().NotBe(nameof(Resource.AutoPropertiesAssignedOnUtc)));
	}
}
