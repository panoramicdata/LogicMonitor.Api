using System.Globalization;

namespace LogicMonitor.Api.Test.Devices;

public class DeviceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetDevicesByDeviceGroupRecursive()
	{
		var devices = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true, default)
			.ConfigureAwait(true);

		devices.Should().NotBeNull();
	}

	[Fact]
	public async Task CreateAndRemoveDeviceAndDeviceGroup()
	{
		var logicMonitorClient = LogicMonitorClient;

		// Device properties
		const string deviceName = "127.199.199.199";
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

		// Device group properties
		const string deviceGroupName = "UnitTest CreateAndRemoveDeviceAndDeviceGroup";
		const string deviceGroupDescription = "UnitTest CreateAndRemoveDeviceAndDeviceGroup Description";
		const int deviceGroupParentId = 1;

		// Delete device if it already exists
		var deviceForDeletion = await LogicMonitorClient
			.GetDeviceByDisplayNameAsync(deviceDisplayName, default)
			.ConfigureAwait(true);
		if (deviceForDeletion is not null)
		{
			await logicMonitorClient
				.DeleteAsync(deviceForDeletion, cancellationToken: default)
				.ConfigureAwait(true);
		}

		// Delete device group if it already exists
		var deviceGroupForDeletion = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(deviceGroupName, default)
			.ConfigureAwait(true);
		if (deviceGroupForDeletion is not null)
		{
			await logicMonitorClient
				.DeleteAsync(deviceGroupForDeletion, cancellationToken: default)
				.ConfigureAwait(true);
		}

		// Get an active collector
		var collectorId = (await logicMonitorClient
			.GetAllAsync<Collector>(default)
			.ConfigureAwait(true))
		.OrderBy(c => c.Id)
		.FirstOrDefault(c => !c.IsDown)?.Id;
		collectorId.Should().NotBeNull();
		collectorId ??= new();

		// Create device group
		var deviceGroup = await logicMonitorClient.CreateAsync(new DeviceGroupCreationDto
		{
			Name = deviceGroupName,
			Description = deviceGroupDescription,
			ParentId = deviceGroupParentId.ToString(CultureInfo.InvariantCulture)
		}, default).ConfigureAwait(true);

		foreach (var finalHardDelete in new[] { false, true })
		{
			// Create device
			var deviceCreationDto = new DeviceCreationDto
			{
				Name = deviceName,
				DisplayName = deviceDisplayName,
				Link = deviceLink,
				DeviceGroupIds = $"{deviceGroup.Id}",
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
					default)
				.ConfigureAwait(true);

			foreach (var device in new[] { deviceFromCreation, await logicMonitorClient.GetAsync<Device>(deviceFromCreation.Id, default).ConfigureAwait(true) })
			{
				device.Name.Should().Be(deviceName);
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
				.DeleteAsync(deviceFromCreation, false, cancellationToken: default)
				.ConfigureAwait(true);

			// Get recycle bin items
			var recycleBinItems = await logicMonitorClient
				.GetAllAsync<RecycleBinItem>(default)
				.ConfigureAwait(true);

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
					.RecycleBinRestoreAsync([recycleBinItem.Id], default)
					.ConfigureAwait(true);

				// Get the recycle bin item and make sure it was restored correctly
				recycleBinItems = await logicMonitorClient
					.GetAllAsync<RecycleBinItem>(default)
					.ConfigureAwait(true);
				recycleBinItem = recycleBinItems
					.SingleOrDefault(i => i.ResourceId == deviceFromCreation.Id);
				recycleBinItem.Should().BeNull();

				// Do a regular hard delete
				await logicMonitorClient
					.DeleteAsync(deviceFromCreation, cancellationToken: default)
					.ConfigureAwait(true);
			}
			else
			{
				// Remove from the recycle bin
				await logicMonitorClient
					.RecycleBinDeleteAsync([recycleBinItem.Id], default)
					.ConfigureAwait(true);
			}
		}

		// Delete device group
		await logicMonitorClient
			.DeleteAsync(deviceGroup, cancellationToken: default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task GetAllScheduledDownTimes()
	{
		var scheduledDownTimes = await LogicMonitorClient
			.GetAllAsync<ScheduledDownTime>(default)
			.ConfigureAwait(true);

		scheduledDownTimes.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDevicePage()
	{
		const int MaxCount = 50;
		var devicesPage = await LogicMonitorClient
			.GetDevicesPageAsync(new Filter<Device> { Skip = 0, Take = MaxCount }, default)
			.ConfigureAwait(true);

		devicesPage.Should().NotBeNull();
		(devicesPage.Items.Count <= MaxCount).Should().BeTrue();
	}

	[Fact]
	public async Task GetAllDeviceInstances()
	{
		var deviceInstances = await LogicMonitorClient
			.GetDeviceInstanceListAsync(WindowsDeviceId, new Filter<DeviceDataSourceInstance>
			{
				Properties =
				[
						nameof(DeviceDataSourceInstance.Id),
						nameof(DeviceDataSourceInstance.Name),
						nameof(DeviceDataSourceInstance.DataSourceName),
				]
			}, default)
			.ConfigureAwait(true);
		(deviceInstances.Items.Count > 10).Should().BeTrue();
	}

	[Fact]
	public async Task GetDeviceByDeviceId()
	{
		var device = await LogicMonitorClient
			.GetAsync<Device>(77, default)
			.ConfigureAwait(true);
		device.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceByNonExistentDeviceIdShouldFail()
	{
		// This should fail as the Device does not exist
		LogicMonitorClient
			.Invoking(async x => await LogicMonitorClient.GetAsync<Device>(12345678, default).ConfigureAwait(true))
			.Should()
			.ThrowAsync<LogicMonitorApiException>();
	}

	[Fact]
	public async Task GetDeviceByDisplayNameAsync()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var device2 = await LogicMonitorClient
			.GetDeviceByDisplayNameAsync(device.DisplayName, default)
			.ConfigureAwait(true);
		device2.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceByHostName()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var devices = await LogicMonitorClient
			.GetDevicesByHostNameAsync(device.Name, 100, default)
			.ConfigureAwait(true);
		devices.Should().ContainSingle();
	}

	[Fact]
	public async Task DateTimeSetCorrectly()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var refresh = await LogicMonitorClient
			.GetAsync<Device>(device.Id, default)
			.ConfigureAwait(true);
		refresh.CreatedOnSeconds.Should().BePositive();
		refresh.CreatedOnUtc.Should().NotBeNull();
		refresh.CreatedOnUtc.Should().BeAfter(DateTime.Parse("2011-02-11", CultureInfo.InvariantCulture));
	}

	[Fact]
	public async Task SerialisationIgnoredProperties()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		device.Should().NotBeNull();
		var jObject = JObject.FromObject(device);
		int.Parse(jObject["id"]!.ToString(), CultureInfo.InvariantCulture).Should().Be(device.Id);
		jObject.Properties().Should().AllSatisfy(p => p.Name.Should().NotBe(nameof(Device.AutoPropertiesAssignedOnUtc)));
	}

	[Fact]
	public async Task GetDevicePropertiesContainsExpected()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var deviceProperties = await LogicMonitorClient
			.GetDevicePropertiesAsync(device.Id, default)
			.ConfigureAwait(true);
		deviceProperties.Should().Contain(dp => dp.Name == "location");
		deviceProperties.Should().Contain(dp => dp.Type == PropertyType.Auto);
	}

	[Fact]
	public async Task GetDevicesAndInstancesAssociatedWithDataSourceById()
	{
		// Get the dataSource
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", default)
			.ConfigureAwait(true);
		dataSource.Should().NotBeNull();

		// Get the information
		var info = await LogicMonitorClient
			.GetDevicesAndInstancesAssociatedWithDataSourceByIdPageAsync(
				dataSource!.Id,
				new Filter<DeviceWithDataSourceInstanceInformation> { Skip = 0, Take = 300 },
				default)
			.ConfigureAwait(true);

		// Check
		info.Should().NotBeNull();
	}

	[Fact]
	public void GetDevicesByDeviceGroupFullPath_InvalidDeviceGroup_ThrowsException()
	{
		LogicMonitorClient
			.Invoking(async x => await x.GetDevicesByDeviceGroupFullPathAsync("XXXXXX/YYYYYY", true, default).ConfigureAwait(true))
			.Should()
			.ThrowAsync<LogicMonitorApiException>();
		LogicMonitorClient
			.Invoking(async x => await x.GetDevicesByDeviceGroupFullPathAsync("XXXXXX/YYYYYY", false, default).ConfigureAwait(true))
			.Should()
			.ThrowAsync<LogicMonitorApiException>();
	}

	[Fact]
	public async Task GetDevicesByDeviceGroupFullPathV78()
	{
		// Recurse
		var allDatacenterDevices = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true, default)
			.ConfigureAwait(true);
		// Don't recurse
		var topFolderDevices = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, false, default)
			.ConfigureAwait(true);

		// Make sure there are more when recursing
		allDatacenterDevices.Should().HaveCountGreaterThan(topFolderDevices.Count);
		allDatacenterDevices.Should().AllSatisfy(d => d.DisplayName.Should().NotBeNull());
	}

	[Fact]
	public async Task GetDevicesRecurse()
	{
		var allStaffDevices = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true, default)
			.ConfigureAwait(true);
		allStaffDevices.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetFewerDevices()
	{
		const int numberToFetch = 10;
		var devices = await LogicMonitorClient
			.GetPageAsync(new Filter<Device> { Skip = 0, Take = numberToFetch }, default)
			.ConfigureAwait(true);
		devices.Items.Should().HaveCount(numberToFetch);
		devices.TotalCount.Should().NotBe(numberToFetch);
		devices.TotalCount.Should().NotBe(0);
	}

	[Fact]
	public async Task GetFullDeviceTree()
	{
		var deviceGroup = await LogicMonitorClient
			.GetFullDeviceTreeAsync(cancellationToken: default)
			.ConfigureAwait(true);
		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetFullDeviceTreeForDatacenter()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		deviceGroup = await LogicMonitorClient
			.GetFullDeviceTreeAsync(deviceGroup.Id, default)
			.ConfigureAwait(true);
		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task TreeNodeFreeSearch()
	{
		// Get the result without specifying a type
		var treeNodeFreeSearch = await LogicMonitorClient
			.TreeNodeFreeSearchAsync("Datacenter", 100, cancellationToken: default)
			.ConfigureAwait(true);
		var totalItemCount = treeNodeFreeSearch.Count;

		// Subtract the count of a search for each individual type
		var treeNodeFreeSearchResultTypes =
		Enum.GetValues(typeof(TreeNodeFreeSearchResultType))
		.Cast<TreeNodeFreeSearchResultType>()
		.Except([TreeNodeFreeSearchResultType.Unknown]);
		var list = new List<TreeNodeFreeSearchResult>();
		foreach (var treeNodeFreeSearchResultType in treeNodeFreeSearchResultTypes)
		{
			var treeNodeFreeSearchResult = await LogicMonitorClient
				.TreeNodeFreeSearchAsync("Datacenter", 100, treeNodeFreeSearchResultType)
				.ConfigureAwait(true);
			list.AddRange(treeNodeFreeSearchResult);
		}
		// Make sure that some are returned
		list.Should().HaveCount(totalItemCount);
	}

	[Fact]
	public async Task UpdateDeviceProperty()
	{
		var portalClient = LogicMonitorClient;
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);

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
			.PutAsync(device, default)
			.ConfigureAwait(true);

		// Re-fetch the device
		device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);

		// Make sure that there is now one called "test"
		var testProperty = device
			.CustomProperties
			.SingleOrDefault(p => p.Name == testPropertyName);
		testProperty.Should().NotBeNull();
		testProperty ??= new();
		testProperty.Value.Should().Be(testPropertyValue);

		// Clean up afterwards - remove the property
		// Re-fetch the properties
		device.CustomProperties = device.CustomProperties.Where(p => p.Name != testPropertyName).ToList();

		// Update the device
		await portalClient.PutAsync(device, default)
			.ConfigureAwait(true);

		// Re-fetch the properties
		device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);

		// Make sure that there are none called "test"
		device.CustomProperties.Should().AllSatisfy(p => p.Name.Should().NotBe(testPropertyName));
	}

	[Fact]
	public async Task SetDeviceCustomProperty()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		const string propertyName = "blah";
		const string value1 = "test1";
		const string value2 = "test2";

		// Set it to an expected value
		await LogicMonitorClient
			.SetDeviceCustomPropertyAsync(device.Id, propertyName, value1, default)
			.ConfigureAwait(true);
		var deviceProperties = await LogicMonitorClient
			.GetDevicePropertiesAsync(device.Id, default)
			.ConfigureAwait(true);
		var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Set it to a different value
		await LogicMonitorClient
			.SetDeviceCustomPropertyAsync(device.Id, propertyName, value2, default)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetDevicePropertiesAsync(device.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Set it to null (delete it)
		await LogicMonitorClient
			.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, cancellationToken: default)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetDevicePropertiesAsync(device.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);

		// This should fail as there is nothing to delete
		var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient
				.SetDeviceCustomPropertyAsync(
					device.Id,
					propertyName,
					null,
					SetPropertyMode.Delete,
					default)
				.ConfigureAwait(true))
			.ConfigureAwait(true);
		deletionException.Should().BeOfType<LogicMonitorApiException>();

		var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Update, default).ConfigureAwait(true)).ConfigureAwait(true);
		updateException.Should().BeOfType<InvalidOperationException>();

		var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Create, default).ConfigureAwait(true)).ConfigureAwait(true);
		createNullException.Should().BeOfType<InvalidOperationException>();

		// Create one without checking
		await LogicMonitorClient
			.SetDeviceCustomPropertyAsync(device.Id, propertyName, value1, SetPropertyMode.Create, cancellationToken: default)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetDevicePropertiesAsync(device.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Update one without checking
		await LogicMonitorClient
			.SetDeviceCustomPropertyAsync(device.Id, propertyName, value2, SetPropertyMode.Update, cancellationToken: default)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetDevicePropertiesAsync(device.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Delete one without checking
		await LogicMonitorClient
			.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Delete, cancellationToken: default)
			.ConfigureAwait(true);
		deviceProperties = await LogicMonitorClient
			.GetDevicePropertiesAsync(device.Id, default)
			.ConfigureAwait(true);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);
	}

	[Fact]
	public async Task GetDeviceCustomProperties()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, default)
			.ConfigureAwait(true);
		deviceProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceUsingSubUrl()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var deviceRefetch = await LogicMonitorClient
			.GetAllAsync<JObject>($"device/devices?filter=id:{device.Id}&fields=inheritedProperties", default)
			.ConfigureAwait(true);
		deviceRefetch.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDifferentDeviceTypes()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		device.DeviceType.Should().Be(DeviceType.Regular);

		device = await GetServiceDeviceAsync(default)
			.ConfigureAwait(true);
		device.DeviceType.Should().Be(DeviceType.Service);

		// TODO - AWS and Azure
	}

	[Fact]
	public async Task GetDeadDevices()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(new Filter<Device>
		{
			FilterItems =
			[
				new Eq<Device>(nameof(Device.DeviceStatus), "dead")
			]
		}, default).ConfigureAwait(true);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAlertDisableDevices_Filtered()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(new Filter<Device>
		{
			FilterItems =
			[
				new Ne<Device>(nameof(Device.AlertDisableStatus), "none-none-none")
			]
		}, default)
		.ConfigureAwait(true);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAlertDisableDevices_RawFilter()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(
			new Filter<Device> { QueryString = "filter=alertDisableStatus!:\"none-none-none\"" },
			default)
			.ConfigureAwait(true);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDevicesFromRoot()
	{
		// Fetch in ascending order to get unchanging DeviceGroup first
		var deviceGroups = await LogicMonitorClient
			.GetAllAsync(new Filter<DeviceGroup>
			{
				Order = new Order<DeviceGroup> { Property = nameof(DeviceGroup.Id), Direction = OrderDirection.Asc }
			}, default)
			.ConfigureAwait(true);
		var deviceGroup = deviceGroups.Find(dg => dg.SubGroups.Count != 0);
		deviceGroup.Should().NotBeNull();
		deviceGroup ??= new();
		var deviceList = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync(deviceGroup.FullPath, true, default)
			.ConfigureAwait(true);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceAlertsPageAsync()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var alertsPage = await LogicMonitorClient
			.GetDeviceAlertsPageAsync(device.Id, 0, 100, default)
			.ConfigureAwait(true);
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
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		device.Should().NotBeNull();

		// Get the Alerts
		var allAlerts = await LogicMonitorClient
			.GetDeviceAlertsByIdAsync(device.Id, alertFilter, default)
			.ConfigureAwait(true);
		allAlerts.Should().NotBeNull();
	}

	[Fact]
	public async Task PatchDeviceAsync()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		var oldDescription = device.Description;
		var newDescription = Guid.NewGuid().ToString();
		await LogicMonitorClient
			.PatchAsync(device, new Dictionary<string, object> { { "description", newDescription } }, default)
			.ConfigureAwait(true);
		var updatedDevice = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);
		updatedDevice.Description.Should().Be(newDescription);
		await LogicMonitorClient
			.PatchAsync(device, new Dictionary<string, object> { { "description", oldDescription } }, default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task ScheduleActiveDiscovery()
		=> await LogicMonitorClient
			.ScheduleActiveDiscovery(WindowsDeviceId, default)
			.ConfigureAwait(true);

	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetDeviceAlertSettings()
	{
		var device = await GetWindowsDeviceAsync(default)
			.ConfigureAwait(true);

		var config = await LogicMonitorClient
			.GetDeviceDataPointConfigurationsAsync(device.Id, default)
			.ConfigureAwait(true);

		config.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetTopTalkerGraphAsync()
	{
		var devicesPage = await LogicMonitorClient
			.GetDevicesPageAsync(new Filter<Device> { Skip = 0, Take = 50 }, default)
			.ConfigureAwait(true);

		var netflowEnabledId = 0;

		foreach (var device in devicesPage.Items)
		{
			if (device.EnableNetflow)
			{
				netflowEnabledId = device.Id;
			}
		}

		var graph = await LogicMonitorClient
			.GetTopTalkersGraphAsync(netflowEnabledId, default)
			.ConfigureAwait(true);
	}
}
