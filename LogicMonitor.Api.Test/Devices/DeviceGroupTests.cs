using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Devices;

public class DeviceGroupTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetDeviceGroupByFullPath()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetDevicesByDeviceGroupByFullPathWhereNameContainsParentheses()
	{
		//// No recurse
		//var nonrecurse = await LogicMonitorClient.GetDevicesByDeviceGroupFullPathAsync("DSW - (Test) Denise Home Network", false, default).ConfigureAwait(true);

		//// Recurse
		//var recursed = await LogicMonitorClient.GetDevicesByDeviceGroupFullPathAsync("DSW - (Test) Denise Home Network", true, default).ConfigureAwait(true);

		//nonrecurse.Should().HaveCount(3);
		//recursed.Should().HaveCount(6);

		// No recurse
		var nonrecurse = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync("DSW - (Test) Denise Home Network/Sub Group 2 (XYZ)", false, default)
			.ConfigureAwait(true);

		// Recurse
		var recursed = await LogicMonitorClient
			.GetDevicesByDeviceGroupFullPathAsync("DSW - (Test) Denise Home Network/Sub Group 2 (XYZ)", true, default)
			.ConfigureAwait(true);

		nonrecurse.Should().ContainSingle();
		recursed.Should().HaveCount(2);
	}

	[Fact]
	public async Task SetAlertThreshold()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default)
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
			.SetDeviceGroupDataSourceDataPointThresholdsAsync(
				deviceGroup.Id,
				dataSource.Id,
				datapoint.Id,
				"> 191 timezone=Europe/London",
				$"Set by LogicMonitor.Api Unit Test at {DateTimeOffset.UtcNow}",
				true,
				default)
			.ConfigureAwait(true);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetDeviceGroupByFullPath_InvalidDeviceGroup_ReturnsNull()
	{
		var nonExistentDeviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync("XXXXXX/YYYYYY", default)
			.ConfigureAwait(true);
		nonExistentDeviceGroup.Should().BeNull();
	}

	[Fact]
	public async Task GetDeviceGroupById()
	{
		var rootDeviceGroup = await LogicMonitorClient
			.GetAsync<DeviceGroup>(1, default)
			.ConfigureAwait(true);
		rootDeviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetRootDeviceGroupByFullPath()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync("", default)
			.ConfigureAwait(true);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetAllDeviceGroups()
	{
		// For LMREP-4060, to test if there are any missing DeviceGroupTypes
		var deviceGroups = await LogicMonitorClient
			.GetAllAsync<DeviceGroup>(default)
			.ConfigureAwait(true);
		deviceGroups.Should().NotBeNull();
		deviceGroups.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		deviceGroups.Select(dg => dg.Id).HasDuplicates().Should().BeFalse();

		// Make sure that all have non-zero Parent Ids
		deviceGroups.Should().AllSatisfy(dg => (dg.Id == 1 || dg.ParentId != 0).Should().BeTrue());
	}

	[Fact]
	public async Task GetDeviceGroupProperties()
	{
		var deviceGroupProperties = await LogicMonitorClient
			.GetDeviceGroupPropertiesAsync(1, default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		deviceGroupProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceGroupPropertiesByName()
	{
		var deviceGroupProperties = await LogicMonitorClient
			.GetDeviceGroupPropertiesByNameAsync(1, "api.account", default)
			.ConfigureAwait(true);
		deviceGroupProperties.Name.Should().NotBe(string.Empty);
	}

	[Fact]
	public async Task GetDeviceGroupsByFullPath()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);

		var deviceGroups = deviceGroup.SubGroups;

		// Make sure that some are returned
		deviceGroups.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		deviceGroups.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetOneDeviceGroup_Succeeds()
	{
		var deviceGroup = await LogicMonitorClient
			.GetAllAsync(new Filter<DeviceGroup> { Take = 1 }, default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		deviceGroup.Should().ContainSingle();
	}

	[Fact]
	public async Task GetDeviceGroupsByParentId()
	{
		var deviceGroup = await LogicMonitorClient
			.GetAsync<DeviceGroup>(1, default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		deviceGroup.SubGroups.Should().NotBeNullOrEmpty();

		// Make sure that all children have Unique Ids
		deviceGroup.SubGroups.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	/// <summary>
	/// Get a Device Group that has a + character in the name (must be escaped)
	/// </summary>
	[Fact]
	public async Task GetDeviceGroupByFullPathWithPlusInName()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);

		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task SetDeviceGroupCustomProperty()
	{
		// Make sure a device group exists for testing purposes
		const string testDeviceGroupName = "Property Test Device Group";
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(
				testDeviceGroupName,
				default)
				.ConfigureAwait(true);

		deviceGroup ??= await LogicMonitorClient.CreateAsync(
					  new DeviceGroupCreationDto
					  {
						  ParentId = "1",
						  Name = testDeviceGroupName
					  }, default)
				.ConfigureAwait(true);

		try
		{
			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(
				deviceGroup.Id,
				propertyName,
				value1,
				SetPropertyMode.Automatic,
				default).ConfigureAwait(true);
			var deviceProperties = await LogicMonitorClient
				.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default)
				.ConfigureAwait(true);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Set it to a different value
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(
				deviceGroup.Id,
				propertyName,
				value2,
				SetPropertyMode.Automatic,
				default).ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete it
			await LogicMonitorClient
				.DeleteDeviceGroupPropertyAsync(deviceGroup.Id, propertyName, default)
				.ConfigureAwait(true);

			//await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(
			//	deviceGroup.Id,
			//	propertyName,
			//	null,
			//	SetPropertyMode.Automatic,
			//	default).ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete, cancellationToken: default).ConfigureAwait(true)).ConfigureAwait(true);
			deletionException.Should().BeOfType<LogicMonitorApiException>();

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Update, cancellationToken: default).ConfigureAwait(true)).ConfigureAwait(true);
			updateException.Should().BeOfType<InvalidOperationException>();

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Create, cancellationToken: default).ConfigureAwait(true)).ConfigureAwait(true);
			createNullException.Should().BeOfType<InvalidOperationException>();

			// Create one without checking
			await LogicMonitorClient
				.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1, SetPropertyMode.Create, default)
				.ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Update one without checking
			await LogicMonitorClient
				.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2, SetPropertyMode.Update, default)
				.ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete one without checking
			await LogicMonitorClient
				.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete, default)
				.ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);
		}
		finally
		{
			await LogicMonitorClient
				.DeleteAsync(deviceGroup, true, default)
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
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(true);
		var sdts = await LogicMonitorClient
			.GetDeviceGroupSdtsAsync(deviceGroup.Id, new Filter<ScheduledDownTime>(), default)
			.ConfigureAwait(true);
		sdts.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDeviceGroupAlerts()
	{
		var alerts = await LogicMonitorClient
			.GetDeviceGroupAlertsAsync(1)
			.ConfigureAwait(true);
		alerts.Items.Should().NotBeEmpty();
	}
}
