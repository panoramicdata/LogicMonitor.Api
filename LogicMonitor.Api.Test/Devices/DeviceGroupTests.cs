using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Devices;

public class DeviceGroupTests : TestWithOutput
{
	public DeviceGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetDeviceGroupByFullPath()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async void SetAlertThreshold()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath)
			.ConfigureAwait(false);
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping")
			.ConfigureAwait(false);
		var datapoints = (await LogicMonitorClient
			.GetDataSourceDataPointsPageAsync(dataSource.Id, new Filter<DataSourceDataPoint> { Skip = 0, Take = 10 })
			.ConfigureAwait(false)).Items;
		var datapoint = datapoints.Single(dp => dp.Name == "average");
		await LogicMonitorClient
			.SetDeviceGroupDataSourceDataPointThresholds(
				deviceGroup.Id,
				dataSource.Id,
				datapoint.Id,
				"> 191 timezone=Europe/London",
				$"Set by LogicMonitor.Api Unit Test at {DateTimeOffset.UtcNow}",
				true
				)
			.ConfigureAwait(false);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async void GetDeviceGroupByFullPath_InvalidDeviceGroup_ReturnsNull()
	{
		var nonExistentDeviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync("XXXXXX/YYYYYY").ConfigureAwait(false);
		nonExistentDeviceGroup.Should().BeNull();
	}

	[Fact]
	public async void GetDeviceGroupById()
	{
		var rootDeviceGroup = await LogicMonitorClient.GetAsync<DeviceGroup>(1).ConfigureAwait(false);
		rootDeviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async void GetRootDeviceGroupByFullPath()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync("").ConfigureAwait(false);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async void GetAllDeviceGroups()
	{
		// For LMREP-4060, to test if there are any missing DeviceGroupTypes
		var deviceGroups = await LogicMonitorClient.GetAllAsync<DeviceGroup>().ConfigureAwait(false);
		deviceGroups.Should().NotBeNull();
		deviceGroups.Should().NotBeNullOrEmpty();

		// Make sure that all have Unique Ids
		deviceGroups.Select(dg => dg.Id).HasDuplicates().Should().BeFalse();

		// Make sure that all have non-zero Parent Ids
		Assert.DoesNotContain(deviceGroups, dg => dg.Id != 1 && dg.ParentId == 0);
	}

	[Fact]
	public async void GetDeviceGroupProperties()
	{
		var deviceGroupProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(1).ConfigureAwait(false);

		// Make sure that some are returned
		Assert.True(deviceGroupProperties.Count > 0);
	}

	[Fact]
	public async void GetDeviceGroupsByFullPath()
	{
		var deviceGroups = (await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false)).SubGroups;

		// Make sure that some are returned
		Assert.True(deviceGroups.Count > 0);

		// Make sure that all have Unique Ids
		deviceGroups.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async void GetOneDeviceGroup_Succeeds()
	{
		var deviceGroup = await LogicMonitorClient
			.GetAllAsync(new Filter<DeviceGroup> { Take = 1 })
			.ConfigureAwait(false);

		// Make sure that some are returned
		deviceGroup.Should().ContainSingle();
	}

	[Fact]
	public async void GetDeviceGroupsByParentId()
	{
		var deviceGroup = await LogicMonitorClient.GetAsync<DeviceGroup>(1).ConfigureAwait(false);

		// Make sure that some are returned
		Assert.True(deviceGroup.SubGroups.Count > 0);

		// Make sure that all children have Unique Ids
		deviceGroup.SubGroups.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	/// <summary>
	/// Get a Device Group that has a + character in the name (must be escaped)
	/// </summary>
	[Fact]
	public async void GetDeviceGroupByFullPathWithPlusInName()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);

		deviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async void SetDeviceGroupCustomProperty()
	{
		// Create a device group for testing purposes
		const string testDeviceGroupName = "Property Test Device Group";
		var deviceGroup = await LogicMonitorClient.CreateAsync(
			new DeviceGroupCreationDto
			{
				ParentId = "1",
				Name = testDeviceGroupName
			}).ConfigureAwait(false);
		try
		{
			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1).ConfigureAwait(false);
			var deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Set it to a different value
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Set it to null (delete it)
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false)).ConfigureAwait(false);
			deletionException.Should().BeOfType<LogicMonitorApiException>();

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Update).ConfigureAwait(false)).ConfigureAwait(false);
			updateException.Should().BeOfType<InvalidOperationException>();

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Create).ConfigureAwait(false)).ConfigureAwait(false);
			createNullException.Should().BeOfType<InvalidOperationException>();

			// Create one without checking
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1, SetPropertyMode.Create).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Update one without checking
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2, SetPropertyMode.Update).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete one without checking
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);
		}
		finally
		{
			await LogicMonitorClient
				.DeleteAsync(deviceGroup)
				.ConfigureAwait(false);
		}
	}
}
