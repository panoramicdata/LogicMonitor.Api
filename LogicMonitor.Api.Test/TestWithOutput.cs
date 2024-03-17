namespace LogicMonitor.Api.Test;

public abstract class TestWithOutput
{
	protected ILogger Logger { get; }

	protected int CollectorId { get; }

	protected string WebsiteGroupFullPath { get; }

	protected string AlertRuleName { get; }

	protected string WebsiteName { get; }

	protected string DeviceGroupFullPath { get; }

	protected string ResourceGroupFullPath { get; }

	protected int ServiceDeviceId { get; }

	protected int WindowsDeviceId { get; }

	protected int ReportId { get; }

	protected int SdtResourceGroupId { get; }

	protected int WindowsDeviceLargeDeviceDataSourceId { get; }

	protected int NetflowDeviceId { get; }

	protected int SnmpDeviceId { get; }

	protected int AllWidgetsDashboardId { get; }

	protected bool AccountHasBillingInformation { get; }
	protected int TestDashboardId { get; }

	protected TestWithOutput(ITestOutputHelper iTestOutputHelper)
	{
		Logger = iTestOutputHelper.BuildLogger();

		var testPortalConfig = new TestPortalConfig(Logger);
		ExperimentalLogicMonitorClient = testPortalConfig.ExperimentalLogicMonitorClient;
		LogicMonitorClient = testPortalConfig.LogicMonitorClient;
		CollectorId = testPortalConfig.CollectorId;
		WindowsDeviceId = testPortalConfig.WindowsDeviceId;
		WindowsDeviceLargeDeviceDataSourceId = testPortalConfig.WindowsDeviceLargeDeviceDataSourceId;
		ServiceDeviceId = testPortalConfig.ServiceDeviceId;
		NetflowDeviceId = testPortalConfig.NetflowDeviceId;
		SdtResourceGroupId = testPortalConfig.SdtResourceGroupId;
		TestDashboardId = testPortalConfig.TestDashboardId;
		DeviceGroupFullPath = testPortalConfig.DeviceGroupFullPath;
		WebsiteGroupFullPath = testPortalConfig.WebsiteGroupFullPath;
		ResourceGroupFullPath = testPortalConfig.ResourceGroupFullPath;
		ReportId = testPortalConfig.ReportId;
		WebsiteName = testPortalConfig.WebsiteName;
		SnmpDeviceId = testPortalConfig.SnmpDeviceId;
		AllWidgetsDashboardId = testPortalConfig.AllWidgetsDashboardId;
		AccountHasBillingInformation = testPortalConfig.AccountHasBillingInformation;
		AlertRuleName = testPortalConfig.AlertRuleName;
		var nowUtc = DateTime.UtcNow;
		StartEpoch = nowUtc.AddDays(-30).SecondsSinceTheEpoch();
		EndEpoch = nowUtc.SecondsSinceTheEpoch();
		Stopwatch = Stopwatch.StartNew();
	}

	private Stopwatch Stopwatch { get; }

	protected int StartEpoch { get; }

	protected int EndEpoch { get; }
	internal Api.Experimental.LogicMonitorClient ExperimentalLogicMonitorClient { get; }
	protected LogicMonitorClient LogicMonitorClient { get; }

	protected void AssertIsFast(int durationSeconds)
		=> Stopwatch.ElapsedMilliseconds.Should().BeLessThan(durationSeconds * 1000);

	protected static long DaysAgoAsUnixSeconds(int days)
		=> DateTimeOffset.UtcNow.AddDays(-days).ToUnixTimeSeconds();

	internal Task<Device> GetNetflowDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Device>(NetflowDeviceId, cancellationToken);

	protected Task<Device> GetServiceDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Device>(ServiceDeviceId, cancellationToken);

	protected Task<Device> GetWindowsDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Device>(WindowsDeviceId, cancellationToken);

	protected Task<Device> GetSnmpDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Device>(SnmpDeviceId, cancellationToken);

	protected Task<Dashboard> GetAllWidgetsDashboardAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Dashboard>(AllWidgetsDashboardId, cancellationToken);
}
