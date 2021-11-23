namespace LogicMonitor.Api.Reports;

/// <summary>
/// A NetflowDeviceMetric report
/// </summary>
[DataContract]
public class NetflowDeviceMetricReport : DateRangeReport
{
	/// <summary>
	/// The hostsVal
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string HostsVal { get; set; }

	/// <summary>
	/// Whether to include DNS mappings
	/// </summary>
	[DataMember(Name = "includeDNSMappings")]
	public bool IncludeDnsMappings { get; set; }
}
