using LogicMonitor.Api.Collectors;
using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using LogicMonitor.Api.RecycleBin;
using LogicMonitor.Api.ScheduledDownTimes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Devices
{
	public class DeviceTests : TestWithOutput
	{
		public DeviceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetDevicesByDeviceGroupRecursive()
		{
			var devices = await PortalClient
				.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true)
				.ConfigureAwait(false);

			Assert.NotNull(devices);
		}

		[Fact]
		public async void CreateAndRemoveDeviceAndDeviceGroup()
		{
			var portalClient = PortalClient;

			// Device properties
			const string deviceName = "8.8.8.8";
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
			var deviceForDeletion = await PortalClient.GetDeviceByDisplayNameAsync(deviceDisplayName).ConfigureAwait(false);
			if (deviceForDeletion != null)
			{
				await portalClient.DeleteAsync(deviceForDeletion).ConfigureAwait(false);
			}

			// Delete device group if it already exists
			var deviceGroupForDeletion = await PortalClient.GetDeviceGroupByFullPathAsync(deviceGroupName).ConfigureAwait(false);
			if (deviceGroupForDeletion != null)
			{
				await portalClient.DeleteAsync(deviceGroupForDeletion).ConfigureAwait(false);
			}

			// Get an active collector
			var collectorId = (await portalClient.GetAllAsync<Collector>().ConfigureAwait(false))
			.OrderBy(c => c.Id)
			.FirstOrDefault(c => !c.IsDown)?.Id;
			Assert.NotNull(collectorId);

			// Create device group
			var deviceGroup = await portalClient.CreateAsync(new DeviceGroupCreationDto
			{
				Name = deviceGroupName,
				Description = deviceGroupDescription,
				ParentId = deviceGroupParentId.ToString()
			}).ConfigureAwait(false);

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
				var deviceFromCreation = await portalClient.CreateAsync(deviceCreationDto).ConfigureAwait(false);

				foreach (var device in new[] { deviceFromCreation, await portalClient.GetAsync<Device>(deviceFromCreation.Id).ConfigureAwait(false) })
				{
					Assert.Equal(deviceName, device.Name);
					Assert.Equal(deviceDisplayName, device.DisplayName);
					Assert.Equal(deviceDescription, device.Description);
					Assert.Equal(deviceLink, device.Link);
					Assert.NotNull(device.CustomProperties);
					Assert.Equal(1, device.CustomProperties.Count(p => p.Name == devicePropertyName && p.Value == devicePropertyValue));
					Assert.Equal(devicePropertyName, device.CustomProperties[0].Name);
					Assert.Equal(devicePropertyValue, device.CustomProperties[0].Value);
					Assert.Equal(deviceDisableAlerting, device.IsAlertingDisabled);
					Assert.Equal(deviceEnableNetflow, device.EnableNetflow);
				}

				// Soft delete device
				await portalClient.DeleteAsync(deviceFromCreation, false).ConfigureAwait(false);

				// Get recycle bin items
				var recycleBinItems = await portalClient.GetAllAsync<RecycleBinItem>().ConfigureAwait(false);

				// Assert that the device is in the recycle bin
				Assert.Contains(deviceFromCreation.Id, recycleBinItems.Select(i => i.ResourceId));

				// Get the recycle bin item
				var recycleBinItem = recycleBinItems.SingleOrDefault(i => i.ResourceId == deviceFromCreation.Id);
				Assert.NotNull(recycleBinItem);
				Assert.Equal(recycleBinItem.ResourceName, deviceFromCreation.DisplayName);
				Assert.Equal("device", recycleBinItem.ResourceType);

				// Hard delete it?
				if (finalHardDelete)
				{
					// Restore from recycle bin
					await portalClient.RecycleBinRestoreAsync(new List<string> { recycleBinItem.Id }).ConfigureAwait(false);

					// Get the recycle bin item and make sure it was restored correctly
					recycleBinItems = await portalClient.GetAllAsync<RecycleBinItem>().ConfigureAwait(false);
					recycleBinItem = recycleBinItems.SingleOrDefault(i => i.ResourceId == deviceFromCreation.Id);
					Assert.Null(recycleBinItem);

					// Do a regular hard delete
					await portalClient.DeleteAsync(deviceFromCreation).ConfigureAwait(false);
				}
				else
				{
					// Remove from the recycle bin
					await portalClient.RecycleBinDeleteAsync(new List<string> { recycleBinItem.Id }).ConfigureAwait(false);
				}
			}

			// Delete device group
			await portalClient.DeleteAsync(deviceGroup).ConfigureAwait(false);
		}

		[Fact]
		public async void GetAllScheduledDownTimes()
		{
			var scheduledDownTimes = await PortalClient.GetAllAsync<ScheduledDownTime>().ConfigureAwait(false);

			Assert.NotNull(scheduledDownTimes);
		}

		[Fact]
		public async void GetDevicePage()
		{
			const int MaxCount = 50;
			var devicesPage = await PortalClient.GetDevicesPageAsync(new Filter<Device> { Skip = 0, Take = MaxCount }).ConfigureAwait(false);

			Assert.NotNull(devicesPage);
			Assert.True(devicesPage.Items.Count <= MaxCount);
		}

		[Fact]
		public async void GetAllDeviceInstances()
		{
			var deviceInstances = await PortalClient
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
			Assert.True(deviceInstances.Count > 300);
		}

		[Fact]
		public async void GetDeviceByDeviceId()
		{
			var device = await PortalClient.GetAsync<Device>(77).ConfigureAwait(false);

			Assert.NotNull(device);
		}

		[Fact]
		public async void GetDeviceByDisplayNameAsync()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var device2 = await PortalClient.GetDeviceByDisplayNameAsync(device.DisplayName).ConfigureAwait(false);
			Assert.NotNull(device2);
		}

		[Fact]
		public async void GetDeviceByHostName()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var devices = await PortalClient.GetDevicesByHostNameAsync(device.Name, 100).ConfigureAwait(false);
			Assert.True(devices.Count == 1);
		}

		[Fact]
		public async void DateTimeSetCorrectly()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var refresh = await PortalClient.GetAsync<Device>(device.Id).ConfigureAwait(false);
			Assert.True(refresh.CreatedOnSeconds > 0);
			Assert.True(refresh.CreatedOnUtc.Value > DateTime.Parse("2012-07-24"));
		}

		[Fact]
		public async void SerialisationIgnoredProperties()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			Assert.NotNull(device);
			var jObject = JObject.FromObject(device);
			Assert.Equal(device.Id, int.Parse(jObject["id"].ToString()));
			Assert.Empty(jObject.Properties().Where(p => string.Equals(p.Name, nameof(Device.AutoPropertiesAssignedOnUtc), StringComparison.InvariantCultureIgnoreCase)));
		}

		[Fact]
		public async void GetDevicePropertiesContainsLocation()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			Assert.Contains(deviceProperties, dp => dp.Name == "location");
		}

		[Fact]
		public async void GetDevicePropertiesContainsAutoProperties()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			Assert.Contains(deviceProperties, dp => dp.Type == PropertyType.Auto);
		}

		[Fact]
		public async void GetDevicesAndInstancesAssociatedWithDataSourceById()
		{
			// Get the dataSourceId
			var dataSourceId = (await PortalClient.GetDataSourceByUniqueNameAsync("WinVolumeUsage-").ConfigureAwait(false))?.Id;
			Assert.NotNull(dataSourceId);

			// Get the information
			var info = await PortalClient.GetDevicesAndInstancesAssociatedWithDataSourceByIdPageAsync((int)dataSourceId, new Filter<DeviceWithDataSourceInstanceInformation> { Skip = 0, Take = 300 }).ConfigureAwait(false);

			// Check
			Assert.NotNull(info);
			//Assert.True(allPulsantDevices.TotalCount >= topFolderDevices.TotalCount);
			//Assert.True(allPulsantDevices.All(d=>d.DisplayName!=null));
		}

		[Fact]
		public void GetDevicesByDeviceGroupFullPath_InvalidDeviceGroup_ThrowsException()
		{
			Assert.ThrowsAsync<LogicMonitorApiException>(async () => { var collection = await PortalClient.GetDevicesByDeviceGroupFullPathAsync("XXXXXX/YYYYYY", true).ConfigureAwait(false); });
			Assert.ThrowsAsync<LogicMonitorApiException>(async () => { var collection = await PortalClient.GetDevicesByDeviceGroupFullPathAsync("XXXXXX/YYYYYY", false).ConfigureAwait(false); });
		}

		[Fact]
		public async void GetDevicesByDeviceGroupFullPathv78()
		{
			// Recurse
			var allDatacenterDevices = await PortalClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true).ConfigureAwait(false);
			// Don't recurse
			var topFolderDevices = await PortalClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, false).ConfigureAwait(false);

			// Make sure there are more when recursing
			Assert.True(allDatacenterDevices.Count > topFolderDevices.Count);
			Assert.True(allDatacenterDevices.All(d => d.DisplayName != null));
		}

		[Fact]
		public async void GetDevicesRecurse()
		{
			var allStaffDevices = await PortalClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true).ConfigureAwait(false);
			Assert.NotEmpty(allStaffDevices);
		}

		[Fact]
		public async void GetFewerDevices()
		{
			const int numberToFetch = 10;
			var devices = await PortalClient.GetPageAsync(new Filter<Device> { Skip = 0, Take = numberToFetch }).ConfigureAwait(false);
			Assert.Equal(numberToFetch, devices.Items.Count);
			Assert.NotEqual(numberToFetch, devices.TotalCount);
			Assert.NotEqual(0, devices.TotalCount);
		}

		[Fact]
		public async void GetFullDeviceTree()
		{
			var deviceGroup = await PortalClient.GetFullDeviceTreeAsync().ConfigureAwait(false);
			Assert.NotNull(deviceGroup);
		}

		[Fact]
		public async void GetFullDeviceTreeForDatacenter()
		{
			var deviceGroup = await PortalClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
			deviceGroup = await PortalClient.GetFullDeviceTreeAsync(deviceGroup.Id).ConfigureAwait(false);
			Assert.NotNull(deviceGroup);
		}

		[Fact]
		public async void TreeNodeFreeSearch()
		{
			// Get the result without specifying a type
			var treeNodeFreeSearch = await PortalClient.TreeNodeFreeSearchAsync("Datacenter", 100).ConfigureAwait(false);
			var totalItemCount = treeNodeFreeSearch.Count;

			// Subtract the count of a search for each individual type
			var treeNodeFreeSearchResultTypes =
			Enum.GetValues(typeof(TreeNodeFreeSearchResultType))
			.Cast<TreeNodeFreeSearchResultType>()
			.Except(new[] { TreeNodeFreeSearchResultType.Unknown });
			var list = new List<TreeNodeFreeSearchResult>();
			foreach (var treeNodeFreeSearchResultType in treeNodeFreeSearchResultTypes)
			{
				var treeNodeFreeSearchResult = await PortalClient.TreeNodeFreeSearchAsync("Datacenter", 100, treeNodeFreeSearchResultType).ConfigureAwait(false);
				list.AddRange(treeNodeFreeSearchResult);
			}
			// Make sure that some are returned
			Assert.Equal(totalItemCount, list.Count);
		}

		[Fact]
		public async void UpdateDeviceProperty()
		{
			var portalClient = PortalClient;
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);

			// Remove any called "test"
			const string testPropertyName = "test";
			const string testPropertyValue = "testValue";
			var errantProperty = device.CustomProperties.SingleOrDefault(p => p.Name == testPropertyName);
			if (errantProperty != null)
			{
				device.CustomProperties.Remove(errantProperty);
			}

			device.CustomProperties.Add(new Property
			{
				Name = testPropertyName,
				Value = testPropertyValue
			});

			// Update the device
			await portalClient.PutAsync(device).ConfigureAwait(false);

			// Re-fetch the device
			device = await GetWindowsDeviceAsync().ConfigureAwait(false);

			// Make sure that there is now one called "test"
			var testProperty = device.CustomProperties.SingleOrDefault(p => p.Name == testPropertyName);
			Assert.NotNull(testProperty);
			Assert.Equal(testPropertyValue, testProperty.Value);

			// Clean up afterwards - remove the property
			// Re-fetch the properties
			device.CustomProperties = device.CustomProperties.Where(p => p.Name != testPropertyName).ToList();

			// Update the device
			await portalClient.PutAsync(device).ConfigureAwait(false);

			// Re-fetch the properties
			device = await GetWindowsDeviceAsync().ConfigureAwait(false);

			// Make sure that there are none called "test"
			Assert.DoesNotContain(device.CustomProperties, p => p.Name == testPropertyName);
		}

		[Fact]
		public async void SetDeviceCustomProperty()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, value1).ConfigureAwait(false);
			var deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Set it to a different value
			await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, value2).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Set it to null (delete it)
			await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<LogicMonitorApiException>(deletionException);

			var updateException = await Record.ExceptionAsync(async () => await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Update).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(updateException);

			var createNullException = await Record.ExceptionAsync(async () => await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Create).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(createNullException);

			// Create one without checking
			await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, value1, SetPropertyMode.Create).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Update one without checking
			await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, value2, SetPropertyMode.Update).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Delete one without checking
			await PortalClient.SetDeviceCustomPropertyAsync(device.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);
		}

		[Fact]
		public async void GetDeviceCustomProperties()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var deviceProperties = await PortalClient.GetDevicePropertiesAsync(device.Id).ConfigureAwait(false);
			Assert.NotEmpty(deviceProperties);
		}

		[Fact]
		public async void GetDeviceUsingSubUrl()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var deviceRefetch = await PortalClient
						.GetAllAsync<JObject>($"device/devices?filter=id:{device.Id}&fields=inheritedProperties", CancellationToken.None)
						.ConfigureAwait(false);
			Assert.NotNull(deviceRefetch);
		}

		[Fact]
		public async void GetDifferentDeviceTypes()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			Assert.Equal(DeviceType.Regular, device.DeviceType);

			device = await GetServiceDeviceAsync().ConfigureAwait(false);
			Assert.Equal(DeviceType.Service, device.DeviceType);

			// TODO - AWS and Azure
		}

		[Fact]
		public async void GetDeadDevices()
		{
			var deviceList = await PortalClient.GetAllAsync(new Filter<Device>
			{
				FilterItems = new List<FilterItem<Device>>
			{
				new Eq<Device>(nameof(Device.DeviceStatus), "dead")
			}
			}).ConfigureAwait(false);
			Assert.NotNull(deviceList);
			Assert.NotEmpty(deviceList);
		}

		[Fact]
		public async void GetAlertDisableDevices_Filtered()
		{
			var deviceList = await PortalClient.GetAllAsync(new Filter<Device>
			{
				FilterItems = new List<FilterItem<Device>>
			{
				new Ne<Device>(nameof(Device.AlertDisableStatus), "none-none-none")
			}
			}).ConfigureAwait(false);
			Assert.NotNull(deviceList);
			Assert.NotEmpty(deviceList);
		}

		[Fact]
		public async void GetAlertDisableDevices_RawFilter()
		{
			var deviceList = await PortalClient.GetAllAsync(new Filter<Device> { QueryString = "filter=alertDisableStatus!:\"none-none-none\"" }).ConfigureAwait(false);
			Assert.NotNull(deviceList);
			Assert.NotEmpty(deviceList);
		}

		[Fact]
		public async void GetDevicesFromRoot()
		{
			// Fetch in ascending order to get unchanging DeviceGroup first
			var deviceGroups = await PortalClient
				.GetAllAsync(new Filter<DeviceGroup>
				{
					Order = new Order<DeviceGroup> { Property = nameof(DeviceGroup.Id), Direction = OrderDirection.Asc }
				})
				.ConfigureAwait(false);
			var deviceGroup = deviceGroups.Find(dg => dg.SubGroups.Count != 0);
			Assert.NotNull(deviceGroup);
			var deviceList = await PortalClient.GetDevicesByDeviceGroupFullPathAsync(deviceGroup.FullPath, true).ConfigureAwait(false);
			Assert.NotNull(deviceList);
			Assert.NotEmpty(deviceList);
		}

		[Fact]
		public async void GetDeviceAlertsPageAsync()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var alertsPage = await PortalClient.GetDeviceAlertsPageAsync(device.Id).ConfigureAwait(false);
			Assert.NotNull(alertsPage);
			Assert.Equal(alertsPage.TotalCount, alertsPage.Items.Count);
		}

		[Fact]
		public async void PatchDeviceAsync()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var oldDescription = device.Description;
			var newDescription = Guid.NewGuid().ToString();
			await PortalClient.PatchAsync(device, new Dictionary<string, object> { { "description", newDescription } }).ConfigureAwait(false);
			var updatedDevice = await GetWindowsDeviceAsync().ConfigureAwait(false);
			Assert.Equal(newDescription, updatedDevice.Description);
			await PortalClient.PatchAsync(device, new Dictionary<string, object> { { "description", oldDescription } }).ConfigureAwait(false);
		}

		[Fact]
		public async void GetDeviceById1487()
		{
			var device = await PortalClient.GetAsync<Device>(1487).ConfigureAwait(false);
			Assert.NotNull(device);
		}

		[Fact]
		public async void ScheduleActiveDiscovery1487()
		{
			await PortalClient.ScheduleActiveDiscovery(1487).ConfigureAwait(false);
			Assert.True(true);
		}

		//[Fact]
		//public async void ChangeDeviceGroup()
		//{
		//	var device = await DefaultPortalClient.GetDeviceByDisplayNameAsync("FOURSIGHT001").ConfigureAwait(false);
		//	Assert.NotNull(device);

		//	// Record the old value
		//	var oldDeviceGroupId = device.DeviceGroupIdsString;

		//	// Set the new value
		//	await DefaultPortalClient.PatchPropertyAsync(device, nameof(Device.DeviceGroupIdsString), "1").ConfigureAwait(false);

		//	// Refetch to make sure that it was set
		//	device = await DefaultPortalClient.GetAsync<Device>(66).ConfigureAwait(false);

		//	// Make sure the new value is as expected
		//	Assert.Equal("1", device.DeviceGroupIdsString);

		//	// Put it back!
		//	await DefaultPortalClient.PatchPropertyAsync(device, nameof(Device.DeviceGroupIdsString), oldDeviceGroupId).ConfigureAwait(false);

		//	// Make sure that's back as it originally was
		//	Assert.Equal(oldDeviceGroupId, device.DeviceGroupIdsString);
		//}
	}
}