using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Devices
{
	public class DeviceDataSourceTests : TestWithOutput
	{
		public DeviceDataSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetAllDeviceDatraSourcesAsync()
		{
			var _ = await PortalClient.GetAllDeviceDataSourcesAsync(WindowsDeviceId, null, CancellationToken.None).ConfigureAwait(false);
		}

		[Fact]
		public async void DeviceDataSourceTests2()
		{
			var dataSource = await PortalClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);

			// Get all windows devices in the datacenter
			var devices = await PortalClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true).ConfigureAwait(false);
			Assert.NotEmpty(devices);
			// We have devices

			// Find datacenter devices with WinCPU datasource
			var deviceDataSources = new List<DeviceDataSource>();
			foreach (var device in devices)
			{
				var deviceDataSourceByDeviceIdAndDataSourceId = await PortalClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
				if (deviceDataSourceByDeviceIdAndDataSourceId == null)
				{
					continue;
				}
				deviceDataSources.Add(deviceDataSourceByDeviceIdAndDataSourceId);
			}

			Assert.NotEmpty(deviceDataSources);
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