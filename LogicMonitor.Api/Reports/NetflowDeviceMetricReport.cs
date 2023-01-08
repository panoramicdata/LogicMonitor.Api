namespace LogicMonitor.Api.Reports;

/// <summary>
/// A NetflowDeviceMetric report
/// </summary>
[DataContract]
public class NetflowDeviceMetricReport : DateRangeReport
{
	/// <summary>
	/// The hosts value
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string HostsValue { get; set; }

	/// <summary>
	/// The hosts value type
	/// </summary>
	[DataMember(Name = "hostsValType")]
	public HostsValueType HostsValueType { get; set; }

	/// <summary>
	/// Whether to include DNS mappings
	/// </summary>
	[DataMember(Name = "includeDNSMappings")]
	public bool IncludeDnsMappings { get; set; }
}
