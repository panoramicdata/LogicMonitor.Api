using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace LogicMonitor.Api.Test
{
	internal class TestPortalConfig
	{
		internal TestPortalConfig(ILogger logger)
		{
			var location = typeof(TestPortalConfig).GetTypeInfo().Assembly.Location;
			var dirPath = Path.Combine(Path.GetDirectoryName(location), "../../..");
			var builder = new ConfigurationBuilder()
				.SetBasePath(dirPath)
				.AddJsonFile("appsettings.json");
			Configuration = builder.Build();

			PortalClient = new PortalClient(Configuration["Config:Account"], Configuration["Config:AccessId"], Configuration["Config:AccessKey"], logger);
			SnmpDeviceId = int.Parse(Configuration["Config:SnmpDeviceId"]);
			NetflowDeviceId = int.Parse(Configuration["Config:NetflowDeviceId"]);
			WindowsDeviceId = int.Parse(Configuration["Config:WindowsDeviceId"]);
			CollectorId = int.Parse(Configuration["Config:CollectorId"]);
			WebsiteGroupFullPath = Configuration["Config:WebsiteGroupFullPath"];
			DeviceGroupFullPath = Configuration["Config:DeviceGroupFullPath"];
			AllWidgetsDashboardId = int.Parse(Configuration["Config:AllWidgetsDashboardId"]);
			AccountHasBillingInformation = bool.Parse(Configuration["Config:AccountHasBillingInformation"]);
		}

		public static IConfigurationRoot Configuration { get; set; }

		public PortalClient PortalClient { get; }

		public int NetflowDeviceId { get; }

		public int SnmpDeviceId { get; }

		public int WindowsDeviceId { get; }

		public bool AccountHasBillingInformation { get; }

		public int AllWidgetsDashboardId { get; }

		public string WebsiteGroupFullPath { get; }

		public string DeviceGroupFullPath { get; }

		internal int CollectorId { get; }
	}
}