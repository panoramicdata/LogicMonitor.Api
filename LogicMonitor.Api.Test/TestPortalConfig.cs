using System.Configuration;
using System.Globalization;

namespace LogicMonitor.Api.Test;

internal sealed class TestPortalConfig
{
	internal TestPortalConfig(ILogger logger)
	{
		var location = typeof(TestPortalConfig).GetTypeInfo().Assembly.Location;
		var dirPath = Path.Combine(Path.GetDirectoryName(location) ?? "", "../../../..");
		var builder = new ConfigurationBuilder()
			.SetBasePath(dirPath)
			.AddUserSecrets<TestPortalConfig>();
		Configuration = builder.Build();

		var timeoutSetting = Configuration["Config:HttpClientTimeoutSeconds"] ?? "120";
		if (!int.TryParse(timeoutSetting, out var clientTimeoutSeconds))
		{
			throw new ConfigurationErrorsException("Configuration setting HttpClientTimeoutSeconds is not an integer!");
		}

		LogicMonitorClient = new LogicMonitorClient(
			new LogicMonitorClientOptions
			{
				Account = Configuration["Config:Account"] ?? "",
				AccessId = Configuration["Config:AccessId"] ?? "",
				AccessKey = Configuration["Config:AccessKey"] ?? "",
				HttpClientTimeoutSeconds = clientTimeoutSeconds,
				Logger = logger
			}
		)
		{
			StrictPagingTotalChecking = true
		};
		ExperimentalLogicMonitorClient = new Api.Experimental.LogicMonitorClient(
			new LogicMonitorClientOptions
			{
				Account = Configuration["Config:Account"] ?? "",
				AccessId = Configuration["Config:AccessId"] ?? "",
				AccessKey = Configuration["Config:AccessKey"] ?? "",
				HttpClientTimeoutSeconds = clientTimeoutSeconds,
				Logger = logger
			}
		);
		SnmpDeviceId = int.Parse(Configuration["Config:SnmpDeviceId"] ?? "0", CultureInfo.InvariantCulture);
		NetflowDeviceId = int.Parse(Configuration["Config:NetflowDeviceId"] ?? "0", CultureInfo.InvariantCulture);
		WindowsDeviceId = int.Parse(Configuration["Config:WindowsDeviceId"] ?? "0", CultureInfo.InvariantCulture);
		WindowsDeviceLargeDeviceDataSourceId = int.Parse(Configuration["Config:WindowsDeviceLargeDeviceDataSourceId"] ?? "0", CultureInfo.InvariantCulture);
		ServiceDeviceId = int.Parse(Configuration["Config:ServiceDeviceId"] ?? "0", CultureInfo.InvariantCulture);
		CollectorId = int.Parse(Configuration["Config:CollectorId"] ?? "0", CultureInfo.InvariantCulture);
		DownCollectorId = int.Parse(Configuration["Config:DownCollectorId"] ?? "0", CultureInfo.InvariantCulture);
		SdtResourceGroupId = int.Parse(Configuration["Config:SDTResourceGroupId"] ?? "0", CultureInfo.InvariantCulture);
		ReportId = int.Parse(Configuration["Config:ReportId"] ?? "0", CultureInfo.InvariantCulture);
		TestDashboardId = int.Parse(Configuration["Config:TestDashboardId"] ?? "0", CultureInfo.InvariantCulture);
		WebsiteGroupFullPath = Configuration["Config:WebsiteGroupFullPath"] ?? "";
		DeviceGroupFullPath = Configuration["Config:DeviceGroupFullPath"] ?? "";
		ResourceGroupFullPath = Configuration["Config:ResourceGroupFullPath"] ?? "";
		AllWidgetsDashboardId = int.Parse(Configuration["Config:AllWidgetsDashboardId"] ?? "0", CultureInfo.InvariantCulture);
		AccountHasBillingInformation = bool.Parse(Configuration["Config:AccountHasBillingInformation"] ?? "false");
		WebsiteName = Configuration["Config:WebsiteName"] ?? "";
		AlertRuleName = Configuration["Config:AlertRuleName"] ?? "";
	}

	public static IConfigurationRoot? Configuration { get; set; }

	public LogicMonitorClient LogicMonitorClient { get; }

	public Api.Experimental.LogicMonitorClient ExperimentalLogicMonitorClient { get; }

	public int NetflowDeviceId { get; }

	public int SnmpDeviceId { get; }

	public int ReportId { get; }

	public int WindowsDeviceId { get; }

	public int WindowsDeviceLargeDeviceDataSourceId { get; }

	public int ServiceDeviceId { get; }

	public bool AccountHasBillingInformation { get; }

	public int AllWidgetsDashboardId { get; }

	public string WebsiteGroupFullPath { get; }

	public string DeviceGroupFullPath { get; }

	public string ResourceGroupFullPath { get; }

	public string WebsiteName { get; }

	public int DownCollectorId { get; }

	public int CollectorId { get; }

	public int SdtResourceGroupId { get; }

	public string AlertRuleName { get; }

	public int TestDashboardId { get; }
}
