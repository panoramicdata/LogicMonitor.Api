namespace LogicMonitor.Api.Alerts;

/// <summary>
/// AlertStatus extension methods
/// </summary>
public static class AlertStatusExtensions
{
	/// <summary>
	/// The alert level, based on the alert status
	/// </summary>
	/// <param name="alertStatus"></param>
	public static AlertLevel? GetAlertLevel(this AlertStatus alertStatus)
	{
		var alertStatusString = alertStatus.ToString();
		if (alertStatusString.Contains("Critical"))
		{
			return AlertLevel.Critical;
		}
		if (alertStatusString.Contains("Error"))
		{
			return AlertLevel.Error;
		}
		if (alertStatusString.Contains("Warn"))
		{
			return AlertLevel.Warning;
		}
		return null;
	}

	/// <summary>
	/// The SDT status, based on the alert status
	/// </summary>
	/// <param name="alertStatus"></param>
	public static bool IsInSdt(this AlertStatus alertStatus) => alertStatus.ToString().Contains("Sdt");

	/// <summary>
	/// The acknowledgement status, based on the alert status
	/// </summary>
	/// <param name="alertStatus"></param>
	public static bool IsAcknowledged(this AlertStatus alertStatus) => alertStatus.ToString().Contains("Confirmed");
}
