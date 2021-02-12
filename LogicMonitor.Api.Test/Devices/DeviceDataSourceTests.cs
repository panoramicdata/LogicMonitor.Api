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
		public async void GetAllDeviceDataSourcesAsync()
		{
			var _ = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(WindowsDeviceId, null, CancellationToken.None).ConfigureAwait(false);
		}

		[Fact]
		public async void DeviceDataSourceTests2()
		{
			var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);

			// Get all windows devices in the datacenter
			var devices = await LogicMonitorClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true).ConfigureAwait(false);
			Assert.NotEmpty(devices);
			// We have devices

			// Find datacenter devices with WinCPU datasource
			var deviceDataSources = new List<DeviceDataSource>();
			foreach (var device in devices)
			{
				var deviceDataSourceByDeviceIdAndDataSourceId = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
				if (deviceDataSourceByDeviceIdAndDataSourceId == null)
				{
					continue;
				}
				deviceDataSources.Add(deviceDataSourceByDeviceIdAndDataSourceId);
			}

			Assert.NotEmpty(deviceDataSources);
		}
	}
}