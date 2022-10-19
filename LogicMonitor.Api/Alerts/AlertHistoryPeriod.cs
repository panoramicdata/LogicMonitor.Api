namespace LogicMonitor.Api.Alerts;

/// <summary>
/// The periods supported for alert history retrieval
/// </summary>
public enum AlertHistoryPeriod : int
{
	/// <summary>
	/// A custom period specified by start and end
	/// </summary>
	Custom = 0,

	/// <summary>
	/// The last 24 hours
	/// </summary>
	Last24Hours = 1,

	/// <summary>
	/// The last 7 days
	/// </summary>
	Last7Days = 2,

	/// <summary>
	/// The last 30 days
	/// </summary>
	Last30Days = 3,
}
