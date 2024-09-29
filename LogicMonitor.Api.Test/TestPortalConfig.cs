namespace LogicMonitor.Api.Test;

public sealed class TestPortalConfig
{
	public required string Account { get; init; }

	public required string AccessId { get; init; }

	public required string AccessKey { get; init; }

	public required int NetflowDeviceId { get; init; }

	public required int SnmpDeviceId { get; init; }

	public required int ReportId { get; init; }

	public required int WindowsDeviceId { get; init; }

	public required int WindowsDeviceLargeDeviceDataSourceId { get; init; }

	public required int ServiceDeviceId { get; init; }

	public required bool AccountHasBillingInformation { get; init; }

	public required int AllWidgetsDashboardId { get; init; }

	public required string WebsiteGroupFullPath { get; init; }

	public required string DeviceGroupFullPath { get; init; }

	public required string ResourceGroupFullPath { get; init; }

	public required string WebsiteName { get; init; }

	public required int DownCollectorId { get; init; }

	public required int CollectorId { get; init; }

	public required int SdtResourceGroupId { get; init; }

	public required string AlertRuleName { get; init; }

	public required int TestDashboardId { get; init; }
}
