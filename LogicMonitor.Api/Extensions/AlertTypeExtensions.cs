namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Alert Type Extensions
/// </summary>
internal static class AlertTypeExtensions
{
	/// <summary>
	/// Convert an alert type to its query string
	/// </summary>
	/// <param name="alertType"></param>
	public static string GetQueryString(this AlertType alertType) => alertType switch
	{
		AlertType.DataSource => "dataSourceAlert",
		AlertType.EventSource => "eventAlert",
		AlertType.Website => "websiteAlert",
		AlertType.DeviceCluster => "hostClusterAlert",
		AlertType.BatchJob => "batchJobAlert",
		AlertType.CollectorDown => "agentDownAlert",
		AlertType.CollectorFailover => "agentFailoverAlert",
		AlertType.CollectorFailBack => "agentFailBackAlert",
		AlertType.AlertThrottled => "alertThrottledAlert",
		AlertType.Log => "logAlert",
		_ => throw new NotSupportedException()
	};
}
