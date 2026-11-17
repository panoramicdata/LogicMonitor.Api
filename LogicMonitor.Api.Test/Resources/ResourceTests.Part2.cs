using System.Globalization;

namespace LogicMonitor.Api.Test.Resources;

public partial class ResourceTests
{

	[Fact]
	public async Task GetResourcePropertiesContainsExpected()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var deviceProperties = await LogicMonitorClient
			.GetResourcePropertiesAsync(device.Id, CancellationToken);
		deviceProperties.Should().Contain(dp => dp.Name == "location");
		deviceProperties.Should().Contain(dp => dp.Type == PropertyType.Auto);
	}

	[Fact]
	public async Task GetResourcesAndInstancesAssociatedWithDataSourceById()
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
				CancellationToken);

		// Check
		info.Should().NotBeNull();
	}

	[Fact]
	public void GetResourcesByDeviceGroupFullPath_InvalidDeviceGroup_ThrowsException()
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
	public async Task GetResourcesByDeviceGroupFullPathV78()
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
	public async Task GetResourcesRecurse()
	{
		var allStaffDevices = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(DeviceGroupFullPath, true, CancellationToken);
		allStaffDevices.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetFewerResources()
	{
		const int numberToFetch = 10;
		var devices = await LogicMonitorClient
			.GetPageAsync(new Filter<Resource> { Skip = 0, Take = numberToFetch }, CancellationToken);
		devices.Items.Should().HaveCount(numberToFetch);
		devices.TotalCount.Should().NotBe(numberToFetch);
		devices.TotalCount.Should().NotBe(0);
	}

	[Fact]
	public async Task GetFullResourceTree()
	{
		var deviceGroup = await LogicMonitorClient
			.GetFullResourceTreeAsync(cancellationToken: CancellationToken);
		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetFullResourceTreeForDatacenter()
	{
		var deviceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();
		deviceGroup = await LogicMonitorClient
			.GetFullResourceTreeAsync(deviceGroup.Id, CancellationToken);
		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task TreeNodeFreeSearch()
	{
		// Get the result without specifying a type
		var treeNodeFreeSearch = await LogicMonitorClient
			.TreeNodeFreeSearchAsync("Datacenter", 100, cancellationToken: CancellationToken);
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
				.TreeNodeFreeSearchAsync("Datacenter", 100, treeNodeFreeSearchResultType);
			list.AddRange(treeNodeFreeSearchResult);
		}
		// Make sure that some are returned
		list.Should().HaveCount(totalItemCount);
	}

	[Fact]
	public async Task UpdateResourceProperty()
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
	public async Task GetResourceCustomProperties()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var deviceProperties = await LogicMonitorClient.GetResourcePropertiesAsync(device.Id, CancellationToken);
		deviceProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetResourceUsingSubUrl()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var deviceRefetch = await LogicMonitorClient
			.GetAllAsync<JObject>($"device/devices?filter=id:{device.Id}&fields=inheritedProperties", CancellationToken);
		deviceRefetch.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDifferentResourceTypes()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		device.ResourceType.Should().Be(ResourceType.Regular);

		device = await GetServiceDeviceAsync(CancellationToken);
		device.ResourceType.Should().Be(ResourceType.Service);

		// TODO - AWS and Azure
	}

	[Fact]
	public async Task GetDeadResources()
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
	public async Task GetAlertDisableResources_Filtered()
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
	public async Task GetAlertDisableResources_RawFilter()
	{
		var deviceList = await LogicMonitorClient.GetAllAsync(
			new Filter<Resource> { QueryString = "filter=alertDisableStatus!:\"none-none-none\"" },
			CancellationToken);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetResourcesFromRoot()
	{
		// Fetch groups ordered by ID ascending, find one with direct resources
		var deviceGroups = await LogicMonitorClient
			.GetAllAsync(new Filter<ResourceGroup>
			{
				Order = new Order<ResourceGroup> { Property = nameof(ResourceGroup.Id), Direction = OrderDirection.Asc }
			}, CancellationToken);
		var deviceGroup = deviceGroups.Find(dg => dg.DirectResourceCount > 0);
		deviceGroup.Should().NotBeNull();
		deviceGroup ??= new();
		var deviceList = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync(deviceGroup.FullPath, false, CancellationToken);
		deviceList.Should().NotBeNull();
		deviceList.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetResourceAlertsPageAsync()
	{
		var device = await GetWindowsResourceAsync(CancellationToken);
		var alertsPage = await LogicMonitorClient
			.GetResourceAlertsPageAsync(device.Id, 0, 100, CancellationToken);
		alertsPage.Should().NotBeNull();
		alertsPage.Items.Should().HaveCount(alertsPage.TotalCount);
	}

	[Fact]
	public async Task GetResourceAlertsWithFilterAsync()
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
}
