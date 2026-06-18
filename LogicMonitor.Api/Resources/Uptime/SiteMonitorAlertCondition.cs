namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// The minimum number of test locations that must fail in order to trigger an alert
/// (the <c>globalSmAlertCond</c> value documented for LM Uptime devices).
/// </summary>
public enum SiteMonitorAlertCondition
{
	/// <summary>
	/// All locations must fail.
	/// </summary>
	AllLocations = 0,

	/// <summary>
	/// Half of the locations must fail.
	/// </summary>
	HalfOfLocations = 1,

	/// <summary>
	/// More than one location must fail.
	/// </summary>
	MoreThanOneLocation = 2,

	/// <summary>
	/// Any single location failing is sufficient.
	/// </summary>
	AnyLocation = 3,
}
