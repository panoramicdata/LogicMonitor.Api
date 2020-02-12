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
			var _ = await PortalClient.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId).ConfigureAwait(false);
		}

		[Fact]
		public async void GetAllDeviceDataSourceInstancesForOneDeviceDataSourceAsync()
		{
			var _ = await PortalClient.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, WindowsDeviceLargeDeviceDataSourceId).ConfigureAwait(false);
		}

		[Fact]
		public async void OnlyMonitoredInstances()
		{
			var device = await GetSnmpDeviceAsync().ConfigureAwait(false);
			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("snmp64_If-").ConfigureAwait(false);
			var deviceDataSource = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			var deviceDataSourceInstances = await PortalClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>
			{
				Order = new Order<DeviceDataSourceInstance> { Direction = OrderDirection.Asc, Property = nameof(DeviceDataSourceInstance.DisplayName) },
				ExtraFilters = new List<FilterItem<DeviceDataSourceInstance>>
				{
					new Eq<DeviceDataSourceInstance>(nameof(DeviceDataSourceInstance.StopMonitoring), false)
				}
			}).ConfigureAwait(false);
			Assert.All(deviceDataSourceInstances, dsi => Assert.False(dsi.StopMonitoring));
		}
	}
}