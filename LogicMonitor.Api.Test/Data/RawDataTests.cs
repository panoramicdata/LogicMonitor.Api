using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Data
{
	public class RawDataTests : TestWithOutput
	{
		public RawDataTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetRawData()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("WinOS").ConfigureAwait(false);
			var deviceDataSource = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstance =
			(await PortalClient.GetDeviceDataSourceInstancesPageAsync(device.Id, deviceDataSource.Id,
				new Filter<DeviceDataSourceInstance> { Skip = 0, Take = 300 }).ConfigureAwait(false)
			).Items.Single();
			var rawData = await PortalClient.GetRawDataSetAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id).ConfigureAwait(false);

			Assert.NotNull(rawData);
		}

		[Fact]
		public async void GetRawDataTimeConstrained()
		{
			var utcNow = DateTime.UtcNow;
			var yesterday = utcNow - TimeSpan.FromDays(1);
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("WinOS").ConfigureAwait(false);
			var deviceDataSource = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstance =
			(await PortalClient.GetDeviceDataSourceInstancesPageAsync(device.Id, deviceDataSource.Id,
				new Filter<DeviceDataSourceInstance> { Skip = 0, Take = 300 }).ConfigureAwait(false)
			).Items.Single();
			var rawData = await PortalClient.GetRawDataSetAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id, yesterday, utcNow).ConfigureAwait(false);

			Assert.NotNull(rawData);

			Assert.All(rawData.UtcTimeStamps, r =>
			{
				var dataDateTime = r.ToDateTimeUtcFromMs();
				Assert.True(yesterday <= dataDateTime && dataDateTime <= utcNow);
			});
		}

		[Fact]
		public async void PollNow()
		{
			var portalClient = PortalClient;
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await portalClient.GetDataSourceByUniqueNameAsync("WinService-").ConfigureAwait(false);
			var deviceDataSource = await portalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstance =
			(await portalClient.GetDeviceDataSourceInstancesPageAsync(device.Id, deviceDataSource.Id,
				new Filter<DeviceDataSourceInstance> { Skip = 0, Take = 300 }).ConfigureAwait(false)
			).Items.FirstOrDefault();
			Assert.NotNull(deviceDataSourceInstance);

			var pollNowResponse = await portalClient.PollNowAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id).ConfigureAwait(false);

			Assert.NotNull(pollNowResponse);
		}
	}
}