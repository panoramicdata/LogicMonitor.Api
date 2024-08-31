namespace LogicMonitor.Api.Test;

internal sealed class TestPortalConfig
{
	public string Account { get; set; }

	public string AccessId { get; set; }

	public string AccessKey { get; set; }

	public int NetflowDeviceId { get; }

	public int SnmpDeviceId { get; set; }

	public int ReportId { get; set; }

	public int WindowsDeviceId { get; set; }

	public int WindowsDeviceLargeDeviceDataSourceId { get; set; }

	public int ServiceDeviceId { get; set; }

	public bool AccountHasBillingInformation { get; set; }

	public int AllWidgetsDashboardId { get; set; }

	public string WebsiteGroupFullPath { get; set; }

	public string DeviceGroupFullPath { get; set; }

	public string ResourceGroupFullPath { get; set; }

	public string WebsiteName { get; set; }

	public int DownCollectorId { get; set; }

	public int CollectorId { get; set; }

	public int SdtResourceGroupId { get; set; }

	public string AlertRuleName { get; set; }

	public int TestDashboardId { get; set; }
}
