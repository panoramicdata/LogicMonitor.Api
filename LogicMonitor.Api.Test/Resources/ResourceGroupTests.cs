using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Resources;

public class ResourceGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetDeviceGroupByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetDevicesByDeviceGroupByFullPathWhereNameContainsParentheses()
	{
		// No recurse
		var nonrecurse = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync("DSW - (Test) Denise Home Network/Sub Group 2 (XYZ)", false, CancellationToken);

		// Recurse
		var recursed = await LogicMonitorClient
			.GetResourcesByResourceGroupFullPathAsync("DSW - (Test) Denise Home Network/Sub Group 2 (XYZ)", true, CancellationToken);

		nonrecurse.Should().ContainSingle();
		recursed.Should().HaveCount(2);
	}

	[Fact]
	public async Task SetAlertThreshold()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", CancellationToken);
		dataSource ??= new();
		var datapoints = (await LogicMonitorClient
			.GetDataSourceDataPointsPageAsync(dataSource.Id, new Filter<DataPoint> { Skip = 0, Take = 10 }, CancellationToken)
			).Items;
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
			;
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetDeviceGroupByFullPath_InvalidDeviceGroup_ReturnsNull()
	{
		var nonExistentResourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync("XXXXXX/YYYYYY", CancellationToken);
		nonExistentResourceGroup.Should().BeNull();
	}

	[Fact]
	public async Task GetDeviceGroupById()
	{
		var rootDeviceGroup = await LogicMonitorClient
			.GetAsync<ResourceGroup>(1, CancellationToken);
		rootDeviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetRootDeviceGroupByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync("", CancellationToken);
		resourceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetAllDeviceGroups()
	{
		// For LMREP-4060, to test if there are any missing DeviceGroupTypes
		var resourceGroups = await LogicMonitorClient
			.GetAllAsync<ResourceGroup>(CancellationToken);
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
			.GetResourceGroupPropertiesAsync(1, CancellationToken);

		// Make sure that some are returned
		resourceGroupProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceGroupPropertiesByName()
	{
		var resourceGroupProperties = await LogicMonitorClient
			.GetResourceGroupPropertiesByNameAsync(1, "api.account", CancellationToken);
		resourceGroupProperties.Name.Should().NotBe(string.Empty);
	}

	[Fact]
	public async Task GetDeviceGroupsByFullPath()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);

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
			.GetAllAsync(new Filter<ResourceGroup> { Take = 1 }, CancellationToken);

		// Make sure that some are returned
		resourceGroup.Should().ContainSingle();
	}

	[Fact]
	public async Task GetDeviceGroupsByParentId()
	{
		var resourceGroup = await LogicMonitorClient
			.GetAsync<ResourceGroup>(1, CancellationToken);

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
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);

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
				;

		resourceGroup ??= await LogicMonitorClient.CreateAsync(
					  new ResourceGroupCreationDto
					  {
						  ParentId = "1",
						  Name = testResourceGroupName
					  }, CancellationToken);

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
				default);
			var resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
			var actual = resourceGroupProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Set it to a different value
			await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(
				resourceGroup.Id,
				propertyName,
				value2,
				SetPropertyMode.Automatic,
				default);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete it
			await LogicMonitorClient
				.DeleteResourceGroupPropertyAsync(resourceGroup.Id, propertyName, CancellationToken);

			//await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(
			//	deviceGroup.Id,
			//	propertyName,
			//	null,
			//	SetPropertyMode.Automatic,
			//	default);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, null, SetPropertyMode.Delete, cancellationToken: default));
			deletionException.Should().BeOfType<LogicMonitorApiException>();

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, null, SetPropertyMode.Update, cancellationToken: default));
			updateException.Should().BeOfType<InvalidOperationException>();

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, null, SetPropertyMode.Create, cancellationToken: default));
			createNullException.Should().BeOfType<InvalidOperationException>();

			// Create one without checking
			await LogicMonitorClient
				.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, value1, SetPropertyMode.Create, CancellationToken);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Update one without checking
			await LogicMonitorClient
				.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, value2, SetPropertyMode.Update, CancellationToken);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete one without checking
			await LogicMonitorClient
				.SetResourceGroupCustomPropertyAsync(resourceGroup.Id, propertyName, null, SetPropertyMode.Delete, CancellationToken);
			resourceGroupProperties = await LogicMonitorClient
				.GetResourceGroupPropertiesAsync(resourceGroup.Id, CancellationToken);
			actual = resourceGroupProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);
		}
		finally
		{
			await LogicMonitorClient
				.DeleteAsync(resourceGroup, true, CancellationToken);
		}
	}

	[Fact]
	public async Task GetAwsExternalId()
	{
		var awsId = await LogicMonitorClient
			.GetExternalIdAsync(CancellationToken);
		awsId.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceGroupSDTs()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(DeviceGroupFullPath, CancellationToken);
		var sdts = await LogicMonitorClient
			.GetResourceGroupSdtsAsync(resourceGroup.Id, new Filter<ScheduledDownTime>(), CancellationToken);
		sdts.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceGroupAlerts()
	{
		var alerts = await LogicMonitorClient
			.GetResourceGroupAlertsAsync(1)
			;
		alerts.Items.Should().NotBeEmpty();
	}
}
