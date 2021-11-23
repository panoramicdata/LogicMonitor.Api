using LogicMonitor.Api.Time;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Text Widget
/// Produces (e.g.): {
///	"name":"lon0685.london.schroders.com-CPU-WinCPU",
///	"description":"desc",
///	"type":"ngraph",
///	"hostName":"lon0685.london.schroders.com",
///	"hId":1717,
///	"dsiId":61863067,
///	"graphId":62,
///	"timescale":"1day",
///	"theme":"newBorderGray",
///	"dashboardId":"50",
///	"rowFilters":"[]"}}
/// </summary>
public class NormalGraphWidgetCreationDto : WidgetCreationDto<NGraphWidget>
{
	/// <summary>
	/// The device Id
	/// </summary>
	[DataMember(Name = "hId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The device Id
	/// </summary>
	[DataMember(Name = "dsiId")]
	public int DeviceDataSourceInstanceId { get; set; }

	/// <summary>
	/// The graph Id
	/// </summary>
	[DataMember(Name = "graphId")]
	public int GraphId { get; set; }

	/// <summary>
	/// The time period
	/// </summary>
	[DataMember(Name = "timescale")]
	public TimePeriod TimePeriod { get; set; }

	/// <summary>
	/// The row filters
	/// </summary>
	[DataMember(Name = "rowFilters")]
	public string RowFilters { get; set; } = "[]";

	/// <inheritdoc />
	public override string Type { get; } = "ngraph";
}
