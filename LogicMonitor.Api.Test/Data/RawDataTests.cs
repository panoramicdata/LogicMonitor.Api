using LogicMonitor.Api.Extensions;
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
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinOS").ConfigureAwait(false);
			var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstance =
			(await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false)
			).Single();
			var rawData = await LogicMonitorClient.GetRawDataSetAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id).ConfigureAwait(false);

			Assert.NotNull(rawData);
		}

		[Fact]
		public async void GetRawDataTimeConstrained()
		{
			var utcNow = DateTime.UtcNow;
			var yesterday = utcNow - TimeSpan.FromDays(1);
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinOS").ConfigureAwait(false);
			var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstance =
			(await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false)
			).Single();
			var rawData = await LogicMonitorClient.GetRawDataSetAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id, yesterday, utcNow).ConfigureAwait(false);

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
			var portalClient = LogicMonitorClient;
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await portalClient.GetDataSourceByUniqueNameAsync("WinService-").ConfigureAwait(false);
			var deviceDataSource = await portalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstance =
			(await portalClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false)
			).FirstOrDefault();
			Assert.NotNull(deviceDataSourceInstance);

			var pollNowResponse = await portalClient.PollNowAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstance.Id).ConfigureAwait(false);

			Assert.NotNull(pollNowResponse);
		}

		[Fact]
		public async void FetchInstanceData()
		{
			var device = await GetWindowsDeviceAsync().ConfigureAwait(false);
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinIf-").ConfigureAwait(false);
			var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstances = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id).ConfigureAwait(false);

			var end = DateTime.UtcNow;
			var start = end.AddHours(-2);

			var rawData = await LogicMonitorClient.GetFetchDataResponseAsync(deviceDataSourceInstances.ConvertAll(ddsi => ddsi.Id), start, end).ConfigureAwait(false);

			Assert.NotNull(rawData);
			Assert.Equal(deviceDataSourceInstances.Count, rawData.TotalCount);
			Assert.Equal(deviceDataSourceInstances.Count, rawData.InstanceFetchDataResponses.Count);
			Assert.All(rawData.InstanceFetchDataResponses, response => Assert.Equal(dataSource.DataSourceDataPoints.Count, response.DataPoints.Length));
		}
	}
}