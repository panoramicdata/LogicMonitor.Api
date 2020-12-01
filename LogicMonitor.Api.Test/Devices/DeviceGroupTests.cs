using FluentAssertions;
using LogicMonitor.Api.Alerts;
using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Devices
{
	public class DeviceGroupTests : TestWithOutput
	{
		public DeviceGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetDeviceGroupByFullPath()
		{
			var deviceGroup = await PortalClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);
			Assert.NotEqual(AlertStatus.Unknown, deviceGroup.AlertStatus);
		}

		[Fact]
		public async void SetAlertThreshold()
		{
			var deviceGroup = await PortalClient
				.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath)
				.ConfigureAwait(false);
			var dataSource = await PortalClient
				.GetByNameAsync<DataSource>("Ping")
				.ConfigureAwait(false);
			var datapoints = (await PortalClient
				.GetDataSourceDataPointsPageAsync(dataSource.Id, new Filter<DataSourceDataPoint> { Skip = 0, Take = 10 })
				.ConfigureAwait(false)).Items;
			var datapoint = datapoints.Single(dp => dp.Name == "average");
			await PortalClient
				.SetDeviceGroupDataSourceDataPointThresholds(
					deviceGroup.Id,
					dataSource.Id,
					datapoint.Id,
					"> 191 timezone=Europe/London",
					$"Set by LogicMonitor.Api Unit Test at {DateTimeOffset.UtcNow}",
					true
					)
				.ConfigureAwait(false);
			Assert.NotEqual(AlertStatus.Unknown, deviceGroup.AlertStatus);
		}

		[Fact]
		public async void GetDeviceGroupByFullPath_InvalidDeviceGroup_ReturnsNull()
		{
			var nonExistentDeviceGroup = await PortalClient.GetDeviceGroupByFullPathAsync("XXXXXX/YYYYYY").ConfigureAwait(false);
			Assert.Null(nonExistentDeviceGroup);
		}

		[Fact]
		public async void GetDeviceGroupById()
		{
			var rootDeviceGroup = await PortalClient.GetAsync<DeviceGroup>(1).ConfigureAwait(false);
			Assert.NotNull(rootDeviceGroup);
		}

		[Fact]
		public async void GetRootDeviceGroupByFullPath()
		{
			var deviceGroup = await PortalClient.GetDeviceGroupByFullPathAsync("").ConfigureAwait(false);
			Assert.NotEqual(AlertStatus.Unknown, deviceGroup.AlertStatus);
		}

		[Fact]
		public async void GetAllDeviceGroups()
		{
			// For LMREP-4060, to test if there are any missing DeviceGroupTypes
			var deviceGroups = await PortalClient.GetAllAsync<DeviceGroup>().ConfigureAwait(false);
			Assert.NotNull(deviceGroups);
			Assert.NotEmpty(deviceGroups);

			// Make sure that all have Unique Ids
			Assert.False(deviceGroups.Select(dg => dg.Id).HasDuplicates());

			// Make sure that all have non-zero Parent Ids
			Assert.DoesNotContain(deviceGroups, dg => dg.Id != 1 && dg.ParentId == 0);
		}

		[Fact]
		public async void GetDeviceGroupProperties()
		{
			var deviceGroupProperties = await PortalClient.GetDeviceGroupPropertiesAsync(1).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.True(deviceGroupProperties.Count > 0);
		}

		[Fact]
		public async void GetDeviceGroupsByFullPath()
		{
			var deviceGroups = (await PortalClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false)).SubGroups;

			// Make sure that some are returned
			Assert.True(deviceGroups.Count > 0);

			// Make sure that all have Unique Ids
			Assert.False(deviceGroups.Select(c => c.Id).HasDuplicates());
		}

		[Fact]
		public async void GetOneDeviceGroup_Succeeds()
		{
			var deviceGroup = await PortalClient
				.GetAllAsync(new Filter<DeviceGroup> { Take = 1 })
				.ConfigureAwait(false);

			// Make sure that some are returned
			deviceGroup.Count.Should().Be(1);
		}

		[Fact]
		public async void GetDeviceGroupsByParentId()
		{
			var deviceGroup = await PortalClient.GetAsync<DeviceGroup>(1).ConfigureAwait(false);

			// Make sure that some are returned
			Assert.True(deviceGroup.SubGroups.Count > 0);

			// Make sure that all children have Unique Ids
			Assert.False(deviceGroup.SubGroups.Select(c => c.Id).HasDuplicates());
		}

		/// <summary>
		/// Get a Device Group that has a + character in the name (must be escaped)
		/// </summary>
		[Fact]
		public async void GetDeviceGroupByFullPathWithPlusInName()
		{
			var deviceGroup = await PortalClient.GetDeviceGroupByFullPathAsync(DeviceGroupFullPath).ConfigureAwait(false);

			Assert.NotNull(deviceGroup);
		}

		[Fact]
		public async void SetDeviceGroupCustomProperty()
		{
			// Create a device group for testing purposes
			const string testDeviceGroupName = "Property Test Device Group";
			var existingDeviceGroup = await PortalClient
				.GetDeviceGroupByFullPathAsync(testDeviceGroupName)
				.ConfigureAwait(false);
			if (existingDeviceGroup != null)
			{
				await PortalClient
					.DeleteAsync(existingDeviceGroup)
					.ConfigureAwait(false);
			}
			var deviceGroup = await PortalClient.CreateAsync(new DeviceGroupCreationDto
			{
				ParentId = "1",
				Name = testDeviceGroupName
			}).ConfigureAwait(false);

			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1).ConfigureAwait(false);
			var deviceProperties = await PortalClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Set it to a different value
			await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Set it to null (delete it)
			await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<LogicMonitorApiException>(deletionException);

			var updateException = await Record.ExceptionAsync(async () => await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Update).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(updateException);

			var createNullException = await Record.ExceptionAsync(async () => await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Create).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(createNullException);

			// Create one without checking
			await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1, SetPropertyMode.Create).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Update one without checking
			await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2, SetPropertyMode.Update).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Delete one without checking
			await PortalClient.SetDeviceGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetDeviceGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);
		}
	}
}