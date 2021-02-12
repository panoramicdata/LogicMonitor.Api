using LogicMonitor.Api.Websites;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Websites
{
	public class WebsiteGroupTests : TestWithOutput
	{
		public WebsiteGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void SetWebsiteGroupCustomProperty()
		{
			// Create a device group for testing purposes
			const string testWebsiteGroupName = "Property Test Device Group";
			var existingWebsiteGroup = await LogicMonitorClient
				.GetWebsiteGroupByFullPathAsync(testWebsiteGroupName)
				.ConfigureAwait(false);
			if (existingWebsiteGroup != null)
			{
				await LogicMonitorClient
					.DeleteAsync(existingWebsiteGroup)
					.ConfigureAwait(false);
			}
			var deviceGroup = await LogicMonitorClient.CreateAsync(new WebsiteGroupCreationDto
			{
				ParentId = "1",
				Name = testWebsiteGroupName
			}).ConfigureAwait(false);

			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1).ConfigureAwait(false);
			var deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Set it to a different value
			await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Set it to null (delete it)
			await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<LogicMonitorApiException>(deletionException);

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Update).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(updateException);

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Create).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(createNullException);

			// Create one without checking
			await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1, SetPropertyMode.Create).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Update one without checking
			await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2, SetPropertyMode.Update).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Delete one without checking
			await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);
		}
	}
}