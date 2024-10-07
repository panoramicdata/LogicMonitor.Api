using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Resources;

public class ResourceGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetDeviceGroupByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetDevicesByDeviceGroupByFullPathWhereNameContainsParentheses()
	{
		// No recurse
		var nonrecurse = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync("DSW - (Test) Denise Home Network/Sub Group 2 (XYZ)", false, default)
			.ConfigureAwait(true);

		// Recurse
		var recursed = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync("DSW - (Test) Denise Home Network/Sub Group 2 (XYZ)", true, default)
			.ConfigureAwait(true);

		nonrecurse.Should().ContainSingle();
		recursed.Should().HaveCount(2);
	}

	[Fact]
	public async Task SetAlertThreshold()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", default)
			.ConfigureAwait(true);
		dataSource ??= new();
		var datapoints = (await LogicMonitorClient
			.GetDataSourceDataPointsPageAsync(dataSource.Id, new Filter<DataPoint> { Skip = 0, Take = 10 }, default)
			.ConfigureAwait(true)).Items;
		var datapoint = datapoints.Single(dp => dp.Name == "average");
		await LogicMonitorClient
			.SetResourceGroupDataSourceDataPointThresholdsAsync(
				resourceGroup.Id,
				dataSource.Id,
				datapoint.Id,
				"> 191 timezone=Europe/London",
				$"Set by LogicMonitor.Api Unit Test at {DateTimeOffset.UtcNow}",
				true,
				default)
			.ConfigureAwait(true);
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetDeviceGroupByFullPath_InvalidDeviceGroup_ReturnsNull()
	{
		var nonExistentResourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync("XXXXXX/YYYYYY", default)
			.ConfigureAwait(true);
		nonExistentResourceGroup.Should().BeNull();
	}

	[Fact]
	public async Task GetDeviceGroupById()
	{
		var rootDeviceGroup = await LogicMonitorClient
			.GetAsync<ResourceGroup>(1, default)
			.ConfigureAwait(true);
		rootDeviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetRootDeviceGroupByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync("", default)
			.ConfigureAwait(true);
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetAllDeviceGroups()
	{
		// For LMREP-4060, to test if there are any missing DeviceGroupTypes
		var resourceGroups = await LogicMonitorClient
			.GetAllAsync<ResourceGroup>(default)
			.ConfigureAwait(true);
		resourceGroups.Should().NotBeNull();
		resourceGroups.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		resourceGroups.Select(dg => dg.Id).HasDuplicates().Should().BeFalse();

		// Make sure that all have non-zero Parent Ids
		resourceGroups.Should().AllSatisfy(dg => (dg.Id == 1 || dg.ParentId != 0).Should().BeTrue());
	}

	[Fact]
	public async Task GetDeviceGroupProperties()
	{
		var resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesAsync(1, default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		resourceGroupProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceGroupPropertiesByName()
	{
		var resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesByNameAsync(1, "api.account", default)
			.ConfigureAwait(true);
		resourceGroupProperties.Name.Should().NotBe(string.Empty);
	}

	[Fact]
	public async Task GetDeviceGroupsByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);

		var resourceGroups = resourceGroup.SubGroups;

		// Make sure that some are returned
		resourceGroups.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		resourceGroups.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetOneDeviceGroup_Succeeds()
	{
		var resourceGroup = await LogicMonitorClient
			.GetAllAsync(new Filter<ResourceGroup> { Take = 1 }, default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		resourceGroup.Should().ContainSingle();
	}

	[Fact]
	public async Task GetDeviceGroupsByParentId()
	{
		var resourceGroup = await LogicMonitorClient
			.GetAsync<ResourceGroup>(1, default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		resourceGroup.SubGroups.Should().NotBeNullOrEmpty();

		// Make sure that all children have Unique Ids
		resourceGroup.SubGroups.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	/// <summary>
	/// Get a ResourceGroup that has a + character in the name (must be escaped)
	/// </summary>
	[Fact]
	public async Task GetDeviceGroupByFullPathWithPlusInName()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);

		resourceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task SetDeviceGroupCustomProperty()
	{
		// Make sure a ResourceGroup exists for testing purposes
		const string testResourceGroupName = "Property Test ResourceGroup";
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(
				testResourceGroupName,
				default)
				.ConfigureAwait(true);

		resourceGroup ??= await LogicMonitorClient.CreateAsync(
					  new ResourceGroupCreationDto
					  {
						  ParentId = "1",
						  Name = testResourceGroupName
					  }, default)
				.ConfigureAwait(true);

		try
		{
			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(
				resourceGroup.Id,
				propertyName,
				value1,
				SetPropertyMode.Automatic,
				default).ConfigureAwait(true);
			var resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, default)
				.ConfigureAwait(true);
			var actual = resourceGroupProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Set it to a different value
			await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(
				resourceGroup.Id,
				propertyName,
				value2,
				SetPropertyMode.Automatic,
				default).ConfigureAwait(true);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, default)
				.ConfigureAwait(true);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete it
			await LogicMonitorClient
				.DeleteResourceGroupPropertyAsync(resourceGroup.Id, propertyName, default)
				.ConfigureAwait(true);

			//await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(
			//	deviceGroup.Id,
			//	propertyName,
			//	null,
			//	SetPropertyMode.Automatic,
			//	default).ConfigureAwait(true);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, default)
				.ConfigureAwait(true);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, null, SetPropertyMode.Delete, cancellationToken: default).ConfigureAwait(true)).ConfigureAwait(true);
			deletionException.Should().BeOfType<LogicMonitorApiException>();

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, null, SetPropertyMode.Update, cancellationToken: default).ConfigureAwait(true)).ConfigureAwait(true);
			updateException.Should().BeOfType<InvalidOperationException>();

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, null, SetPropertyMode.Create, cancellationToken: default).ConfigureAwait(true)).ConfigureAwait(true);
			createNullException.Should().BeOfType<InvalidOperationException>();

			// Create one without checking
			await LogicMonitorClient
				.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, value1, SetPropertyMode.Create, default)
				.ConfigureAwait(true);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, default)
				.ConfigureAwait(true);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Update one without checking
			await LogicMonitorClient
				.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, value2, SetPropertyMode.Update, default)
				.ConfigureAwait(true);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, default)
				.ConfigureAwait(true);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete one without checking
			await LogicMonitorClient
				.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, null, SetPropertyMode.Delete, default)
				.ConfigureAwait(true);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, default)
				.ConfigureAwait(true);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);
		}
		finally
		{
			await LogicMonitorClient
				.DeleteAsync(resourceGroup, true, default)
				.ConfigureAwait(true);
		}
	}

	[Fact]
	public async Task GetAwsExternalId()
	{
		var awsId = await LogicMonitorClient
			.GetExternalIdAsync(default)
			.ConfigureAwait(true);
		awsId.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceGroupSDTs()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		var sdts = await LogicMonitorClient
			.GetResourceGroupSdtsAsync(resourceGroup.Id, new Filter<ScheduledDownTime>(), default)
			.ConfigureAwait(true);
		sdts.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceGroupAlerts()
	{
		var alerts = await LogicMonitorClient
			.GetResourceGroupAlertsAsync(1)
			.ConfigureAwait(true);
		alerts.Items.Should().NotBeEmpty();
	}
}
