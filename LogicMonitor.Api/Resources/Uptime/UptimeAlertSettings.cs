namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// Alert tuning shared by all Uptime check types.
/// </summary>
public class UptimeAlertSettings
{
	/// <summary>
	/// The alert level applied to the overall check. Defaults to <see cref="Level.Warning"/>.
	/// </summary>
	public Level OverallAlertLevel { get; set; } = Level.Warning;

	/// <summary>
	/// The alert level applied to an individual checkpoint when <see cref="IndividualCheckpointAlertsEnabled"/> is set.
	/// Defaults to <see cref="Level.Warning"/>.
	/// </summary>
	public Level IndividualAlertLevel { get; set; } = Level.Warning;

	/// <summary>
	/// Whether an alert is raised when an individual checkpoint meets the alert condition.
	/// </summary>
	public bool IndividualCheckpointAlertsEnabled { get; set; }

	/// <summary>
	/// The number of consecutive failed checks before an alert is triggered.
	/// Supported values are 1-10, 30 and 60. Defaults to 1.
	/// </summary>
	public int FailedCheckCountBeforeAlerting { get; set; } = 1;

	/// <summary>
	/// The minimum number of test locations that must fail to trigger an alert.
	/// </summary>
	public SiteMonitorAlertCondition AlertCondition { get; set; } = SiteMonitorAlertCondition.AllLocations;
}

