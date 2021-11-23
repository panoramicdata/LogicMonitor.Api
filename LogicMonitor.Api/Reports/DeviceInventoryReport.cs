using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports;

/// <summary>
/// A host inventory report
/// </summary>
[DataContract]
public class DeviceInventoryReport : Report
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
	/// The columns to sort by
	/// </summary>
	[DataMember(Name = "sortedBy")]
	public string SortedBy { get; set; }

	/// <summary>
	/// The properties
	/// </summary>
	[DataMember(Name = "properties")]
	public List<string> Properties { get; set; }

	/// <summary>
	/// The metrics
	/// </summary>
	public List<DeviceInventoryReportMetric> Metrics { get; set; }
}
