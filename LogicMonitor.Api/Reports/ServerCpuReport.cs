namespace LogicMonitor.Api.Reports;

/// <summary>
/// A host CPU report
/// </summary>
[DataContract]
public class ServerCpuReport : DateRangeReport
{
	/// <summary>
	/// The hostsVal
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string HostsVal { get; set; }

	/// <summary>
	/// Whether to only show the top 10
	/// </summary>
	[DataMember(Name = "top10Only")]
	public bool Top10Only { get; set; }

	/// <summary>
	/// Whether to display graphs
	/// </summary>
	[DataMember(Name = "displayGraphs")]
	public bool DisplayGraphs { get; set; }
}
