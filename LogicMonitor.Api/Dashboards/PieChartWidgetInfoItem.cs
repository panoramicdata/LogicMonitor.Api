namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Pie chart widget info items
/// </summary>
[DataContract]
public class PieChartWidgetInfoItem
{
	/// <summary>
	/// The data point name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; }

	/// <summary>
	/// The legend
	/// </summary>
	[DataMember(Name = "legend")]
	public string Legend { get; set; }

	/// <summary>
	/// The color
	/// </summary>
	[DataMember(Name = "color")]
	public string Color { get; set; }
}
