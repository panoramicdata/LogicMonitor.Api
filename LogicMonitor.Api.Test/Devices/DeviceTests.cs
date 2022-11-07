using System.Globalization;

namespace LogicMonitor.Api.Test.Devices;

public class DeviceTests : TestWithOutput
{
	public DeviceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetDevicesByDeviceGroupRecursive()
	{
		var devices = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken.None)
			.ConfigureAwait(false);

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
		var deviceProperty = new Property
		{
			Name = devicePropertyName,
			Value = devicePropertyValue
		};

		// Device group properties
		const string deviceGroupName = "UnitTest CreateAndRemoveDeviceAndDeviceGroup";
		const string deviceGroupDescription = "UnitTest CreateAndRemoveDeviceAndDeviceGroup Description";
		const int deviceGroupParentId = 1;

		// Delete device if it already exists
		var deviceForDeletion = await LogicMonitorClient.GetDeviceByDisplayNameAsync(deviceDisplayName, CancellationToken.None).ConfigureAwait(false);
		if (deviceForDeletion is not null)
		{
			await logicMonitorClient
				.DeleteAsync(deviceForDeletion, cancellationToken: CancellationToken.None)
				.ConfigureAwait(false);
		}

		// Delete device group if it already exists
		var deviceGroupForDeletion = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(deviceGroupName, CancellationToken.None)
			.ConfigureAwait(false);
		if (deviceGroupForDeletion is not null)
		{
			await logicMonitorClient
				.DeleteAsync(deviceGroupForDeletion, cancellationToken: CancellationToken.None)
				.ConfigureAwait(false);
		}

		// Get an active collector
		var collectorId = (await logicMonitorClient
			.GetAllAsync<Collector>(CancellationToken.None)
			.ConfigureAwait(false))
		.OrderBy(c => c.Id)
		.FirstOrDefault(c => !c.IsDown)?.Id;
		collectorId.Should().NotBeNull();

		// Create device group
		var deviceGroup = await logicMonitorClient.CreateAsync(new DeviceGroupCreationDto
		{
			Name = deviceGroupName,
			Description = deviceGroupDescription,
			ParentId = deviceGroupParentId.ToString(CultureInfo.InvariantCulture)
		}, CancellationToken.None).ConfigureAwait(false);

		foreach (var finalHardDelete in new bool[] { false, true })
		{
			// Create device
			var deviceCreationDto = new DeviceCreationDto
			{
				Name = deviceName,
				DisplayName = deviceDisplayName,
				Link = deviceLink,
				DeviceGroupIds = $"{deviceGroup.Id}",
				CustomProperties = new List<Property>
					{
						deviceProperty
					},
				Description = deviceDescription,
				DisableAlerting = deviceDisableAlerting,
				EnableNetflow = deviceEnableNetflow,
				PreferredCollectorId = collectorId.Value
			};
			var deviceFromCreation = await logicMonitorClient
				.CreateAsync(
					deviceCreationDto,
					CancellationToken.None)
				.ConfigureAwait(false);

			foreach (var device in new[] { deviceFromCreation, await logicMonitorClient.GetAsync<Device>(deviceFromCreation.Id, CancellationToken.None).ConfigureAwait(false) })
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
				.DeleteAsync(deviceFromCreation, false, cancellationToken: CancellationToken.None)
				.ConfigureAwait(false);

			// Get recycle bin items
			var recycleBinItems = await logicMonitorClient
				.GetAllAsync<RecycleBinItem>(CancellationToken.None)
				.ConfigureAwait(false);

			// Assert that the device is in the recycle bin
			recycleBinItems.Select(i => i.ResourceId).Should().Contain(deviceFromCreation.Id);

			// Get the recycle bin item
			var recycleBinItem = recycleBinItems.SingleOrDefault(i => i.ResourceId == deviceFromCreation.Id);
			recycleBinItem.Should().NotBeNull();
			deviceFromCreation.DisplayName.Should().Be(recycleBinItem.ResourceName);
			recycleBinItem.ResourceType.Should().Be("device");

			// Hard delete it?
			if (finalHardDelete)
			{
				// Restore from recycle bin
				await logicMonitorClient
					.RecycleBinRestoreAsync(new List<string> { recycleBinItem.Id }, CancellationToken.None)
					.ConfigureAwait(false);

				// Get the recycle bin item and make sure it was restored correctly
				recycleBinItems = await logicMonitorClient
					.GetAllAsync<RecycleBinItem>(CancellationToken.None)
					.ConfigureAwait(false);
				recycleBinItem = recycleBinItems
					.SingleOrDefault(i => i.ResourceId == deviceFromCreation.Id);
				recycleBinItem.Should().BeNull();

				// Do a regular hard delete
				await logicMonitorClient
					.DeleteAsync(deviceFromCreation, cancellationToken: CancellationToken.None)
					.ConfigureAwait(false);
			}
			else
			{
				// Remove from the recycle bin
				await logicMonitorClient
					.RecycleBinDeleteAsync(new List<string> { recycleBinItem.Id }, CancellationToken.None)
					.ConfigureAwait(false);
			}
		}

		// Delete device group
		await logicMonitorClient
			.DeleteAsync(deviceGroup, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task GetAllScheduledDownTimes()
	{
		var scheduledDownTimes = await LogicMonitorClient
			.GetAllAsync<ScheduledDownTime>(CancellationToken.None)
			.ConfigureAwait(false);

		scheduledDownTimes.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDevicePage()
	{
		const int MaxCount = 50;
		var devicesPage = await LogicMonitorClient
			.GetDevicesPageAsync(new Filter<Device> { Skip = 0, Take = MaxCount }, CancellationToken.None)
			.ConfigureAwait(false);

		devicesPage.Should().NotBeNull();
		(devicesPage.Items.Count <= MaxCount).Should().BeTrue();
	}

	[Fact]
	public async Task GetAllDeviceInstances()
	{
		var deviceInstances = await LogicMonitorClient
			.GetAllDeviceInstances(WindowsDeviceId, new Filter<DeviceDataSourceInstance>
			{
				Properties = new List<string>
				{
						nameof(DeviceDataSourceInstance.Id),
						nameof(DeviceDataSourceInstance.Name),
						nameof(DeviceDataSourceInstance.DataSourceName),
				}
			}, CancellationToken.None)
			.ConfigureAwait(false);
		(deviceInstances.Count > 10).Should().BeTrue();
	}

	[Fact]
	public async Task GetDeviceByDeviceId()
	{
		var device = await LogicMonitorClient
			.GetAsync<Device>(77, CancellationToken.None)
			.ConfigureAwait(false);
		device.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceByDisplayNameAsync()
	{
		var device = await GetWindowsDeviceAsync()
			.ConfigureAwait(false);
		var device2 = await LogicMonitorClient
			.GetDeviceByDisplayNameAsync(device.DisplayName, CancellationToken.None)
			.ConfigureAwait(false);
		device2.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceByHostName()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var devices = await LogicMonitorClient
			.GetDevicesByHostNameAsync(device.Name, 100, CancellationToken.None)
			.ConfigureAwait(false);
		devices.Should().ContainSingle();
	}

	[Fact]
	public async Task DateTimeSetCorrectly()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var refresh = await LogicMonitorClient
			.GetAsync<Device>(device.Id, CancellationToken.None)
			.ConfigureAwait(false);
		refresh.CreatedOnSeconds.Should().BePositive();
		refresh.CreatedOnUtc.Value.Should().BeAfter(DateTime.Parse("2012-07-24", CultureInfo.InvariantCulture));
	}

	[Fact]
	public async Task SerialisationIgnoredProperties()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		device.Should().NotBeNull();
		var jObject = JObject.FromObject(device);
		int.Parse(jObject["id"]!.ToString(), CultureInfo.InvariantCulture).Should().Be(device.Id);
		jObject.Properties().Should().AllSatisfy(p => p.Name.Should().NotBe(nameof(Device.AutoPropertiesAssignedOnUtc)));
	}

	[Fact]
	public async Task GetDevicePropertiesContainsExpected()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, CancellationToken.None).ConfigureAwait(false);
		deviceProperties.Should().Contain(dp => dp.Name == "location");
		deviceProperties.Should().Contain(dp => dp.Type == PropertyType.Auto);
	}

	[Fact]
	public async Task GetDevicesAndInstancesAssociatedWithDataSourceById()
	{
		// Get the dataSource
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinVolumeUsage-", CancellationToken.None).ConfigureAwait(false);
		dataSource.Should().NotBeNull();

		// Get the information
		var info = await LogicMonitorClient
			.GetDevicesAndInstancesAssociatedWithDataSourceByIdPageAsync(
				dataSource!.Id,
				new Filter<DeviceWithDataSourceInstanceInformation> { Skip = 0, Take = 300 },
				CancellationToken.None)
			.ConfigureAwait(false);

		// Check
		info.Should().NotBeNull();
	}

	[Fact]
	public void GetDevicesByDeviceGroupFullPath_InvalidDeviceGroup_ThrowsException()
	{
		LogicMonitorClient
			.Invoking(async x => await x.GetDevicesByDeviceGroupFullPathAsync("XXXXXX/YYYYYY", true, CancellationToken.None).ConfigureAwait(false))
			.Should()
			.ThrowAsync<LogicMonitorApiException>();
		LogicMonitorClient
			.Invoking(async x => await x.GetDevicesByDeviceGroupFullPathAsync("XXXXXX/YYYYYY", false, CancellationToken.None).ConfigureAwait(false))
			.Should()
			.ThrowAsync<LogicMonitorApiException>();
	}

	[Fact]
	public async Task GetDevicesByDeviceGroupFullPathv78()
	{
		// Recurse
		var allDatacenterDevices = await LogicMonitorClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken.None).ConfigureAwait(false);
		// Don't recurse
		var topFolderDevices = await LogicMonitorClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, false, CancellationToken.None).ConfigureAwait(false);

		// Make sure there are more when recursing
		allDatacenterDevices.Should().HaveCountGreaterThan(topFolderDevices.Count);
		allDatacenterDevices.Should().AllSatisfy(d => d.DisplayName.Should().NotBeNull());
	}

	[Fact]
	public async Task GetDevicesRecurse()
	{
		var allStaffDevices = await LogicMonitorClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken.None).ConfigureAwait(false);
		allStaffDevices.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetFewerDevices()
	{
		const int numberToFetch = 10;
		var devices = await LogicMonitorClient.GetPageAsync(new Filter<Device> { Skip = 0, Take = numberToFetch }, CancellationToken.None).ConfigureAwait(false);
		devices.Items.Count.Should().Be(numberToFetch);
		devices.TotalCount.Should().NotBe(numberToFetch);
		devices.TotalCount.Should().NotBe(0);
	}

	[Fact]
	public async Task GetFullDeviceTree()
	{
		var deviceGroup = await LogicMonitorClient.GetFullDeviceTreeAsync(cancellationToken: CancellationToken.None).ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetFullDeviceTreeForDatacenter()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken.None).ConfigureAwait(false);
		deviceGroup = await LogicMonitorClient.GetFullDeviceTreeAsync(deviceGroup.Id, CancellationToken.None).ConfigureAwait(false);
		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task TreeNodeFreeSearch()
	{
		// Get the result without specifying a type
		var treeNodeFreeSearch = await LogicMonitorClient.TreeNodeFreeSearchAsync("Datacenter", 100, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		var totalItemCount = treeNodeFreeSearch.Count;

		// Subtract the count of a search for each individual type
		var treeNodeFreeSearchResultTypes =
		Enum.GetValues(typeof(TreeNodeFreeSearchResultType))
		.Cast<TreeNodeFreeSearchResultType>()
		.Except(new[] { TreeNodeFreeSearchResultType.Unknown });
		var list = new List<TreeNodeFreeSearchResult>();
		foreach (var treeNodeFreeSearchResultType in treeNodeFreeSearchResultTypes)
		{
			var treeNodeFreeSearchResult = await LogicMonitorClient.TreeNodeFreeSearchAsync("Datacenter", 100, treeNodeFreeSearchResultType, CancellationToken.None).ConfigureAwait(false);
			list.AddRange(treeNodeFreeSearchResult);
		}
		// Make sure that some are returned
		list.Should().HaveCount(totalItemCount);
	}

	[Fact]
	public async Task UpdateDeviceProperty()
	{
		var portalClient = LogicMonitorClient;
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);

		// Remove any called "test"
		const string testPropertyName = "test";
		const string testPropertyValue = "testValue";
		var errantProperty = device.CustomProperties.SingleOrDefault(p => p.Name == testPropertyName);
		if (errantProperty is not null)
		{
			device.CustomProperties.Remove(errantProperty);
		}

		device.CustomProperties.Add(new Property
		{
			Name = testPropertyName,
			Value = testPropertyValue
		});

		// Update the device
		await portalClient.PutAsync(device, CancellationToken.None).ConfigureAwait(false);

		// Re-fetch the device
		device = await GetWindowsDeviceAsync().ConfigureAwait(false);

		// Make sure that there is now one called "test"
		var testProperty = device.CustomProperties.SingleOrDefault(p => p.Name == testPropertyName);
		testProperty.Should().NotBeNull();
		testProperty.Value.Should().Be(testPropertyValue);

		// Clean up afterwards - remove the property
		// Re-fetch the properties
		device.CustomProperties = device.CustomProperties.Where(p => p.Name != testPropertyName).ToList();

		// Update the device
		await portalClient.PutAsync(device, CancellationToken.None).ConfigureAwait(false);

		// Re-fetch the properties
		device = await GetWindowsDeviceAsync().ConfigureAwait(false);

		// Make sure that there are none called "test"
		device.CustomProperties.Should().AllSatisfy(p => p.Name.Should().NotBe(testPropertyName));
	}

	[Fact]
	public async Task SetDeviceCustomProperty()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		const string propertyName = "blah";
		const string value1 = "test1";
		const string value2 = "test2";

		// Set it to an expected value
		await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, value1, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		var deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, CancellationToken.None).ConfigureAwait(false);
		var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Set it to a different value
		await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, value2, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, CancellationToken.None).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Set it to null (delete it)
		await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, CancellationToken.None).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);

		// This should fail as there is nothing to delete
		var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient
				.SetDeviceCustomPropertyAsync(
					device.Id,
					propertyName,
					null,
					SetPropertyMode.Delete,
					CancellationToken.None)
				.ConfigureAwait(false))
			.ConfigureAwait(false);
		deletionException.Should().BeOfType<LogicMonitorApiException>();

		var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Update, CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);
		updateException.Should().BeOfType<InvalidOperationException>();

		var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Create, CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);
		createNullException.Should().BeOfType<InvalidOperationException>();

		// Create one without checking
		await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, value1, SetPropertyMode.Create, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, CancellationToken.None).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Update one without checking
		await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, value2, SetPropertyMode.Update, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, CancellationToken.None).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Delete one without checking
		await LogicMonitorClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Delete, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, CancellationToken.None).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);
	}

	[Fact]
	public async Task GetDeviceCustomProperties()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var deviceProperties = await LogicMonitorClient.GetDevicePropertiesAsync(device.Id, CancellationToken.None).ConfigureAwait(false);
		deviceProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceUsingSubUrl()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var deviceRefetch = await LogicMonitorClient
					.GetAllAsync<JObject>($"device/devices?filter=id:{device.Id}&fields=inheritedProperties", CancellationToken.None)
					.ConfigureAwait(false);
		deviceRefetch.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDifferentDeviceTypes()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		device.DeviceType.Should().Be(DeviceType.Regular);

		device = await GetServiceDeviceAsync().ConfigureAwait(false);
		device.DeviceType.Should().Be(DeviceType.Service);

		// TODO - AWS and Azure
	}

	[Fact]
	public async Task GetDeadDevices()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(new Filter<Device>
		{
			FilterItems = new List<FilterItem<Device>>
			{
				new Eq<Device>(nameof(Device.DeviceStatus), "dead")
			}
		}, CancellationToken.None).ConfigureAwait(false);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAlertDisableDevices_Filtered()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(new Filter<Device>
		{
			FilterItems = new List<FilterItem<Device>>
			{
				new Ne<Device>(nameof(Device.AlertDisableStatus), "none-none-none")
			}
		}, CancellationToken.None).ConfigureAwait(false);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAlertDisableDevices_RawFilter()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(
			new Filter<Device> { QueryString = "filter=alertDisableStatus!:\"none-none-none\"" },
			CancellationToken.None)
			.ConfigureAwait(false);
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
			}, CancellationToken.None)
			.ConfigureAwait(false);
		var deviceGroup = deviceGroups.Find(dg => dg.SubGroups.Count != 0);
		deviceGroup.Should().NotBeNull();
		var deviceList = await LogicMonitorClient.GetDevicesByDeviceGroupFullPathAsync(deviceGroup.FullPath, true, CancellationToken.None).ConfigureAwait(false);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceAlertsPageAsync()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var alertsPage = await LogicMonitorClient.GetDeviceAlertsPageAsync(device.Id, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		alertsPage.Should().NotBeNull();
		alertsPage.Items.Count.Should().Be(alertsPage.TotalCount);
	}

	[Fact]
	public async Task GetDeviceAlertsWithFilterAsync()
	{
		var alertFilter = new AlertFilter
		{
			IncludeCleared = true,
			Levels = new List<AlertLevel>
				 {
					 AlertLevel.Warning,
					 AlertLevel.Error,
					 AlertLevel.Critical
				 },
			StartUtcIsAfter = DateTime.UtcNow.AddDays(-25)
		};

		// Get the Device
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		device.Should().NotBeNull();

		// Get the Alerts
		var allAlerts = await LogicMonitorClient.GetDeviceAlertsByIdAsync(device.Id, alertFilter, default).ConfigureAwait(false);
		allAlerts.Should().NotBeNull();
	}

	[Fact]
	public async Task PatchDeviceAsync()
	{
		var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
		var oldDescription = device.Description;
		var newDescription = Guid.NewGuid().ToString();
		await LogicMonitorClient.PatchAsync(device, new Dictionary<string, object> { { "description", newDescription } }, CancellationToken.None).ConfigureAwait(false);
		var updatedDevice = await GetWindowsDeviceAsync().ConfigureAwait(false);
		updatedDevice.Description.Should().Be(newDescription);
		await LogicMonitorClient.PatchAsync(device, new Dictionary<string, object> { { "description", oldDescription } }, CancellationToken.None).ConfigureAwait(false);
	}

	[Fact]
	public async Task ScheduleActiveDiscovery()
		=> await LogicMonitorClient.ScheduleActiveDiscovery(WindowsDeviceId, CancellationToken.None).ConfigureAwait(false);
}
