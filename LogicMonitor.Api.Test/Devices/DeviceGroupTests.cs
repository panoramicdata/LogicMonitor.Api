using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Devices;

public class DeviceGroupTests : TestWithOutput
{
	public DeviceGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetDeviceGroupByFullPath()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default).ConfigureAwait(false);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task SetAlertThreshold()
	{
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default)
			.ConfigureAwait(false);
		var dataSource = await LogicMonitorClient
			.GetByNameAsync<DataSource>("Ping", default)
			.ConfigureAwait(false);
		dataSource ??= new();
		var datapoints = (await LogicMonitorClient
			.GetDataSourceDataPointsPageAsync(dataSource.Id, new Filter<DataPoint> { Skip = 0, Take = 10 }, default)
			.ConfigureAwait(false)).Items;
		var datapoint = datapoints.Single(dp => dp.Name == "average");
		await LogicMonitorClient
			.SetDeviceGroupDataSourceDataPointThresholds(
				deviceGroup.Id,
				dataSource.Id,
				datapoint.Id,
				"> 191 timezone=Europe/London",
				$"Set by LogicMonitor.Api Unit Test at {DateTimeOffset.UtcNow}",
				true,
				default)
			.ConfigureAwait(false);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetDeviceGroupByFullPath_InvalidDeviceGroup_ReturnsNull()
	{
		var nonExistentDeviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync("XXXXXX/YYYYYY", default).ConfigureAwait(false);
		nonExistentDeviceGroup.Should().BeNull();
	}

	[Fact]
	public async Task GetDeviceGroupById()
	{
		var rootDeviceGroup = await LogicMonitorClient
			.GetAsync<DeviceGroup>(1, default)
			.ConfigureAwait(false);
		rootDeviceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetRootDeviceGroupByFullPath()
	{
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync("", default).ConfigureAwait(false);
		deviceGroup.AlertStatus.Should().NotBe(AlertStatus.Unknown);
	}

	[Fact]
	public async Task GetAllDeviceGroups()
	{
		// For LMREP-4060, to test if there are any missing DeviceGroupTypes
		var deviceGroups = await LogicMonitorClient.GetAllAsync<DeviceGroup>(default).ConfigureAwait(false);
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
		var deviceGroupProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(1, default).ConfigureAwait(false);

		// Make sure that some are returned
		deviceGroupProperties.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceGroupsByFullPath()
	{
		var deviceGroups = (await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default).ConfigureAwait(false)).SubGroups;

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
			.ConfigureAwait(false);

		// Make sure that some are returned
		deviceGroup.Should().ContainSingle();
	}

	[Fact]
	public async Task GetDeviceGroupsByParentId()
	{
		var deviceGroup = await LogicMonitorClient.GetAsync<DeviceGroup>(1, default).ConfigureAwait(false);

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
		var deviceGroup = await LogicMonitorClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath, default).ConfigureAwait(false);

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
				.ConfigureAwait(false);

		deviceGroup ??= await LogicMonitorClient.CreateAsync(
					  new DeviceGroupCreationDto
					  {
						  ParentId = "1",
						  Name = testDeviceGroupName
					  }, default)
				.ConfigureAwait(false);

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
				default).ConfigureAwait(false);
			var deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default).ConfigureAwait(false);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Set it to a different value
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(
				deviceGroup.Id,
				propertyName,
				value2,
				SetPropertyMode.Automatic,
				default).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Set it to null (delete it)
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(
				deviceGroup.Id,
				propertyName,
				null,
				SetPropertyMode.Automatic,
				default).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete, cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);
			deletionException.Should().BeOfType<LogicMonitorApiException>();

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Update, cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);
			updateException.Should().BeOfType<InvalidOperationException>();

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Create, cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);
			createNullException.Should().BeOfType<InvalidOperationException>();

			// Create one without checking
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1, SetPropertyMode.Create, default).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Update one without checking
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2, SetPropertyMode.Update, default).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete one without checking
			await LogicMonitorClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete, default).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id, default).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);
		}
		finally
		{
			await LogicMonitorClient
				.DeleteAsync(deviceGroup, true, default)
				.ConfigureAwait(false);
		}
	}
}
