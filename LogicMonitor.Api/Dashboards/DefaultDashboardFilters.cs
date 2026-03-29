namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     Default dashboard filters
/// </summary>
[DataContract]
public class DefaultDashboardFilters
{
	/// <summary>
	///     The default dashboard filter details
	/// </summary>
	[DataMember(Name = "defaultDashboardFilterDetails")]
	public List<DefaultDashboardFilterDetail> DefaultDashboardFilterDetails { get; set; } = [];
}
