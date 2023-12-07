namespace LogicMonitor.Api.Reports;

/// <summary>
/// An interfaces bandwidth report
/// </summary>
[DataContract]
public class InterfBandwidthReport : DateRangeReport
{
	/// <summary>
	/// The devices OR groups selected for the report, where multiple entities are separated by commas
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string HostsVal { get; set; } = string.Empty;

	/// <summary>
	/// host | group. The type of entities specified in the hostsVal field
	/// </summary>
	[DataMember(Name = "hostsValType")]
	public string HostsValType { get; set; } = string.Empty;

	/// <summary>
	/// 0 | 1\n0: Text only - metrics will be displayed in a tabular format.\n1: One graph per instance - metrics will be displayed in a tabular format and one graph will be displayed per instance
	/// </summary>
	[DataMember(Name = "rowFormat")]
	public int RowFormat { get; set; }

	/// <summary>
	/// true | false\nfalse: Scale the number using 1000 \ntrue: Scale the number using 1024
	/// </summary>
	[DataMember(Name = "isBase1024")]
	public bool IsBase1024 { get; set; }

	/// <summary>
	/// The datapoint or calculation on a datapoint that will be included in the report, where each datapoint/calculation is specified by three fields: dataSourceId, instances (glob is okay)
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<Metric> Metrics { get; set; } = [];

	/// <summary>
	/// true | false\nfalse: Metrics will be displayed for all selected devices or groups\ntrue: Metrics will only be displayed for the top ten device or groups
	/// </summary>
	[DataMember(Name = "top10Only")]
	public bool IsTop10Only { get; set; }
}
