using LogicMonitor.Api.Dashboards;
using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test
{
	public abstract class TestWithOutput
	{
		protected ILogger Logger { get; }

		protected string WebsiteGroupFullPath { get; }

		protected string DeviceGroupFullPath { get; }

		protected int ServiceDeviceId { get; }

		protected int WindowsDeviceId { get; }

		protected int WindowsDeviceLargeDeviceDataSourceId { get; }

		protected int NetflowDeviceId { get; }

		protected int SnmpDeviceId { get; }

		protected int AllWidgetsDashboardId { get; }

		protected bool AccountHasBillingInformation { get; }

		protected TestWithOutput(ITestOutputHelper iTestOutputHelper)
		{
			Logger = iTestOutputHelper.BuildLogger();

			var testPortalConfig = new TestPortalConfig(Logger);
			PortalClient = testPortalConfig.PortalClient;
			WindowsDeviceId = testPortalConfig.WindowsDeviceId;
			WindowsDeviceLargeDeviceDataSourceId = testPortalConfig.WindowsDeviceLargeDeviceDataSourceId;
			ServiceDeviceId = testPortalConfig.ServiceDeviceId;
			NetflowDeviceId = testPortalConfig.NetflowDeviceId;
			DeviceGroupFullPath = testPortalConfig.DeviceGroupFullPath;
			WebsiteGroupFullPath = testPortalConfig.WebsiteGroupFullPath;
			SnmpDeviceId = testPortalConfig.SnmpDeviceId;
			AllWidgetsDashboardId = testPortalConfig.AllWidgetsDashboardId;
			AccountHasBillingInformation = testPortalConfig.AccountHasBillingInformation;
			var nowUtc = DateTime.UtcNow;
			StartEpoch = nowUtc.AddDays(-30).SecondsSinceTheEpoch();
			EndEpoch = nowUtc.SecondsSinceTheEpoch();
			Stopwatch = Stopwatch.StartNew();
		}

		private Stopwatch Stopwatch { get; }

		protected int StartEpoch { get; }
		protected int EndEpoch { get; }
		protected PortalClient PortalClient { get; }

		protected void AssertIsFast(int durationSeconds)
			=> Assert.InRange(Stopwatch.ElapsedMilliseconds, 0, durationSeconds * 1000);

		protected long DaysAgoAsUnixSeconds(int days)
			=> DateTimeOffset.UtcNow.AddDays(-days).ToUnixTimeSeconds();

		internal Task<Device> GetNetflowDeviceAsync()
			=> PortalClient.GetAsync<Device>(NetflowDeviceId);

		protected Task<Device> GetServiceDeviceAsync()
			=> PortalClient.GetAsync<Device>(ServiceDeviceId);

		protected Task<Device> GetWindowsDeviceAsync()
			=> PortalClient.GetAsync<Device>(WindowsDeviceId);

		protected Task<Device> GetSnmpDeviceAsync()
			=> PortalClient.GetAsync<Device>(SnmpDeviceId);

		protected Task<Dashboard> GetAllWidgetsDashboardAsync()
			=> PortalClient.GetAsync<Dashboard>(AllWidgetsDashboardId);

		protected string GetWebsiteGroupFullPath()
			=> WebsiteGroupFullPath;
	}
}