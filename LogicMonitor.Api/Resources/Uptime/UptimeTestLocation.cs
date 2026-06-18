namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// The set of locations from which an Uptime check is performed.
///
/// For internal checks supply <see cref="CollectorIds"/> (and set <see cref="All"/> to <c>false</c>);
/// for external checks supply <see cref="SmgIds"/> (Site Monitor Group ids).
/// </summary>
public class UptimeTestLocation
{
	/// <summary>
	/// Whether all available locations are used. For an internal check that pins specific Collectors,
	/// this should be <c>false</c>.
	/// </summary>
	public bool All { get; set; }

	/// <summary>
	/// The Collector ids that perform an internal check.
	/// </summary>
	public List<int> CollectorIds { get; set; } = [];

	/// <summary>
	/// The Site Monitor Group ids that perform an external check.
	/// </summary>
	public List<int> SmgIds { get; set; } = [];
}

