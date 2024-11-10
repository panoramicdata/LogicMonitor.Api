namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     Overview graph creation DTO
/// </summary>
public class OverviewGraphWidgetCreationDto : WidgetCreationDto<OverviewGraphWidget>
{
	/// <summary>
	///     The Resource id
	/// </summary>
	[DataMember(Name = "hId")]
	public int ResourceId { get; set; }

	/// <summary>
	///     The ResourceDataSourceInstanceGroupId
	/// </summary>
	[DataMember(Name = "dsigId")]
	public int ResourceDataSourceInstanceGroupId { get; set; }

	/// <summary>
	///     The overview graph Id
	/// </summary>
	[DataMember(Name = "graphId")]
	public int GraphId { get; set; }

	/// <summary>
	///     The graph Id
	/// </summary>
	[DataMember(Name = "timescale")]
	public TimePeriod TimePeriod { get; set; }

	/// <summary>
	///     The graph Id
	/// </summary>
	[DataMember(Name = "rowFilters")]
	public string RowFilters { get; set; } = "[]";

	/// <inheritdoc />
	public override string Type { get; } = "ograph";
}
