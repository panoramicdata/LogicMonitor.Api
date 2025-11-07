using System.Globalization;

namespace LogicMonitor.Api.Test.Resources;

public class ResourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetResourcesByResourceGroupRecursive()
	{
		var devices = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken);

		devices.Should().NotBeNull();
	}

	[Fact]
	public async Task CreateAndRemoveResourceAndResourceGroup()
	{
		var logicMonitorClient = LogicMonitorClient;

		// Device properties
		const string resourceName = "127.199.199.199";
		const string deviceDisplayName = "dns.google.com";
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

		// ResourceGroup properties
		const string deviceGroupName = "UnitTest CreateAndRemoveDeviceAndDeviceGroup";
		const string deviceGroupDescription = "UnitTest CreateAndRemoveDeviceAndDeviceGroup Description";
		const int deviceGroupParentId = 1;

		// Delete device if it already exists
		var deviceForDeletion = await LogicMonitorClient
			.GetResourceByDisplayNameAsync(deviceDisplayName, CancellationToken);
		if (deviceForDeletion is not null)
		{
			await logicMonitorClient
				.DeleteAsync(deviceForDeletion, cancellationToken: CancellationToken)
				;
		}

		// Delete ResourceGroup if it already exists
		var deviceGroupForDeletion = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(deviceGroupName, CancellationToken);
		if (deviceGroupForDeletion is not null)
		{
			await logicMonitorClient
				.DeleteAsync(deviceGroupForDeletion, cancellationToken: CancellationToken)
				;
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
				PreferredCollectorId = collectorId.Value
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

			// Hard delete it?
			if (finalHardDelete)
			{
				// Restore from recycle bin
				await logicMonitorClient
					.RecycleBinRestoreAsync([recycleBinItem.Id], CancellationToken);

				// Make sure that it's no longer in the recycle bin
				recycleBinItems = await logicMonitorClient
					.GetAllAsync<RecycleBinItem>(CancellationToken);
				recycleBinItem = recycleBinItems
					.SingleOrDefault(i => i.ResourceId == deviceFromCreation.Id);
				recycleBinItem.Should().BeNull();

				// Get the resource again by Id
				var restoredResource = await logicMonitorClient
					.GetAsync<Resource>(deviceFromCreation.Id, CancellationToken);

				// Check that it's the same
				restoredResource.Should().NotBeNull();
				restoredResource.DisplayName.Should().Be(deviceFromCreation.DisplayName);

				// Do a regular hard delete
				await logicMonitorClient
					.DeleteAsync(deviceFromCreation, cancellationToken: CancellationToken)
					;
			}
			else
			{
				// Remove from the recycle bin
				await logicMonitorClient
					.RecycleBinDeleteAsync([recycleBinItem.Id], CancellationToken);
			}
		}

		// Delete ResourceGroup
		await logicMonitorClient
			.DeleteAsync(deviceGroup, cancellationToken: CancellationToken)
			;
	}

	[Fact]
	public async Task GetAllScheduledDownTimes()
	{
		var scheduledDownTimes = await LogicMonitorClient
			.GetAllAsync<ScheduledDownTime>(CancellationToken);

		scheduledDownTimes.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDevicePage()
	{
		const int MaxCount = 50;
		var devicesPage = await LogicMonitorClient
			.GetResourcesPageAsync(new Filter<Resource> { Skip = 0, Take = MaxCount }, CancellationToken);

		devicesPage.Should().NotBeNull();
		(devicesPage.Items.Count <= MaxCount).Should().BeTrue();
	}

	[Fact]
	public async Task GetAllDeviceInstances()
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
	public async Task GetDeviceByDeviceId()
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
	public Task GetDeviceByNonExistentDeviceIdShouldFail() =>
		LogicMonitorClient
			.Invoking(async x => await LogicMonitorClient
				.GetAsync<Resource>(12345678, CancellationToken)
				)
			.Should()
			.ThrowAsync<LogicMonitorApiException>();

	[Fact]
	public async Task GetDeviceByDisplayNameAsync()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var device2 = await LogicMonitorClient
			.GetResourceByDisplayNameAsync(device.DisplayName, CancellationToken);
		device2.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceByHostName()
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

	[Fact]
	public async Task GetDevicePropertiesContainsExpected()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		deviceProperties.Should().Contain(dp => dp.Name == "location");
		deviceProperties.Should().Contain(dp => dp.Type == PropertyType.Auto);
	}

	[Fact]
	public async Task GetDevicesAndInstancesAssociatedWithDataSourceById()
	{
		// Get the dataSource
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken);
		dataSource.Should().NotBeNull();

		// Get the information
		var info = await LogicMonitorClient
			.GetResourcesAndInstancesAssociatedWithDataSourceByIdPageAsync(
				dataSource!.Id,
				new Filter<ResourceWithDataSourceInstanceInformation> { Skip = 0, Take = 300 },
				CancellationToken)
			;

		// Check
		info.Should().NotBeNull();
	}

	[Fact]
	public void GetDevicesByDeviceGroupFullPath_InvalidDeviceGroup_ThrowsException()
	{
		LogicMonitorClient
			.Invoking(async x => await x.GetResourcesByResourceGroupFullPathAsync("XXXXXX/YYYYYY", true, CancellationToken))
			.Should()
			.ThrowAsync<LogicMonitorApiException>();
		LogicMonitorClient
			.Invoking(async x => await x.GetResourcesByResourceGroupFullPathAsync("XXXXXX/YYYYYY", false, CancellationToken))
			.Should()
			.ThrowAsync<LogicMonitorApiException>();
	}

	[Fact]
	public async Task GetDevicesByDeviceGroupFullPathV78()
	{
		// Recurse
		var allDatacenterDevices = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken);
		// Don't recurse
		var topFolderDevices = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(DeviceGroupFullPath, false, CancellationToken);

		// Make sure there are more when recursing
		allDatacenterDevices.Should().HaveCountGreaterThan(topFolderDevices.Count);
		allDatacenterDevices.Should().AllSatisfy(d => d.DisplayName.Should().NotBeNull());
	}

	[Fact]
	public async Task GetDevicesRecurse()
	{
		var allStaffDevices = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken);
		allStaffDevices.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetFewerDevices()
	{
		const int numberToFetch = 10;
		var devices = await LogicMonitorClient
			.GetPageAsync(new Filter<Resource> { Skip = 0, Take = numberToFetch }, CancellationToken);
		devices.Items.Should().HaveCount(numberToFetch);
		devices.TotalCount.Should().NotBe(numberToFetch);
		devices.TotalCount.Should().NotBe(0);
	}

	[Fact]
	public async Task GetFullDeviceTree()
	{
		var deviceGroup = await LogicMonitorClient
			.GetFullResourceTreeAsync(cancellationToken: CancellationToken)
			;
		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetFullDeviceTreeForDatacenter()
	{
		var deviceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);
		deviceGroup = await LogicMonitorClient
			.GetFullResourceTreeAsync(deviceGroup.Id, CancellationToken);
		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task TreeNodeFreeSearch()
	{
		// Get the result without specifying a type
		var treeNodeFreeSearch = await LogicMonitorClient
			.TreeNodeFreeSearchAsync("Datacenter", 100, cancellationToken: CancellationToken)
			;
		var totalItemCount = treeNodeFreeSearch.Count;

		// Subtract the count of a search for each individual type
		var treeNodeFreeSearchResultTypes =
		Enum.GetValues<TreeNodeFreeSearchResultType>()
			.Cast<TreeNodeFreeSearchResultType>()
			.Except([TreeNodeFreeSearchResultType.Unknown]);
		var list = new List<TreeNodeFreeSearchResult>();
		foreach (var treeNodeFreeSearchResultType in treeNodeFreeSearchResultTypes)
		{
			var treeNodeFreeSearchResult = await LogicMonitorClient
				.TreeNodeFreeSearchAsync("Datacenter", 100, treeNodeFreeSearchResultType)
				;
			list.AddRange(treeNodeFreeSearchResult);
		}
		// Make sure that some are returned
		list.Should().HaveCount(totalItemCount);
	}

	[Fact]
	public async Task UpdateDeviceProperty()
	{
		var portalClient = LogicMonitorClient;
		var device = await GetWindowsResourceAsync(CancellationToken);

		// Remove any called "test"
		const string testPropertyName = "test";
		const string testPropertyValue = "testValue";
		var errantProperty = device.CustomProperties.SingleOrDefault(p => p.Name == testPropertyName);
		if (errantProperty is not null)
		{
			device.CustomProperties.Remove(errantProperty);
		}

		device.CustomProperties.Add(new EntityProperty
		{
			Name = testPropertyName,
			Value = testPropertyValue
		});

		// Update the device
		await portalClient
			.PutAsync(device, CancellationToken);

		// Re-fetch the device
		device = await GetWindowsResourceAsync(CancellationToken);

		// Make sure that there is now one called "test"
		var testProperty = device
			.CustomProperties
			.SingleOrDefault(p => p.Name == testPropertyName);
		testProperty.Should().NotBeNull();
		testProperty ??= new();
		testProperty.Value.Should().Be(testPropertyValue);

		// Clean up afterwards - remove the property
		// Re-fetch the properties
		device.CustomProperties = [.. device.CustomProperties.Where(p => p.Name != testPropertyName)];

		// Update the device
		await portalClient.PutAsync(device, CancellationToken);

		// Re-fetch the properties
		device = await GetWindowsResourceAsync(CancellationToken);

		// Make sure that there are none called "test"
		device.CustomProperties.Should().AllSatisfy(p => p.Name.Should().NotBe(testPropertyName));
	}

	[Fact]
	public async Task SetDeviceCustomProperty()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		const string propertyName = "blah";
		const string value1 = "test1";
		const string value2 = "test2";

		// Set it to an expected value
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(device.Id, propertyName, value1, CancellationToken);
		var deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Set it to a different value
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(device.Id, propertyName, value2, CancellationToken);
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Set it to null (delete it)
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(device.Id, propertyName, null, cancellationToken: CancellationToken)
			;
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);

		// This should fail as there is nothing to delete
		var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient
				.SetResourceCustomPropertyAsync(
					device.Id,
					propertyName,
					null,
					SetPropertyMode.Delete,
					CancellationToken)
				)
			;
		deletionException.Should().BeOfType<LogicMonitorApiException>();

		var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				propertyName,
				null,
				SetPropertyMode.Update,
				CancellationToken)
			);
		updateException.Should().BeOfType<InvalidOperationException>();

		var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				propertyName,
				null,
				SetPropertyMode.Create,
				CancellationToken)
				)
			;
		createNullException.Should().BeOfType<InvalidOperationException>();

		// Create one without checking
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				propertyName,
				value1,
				SetPropertyMode.Create,
				cancellationToken: CancellationToken)
			;
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Update one without checking
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				propertyName,
				value2,
				SetPropertyMode.Update,
				cancellationToken: CancellationToken)
			;
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Delete one without checking
		await LogicMonitorClient
			.SetResourceCustomPropertyAsync(
				device.Id,
				propertyName,
				null,
				SetPropertyMode.Delete,
				cancellationToken: CancellationToken)
			;
		deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);
	}

	[Fact]
	public async Task GetDeviceCustomProperties()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var deviceProperties = await LogicMonitorClient.GetResourcePropertiesAsync(device.Id, CancellationToken);
		deviceProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceUsingSubUrl()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var deviceRefetch = await LogicMonitorClient
			.GetAllAsync<JObject>($"device/devices?filter=id:{device.Id}&fields=inheritedProperties", CancellationToken);
		deviceRefetch.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDifferentDeviceTypes()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		device.ResourceType.Should().Be(ResourceType.Regular);

		device = await GetServiceDeviceAsync(CancellationToken);
		device.ResourceType.Should().Be(ResourceType.Service);

		// TODO - AWS and Azure
	}

	[Fact]
	public async Task GetDeadDevices()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(new Filter<Resource>
		{
			FilterItems =
			[
				new Eq<Resource>(nameof(Resource.ResourceStatus), "dead")
			]
		}, CancellationToken);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAlertDisableDevices_Filtered()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(new Filter<Resource>
		{
			FilterItems =
			[
				new Ne<Resource>(nameof(Resource.AlertDisableStatus), "none-none-none")
			]
		}, CancellationToken);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAlertDisableDevices_RawFilter()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(
			new Filter<Resource> { QueryString = "filter=alertDisableStatus!:\"none-none-none\"" },
			CancellationToken);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDevicesFromRoot()
	{
		// Fetch in ascending order to get unchanging DeviceGroup first
		var deviceGroups = await LogicMonitorClient
			.GetAllAsync(new Filter<ResourceGroup>
			{
				Order = new Order<ResourceGroup> { Property = nameof(ResourceGroup.Id), Direction = OrderDirection.Asc }
			}, CancellationToken);
		var deviceGroup = deviceGroups.Find(dg => dg.SubGroups.Count != 0);
		deviceGroup.Should().NotBeNull();
		deviceGroup ??= new();
		var deviceList = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(deviceGroup.FullPath, true, CancellationToken);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceAlertsPageAsync()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var alertsPage = await LogicMonitorClient
			.GetResourceAlertsPageAsync(device.Id, 0, 100, CancellationToken);
		alertsPage.Should().NotBeNull();
		alertsPage.Items.Should().HaveCount(alertsPage.TotalCount);
	}

	[Fact]
	public async Task GetDeviceAlertsWithFilterAsync()
	{
		var alertFilter = new AlertFilter
		{
			IncludeCleared = true,
			Levels =
				 [
					 AlertLevel.Warning,
					 AlertLevel.Error,
					 AlertLevel.Critical
				 ],
			StartUtcIsAfter = DateTime.UtcNow.AddDays(-25)
		};

		// Get the Device
		var device = await GetWindowsResourceAsync(CancellationToken);
		device.Should().NotBeNull();

		// Get the Alerts
		var allAlerts = await LogicMonitorClient
			.GetResourceAlertsByIdAsync(device.Id, alertFilter, CancellationToken);
		allAlerts.Should().NotBeNull();
	}

	[Fact]
	public async Task PatchDeviceAsync()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var oldDescription = device.Description;
		var newDescription = Guid.NewGuid().ToString();
		await LogicMonitorClient
			.PatchAsync(device, new Dictionary<string, object> { { "description", newDescription } }, CancellationToken);
		var updatedDevice = await GetWindowsResourceAsync(CancellationToken);
		updatedDevice.Description.Should().Be(newDescription);
		await LogicMonitorClient
			.PatchAsync(device, new Dictionary<string, object> { { "description", oldDescription } }, CancellationToken);
	}

	[Fact]
	public Task ScheduleActiveDiscovery()
		=> LogicMonitorClient
			.ScheduleActiveDiscovery(WindowsDeviceId, CancellationToken);

	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetDeviceAlertSettings()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);

		var config = await LogicMonitorClient
			.GetResourceDataPointConfigurationsAsync(device.Id, CancellationToken);

		config.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetDeviceDataSourceInstanceDataPointConfigurationAsync()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);

		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SNMP_Network_Interfaces", CancellationToken);
		dataSource.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken);

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, CancellationToken);

		var deviceDataSourceInstance = deviceDataSourceInstances.FirstOrDefault();
		deviceDataSourceInstance.Should().NotBeNull();

		var config = await LogicMonitorClient
			.GetResourceDataSourceInstanceDataPointConfigurationsAsync(
				device.Id,
				deviceDataSource.Id,
				deviceDataSourceInstance.Id,
				CancellationToken);

		config.Should().NotBeNull();
	}

	[Fact]
	public async Task GetTopTalkerGraphAsync()
	{
		var devicesPage = await LogicMonitorClient
			.GetResourcesPageAsync(new Filter<Resource> { Skip = 0, Take = 50 }, CancellationToken);

		var netflowEnabledId = 0;

		foreach (var device in devicesPage.Items)
		{
			if (device.EnableNetflow)
			{
				netflowEnabledId = device.Id;
			}
		}

		var graph = await LogicMonitorClient
			.GetTopTalkersGraphAsync(netflowEnabledId, CancellationToken);
		graph.Should().NotBeNull();
	}
}
