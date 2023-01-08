namespace LogicMonitor.Api.Reports;

/// <summary>
/// An interfaces bandwidth report
/// </summary>
[DataContract]
public class InterfacesBandwidthReport : DateRangeReport
{
	/// <summary>
	/// The hostsVal
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string HostsVal { get; set; }

	/// <summary>
	/// The hostsValType
	/// </summary>
	[DataMember(Name = "hostsValType")]
	public string HostsValType { get; set; }

	/// <summary>
	/// The rowFormat
	/// </summary>
	[DataMember(Name = "rowFormat")]
	public int RowFormat { get; set; }

	/// <summary>
	/// Whether the report is in Base 1024
	/// </summary>
	[DataMember(Name = "isBase1024")]
	public bool IsBase1024 { get; set; }

	/// <summary>
	/// Whether the report is in Base 1024
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<Metric> Metrics { get; set; }

	/// <summary>
	/// Whether the report is top 10 only
	/// </summary>
	[DataMember(Name = "top10Only")]
	public bool IsTop10Only { get; set; }
}
