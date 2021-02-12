using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Devices
{
	public class DeviceDataSourceInstanceTests : TestWithOutput
	{
		public DeviceDataSourceInstanceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetAllDeviceDataSourceInstancesAsync()
		{
			var _ = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId).ConfigureAwait(false);
		}

		[Fact]
		public async void GetAllDeviceDataSourceInstancesForOneDeviceDataSourceAsync()
		{
			var dataSource = await LogicMonitorClient
				.GetDataSourceByUniqueNameAsync("WinIf-")
				.ConfigureAwait(false);
			var deviceDataSource = await LogicMonitorClient
				.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id)
				.ConfigureAwait(false);

			var _ = await LogicMonitorClient
				.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id)
				.ConfigureAwait(false);
		}

		[Fact]
		public async void OnlyMonitoredInstances()
		{
			var device = await GetSnmpDeviceAsync()
				.ConfigureAwait(false);
			var dataSource = await LogicMonitorClient
				.GetDataSourceByUniqueNameAsync("snmp64_If-")
				.ConfigureAwait(false);
			var deviceDataSource = await LogicMonitorClient
				.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id)
				.ConfigureAwait(false);
			var deviceDataSourceInstances = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>
			{
				Order = new Order<DeviceDataSourceInstance> { Direction = OrderDirection.Asc, Property = nameof(DeviceDataSourceInstance.DisplayName) },
				ExtraFilters = new List<FilterItem<DeviceDataSourceInstance>>
				{
					new Eq<DeviceDataSourceInstance>(nameof(DeviceDataSourceInstance.StopMonitoring), false)
				}
			}).ConfigureAwait(false);

			// BROKEN - extraFilters appears not to work!
			Assert.All(deviceDataSourceInstances, dsi => Assert.False(dsi.StopMonitoring));
		}
	}
}